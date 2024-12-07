#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EstadosEnemy
{
    Stay = 0,
    Pursuit = 1,
    Atacking = 2,
    StayPatrol = 3,
    StayRandom = 4,
    LookAtPlayer
}
public class EnemyBehaviour : MonoBehaviour
{
    //private bool isPlayerInSight;
    //private float currentHealt;
    //private bool isConfused;

    [SerializeField] private EstadosEnemy startingtState;
    [SerializeField] private float pursuitThreshold;
    [SerializeField] private float attackingThreshold;
    [SerializeField] private float escapeThreshold;
    [SerializeField] private float damage;

    [SerializeField] private bool autoSelectTarget = true;
    [SerializeField] private Transform player;
    [SerializeField] private float distance;

    [SerializeField] private Transform[] puntosMov;
    [SerializeField] private float distminima;
    private int siguientepaso = 0;

    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Animator characterAnimator;

    private bool statePatrol;
    private bool stateStay;
    private bool stateRandom;

    private int rutina;
    private float cronometro;
    private Quaternion angulo;
    private float grado;


    //[SerializeField] private float damage;
    //private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        var diff = transform.position - player.transform.position;
        distance = diff.magnitude;
        //Todo
        switch (startingtState)
        {
            case EstadosEnemy.Stay:
                Stay();
                break;
            case EstadosEnemy.Pursuit:
                Pursuit();
                break;
            case EstadosEnemy.Atacking:
                Atacking();
                break;
            case EstadosEnemy.StayPatrol:
                Patrol();
                break;
            case EstadosEnemy.StayRandom:
                StayRandom();
                break;
            case EstadosEnemy.LookAtPlayer:
                LookRotationPlayer();
                break;
            default:
                break;
        }
    }

    private void ChangeState (EstadosEnemy e)
    {
        switch (e)
        {
            case EstadosEnemy.Stay:
                break;
            case EstadosEnemy.Pursuit:
                break;
            case EstadosEnemy.Atacking:
                break;
            case EstadosEnemy.StayPatrol:
                break;
            case EstadosEnemy.StayRandom:
                break;
            default:
                break;
        }
        startingtState = e;
    }

    private void Stay()
    {
        stateStay = true;
        statePatrol = false;
        stateRandom = false;
        
        if (distance < pursuitThreshold) 
        {
            ChangeState(EstadosEnemy.Pursuit);
        }
        characterAnimator.SetBool("isAtacking", false);
        characterAnimator.SetBool("isRunning", false);
    }

    //El enemigo gira hacia el jugador y lo persigue
    private void Pursuit()
    {
        if (distance < attackingThreshold)
        {
            ChangeState(EstadosEnemy.Atacking);
        }
        else if (distance > escapeThreshold) 
        {
            if (!statePatrol && !stateRandom && stateStay == true)
            {
                ChangeState(EstadosEnemy.Stay);
            }
            else if (!stateRandom && !stateStay &&  statePatrol == true)
            {
                ChangeState(EstadosEnemy.StayPatrol);
            }
            else
            {
                ChangeState(EstadosEnemy.StayRandom);
            }
        }
        characterAnimator.SetBool("isAtacking", false);
        characterAnimator.SetBool("isWalking", false);
        characterAnimator.SetBool("isRunning", true);
        LookRotationPlayer();
        transform.position += transform.forward * (Time.deltaTime * runSpeed);
    }

    private void Atacking()
    {
        if (distance > attackingThreshold + 0.4f)
        {
            ChangeState (EstadosEnemy.Pursuit);
        }
        characterAnimator.SetBool("isAtacking", true);
    }

    private void Patrol()
    {
        statePatrol = true;
        stateStay = false;
        stateRandom = false;

        if (distance < pursuitThreshold)
        {
            ChangeState(EstadosEnemy.Pursuit);
        }
        characterAnimator.SetBool("isAtacking", false);
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isWalking", true);

        if (puntosMov == null || puntosMov.Length == 0) return;

        Vector3 targetPosition = puntosMov[siguientepaso].position;
        Vector3 direction = (targetPosition - transform.position).normalized;


        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }


        transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < distminima)
        {
            siguientepaso = (siguientepaso + 1) % puntosMov.Length;
        }
    }

    private void StayRandom()
    {
        stateRandom = true;
        statePatrol = false;
        stateStay = false;
        if (distance < pursuitThreshold)
        {
            ChangeState(EstadosEnemy.Pursuit);
        }
        characterAnimator.SetBool("isAtacking", false);
        characterAnimator.SetBool("isRunning", false);
       
        cronometro += 1 * Time.deltaTime;
        if(cronometro >= 3)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }
        switch (rutina)
        {
            case 0:
                characterAnimator.SetBool("isWalking", false);
                break;
            case 1:
                grado = Random.Range(0, 360);
                angulo = Quaternion.Euler(0, grado, 0);
                rutina++;
                break;
            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                characterAnimator.SetBool("isWalking", true);
                break;
        }

    }

    //Modo mas simple de mirar al jugador 
    private void LookAtPlayer()
    {
        //transform.LookAt(player.transform.position);
    }

    //Modo alternativo para mirar al jugador
    private void LookRotationPlayer()
    {
        var newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, attackingThreshold);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, pursuitThreshold);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, escapeThreshold);

    }

#endif

    private void OnCollisionStay(Collision other)
    {
        var colliderGameObject = other.gameObject;
        //Necesito chequear la tag/label/etiqueta de el gameobject

        MainCharacter player = colliderGameObject.GetComponent<MainCharacter>();

        if (player != null) //Tiene el componente player
        {
            //Es un player
            Debug.Log("Choco contra el player");
            player.TakeDamage(damage * Time.fixedDeltaTime);
        }
    }

    
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif  


public enum EstadosEnemy
{
    Stay = 0,
    Pursuit = 1,
    Atacking = 2,
    Patrol = 3,
    Dead = 4,
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

    [SerializeField] private bool statePatrol;
    [SerializeField] private bool stateStay;

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
            case EstadosEnemy.Patrol:
                Patrol();
                break;
            case EstadosEnemy.Dead:
                Dead();
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
            case EstadosEnemy.Patrol:
                break;
            case EstadosEnemy.Dead:

                break;
            default:
                break;
        }
        startingtState = e;
    }

    private void Stay()
    {
        statePatrol = false;
        stateStay = true;
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
            if (!statePatrol && stateStay == true)
            {
                ChangeState(EstadosEnemy.Stay);
            }
            else 
            {
                ChangeState(EstadosEnemy.Patrol);
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
    private void Dead()
    {
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

   
    //private void OnCollisionStay(Collision other)
    //{
    //    var colliderGameObject = other.gameObject;
    //    //Necesito chequear la tag/label/etiqueta de el gameobject

    //    MainCharacter player = colliderGameObject.GetComponent<MainCharacter>();

    //    if (player != null) //Tiene el componente player
    //    {
    //        //Es un player
    //        Debug.Log("Choco contra el player");
    //        Atacking();
    //        player.TakeDamage(damage * Time.fixedDeltaTime);
    //    }
    //}
}
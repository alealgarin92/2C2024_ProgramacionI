using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private int pointsPerClick = 10;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación
    [SerializeField] private Bullet bullet;
    [SerializeField] private float shootingCooldownBase;
    [SerializeField] private PermanentBullet permanentBullet;
    [SerializeField] private Transform raycastOrigin;

    [SerializeField] private float maxHealth;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float health;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Vector3 startingRotation;

    [SerializeField] private Animator mouseAnimator;

    [SerializeField] private AudioSource pasos;
    //[SerializeField] private Initializer initializer;
    private bool Hactivo;
    private bool Vactivo;


    private EnemyBehaviour targetEnemy;

    private float shootingCooldown;

    private new Camera camera;
    Vector3 camForward, camRight, moveDir;
    private Vector3 movementDir;


    private void Start()
    {
        shootingCooldown = shootingCooldownBase;
        health = maxHealth;
        camera = Camera.main;

        //initializer.OnInitializeComplete += OnInitalizeCompleteHandler;
        //initializer.OnInitializeCompleteUnity.AddListener(OnInitalizeCompleteHandler);

        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            if (camera == null)
            {
                camera = Camera.main; // Obtiene la cámara principal si no está asignada
            }
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPoints();
        }

        //Mover utilizando WASD

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direccion = new Vector2 (horizontal, vertical);

        // Obtener la dirección forward y right de la cámara
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        // Asegurarse de que las direcciones estén en el plano horizontal
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Crear el vector de movimiento basado en la entrada del jugador y la orientación de la cámara
        // Crear dirección de movimiento
        movementDir = (cameraRight * horizontal + cameraForward * vertical).normalized;

        // Rotar al jugador de forma suave
        if (movementDir != Vector3.zero) // Asegurarse de que hay movimiento
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDir); // Rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        //Si el jugador presiona Space Y el cooldown ya se termino, dispara
        shootingCooldown -= Time.deltaTime;
        if (Input.GetButton("Shoot") && shootingCooldown <= 0)
        {
            shootingCooldown = shootingCooldownBase;
            Shoot();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Instantiate(permanentBullet, transform.position, transform.rotation); 
        //}
        

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            StartJump();

        }


        if (direccion.magnitude <= 0)
        {
            mouseAnimator.SetFloat("movements", 0, 0.1f, Time.deltaTime);
        }
        else if (direccion.magnitude >= 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            mouseAnimator.SetFloat("movements", 1, 0.1f, Time.deltaTime);
            movementSpeed = 4;
            
        }
        else 
        {
            mouseAnimator.SetFloat("movements", 0.5f, 0.1f, Time.deltaTime);
            movementSpeed = 2;   
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            Hactivo = true;
            pasos.Play();
        }
        if (Input.GetButtonDown("Vertical"))
        {
            Vactivo = true;
            pasos.Play();
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            Hactivo = false;
            if(Vactivo == false)
            {
                pasos.Pause();
            }
        }
        if (Input.GetButtonUp("Vertical"))
        {
            Vactivo = false;
            if (Hactivo == false)
            {
                pasos.Pause();
            }
        }
    }

 
    public void OnInitalizeCompleteHandler()
    {
        Debug.Log("Initialized has completed");
    }

    private void AddPoints()
    {
        GameManager.instance.AddPoints(pointsPerClick);
    }

    //private void FixedUpdate()
    //{
    //    // Movimiento físico
    //    Vector3 movementDir = (camRight * Input.GetAxis("Horizontal") + camForward * Input.GetAxis("Vertical")).normalized;
    //    rb.MovePosition(rb.position + movementDir * movementSpeed * Time.fixedDeltaTime);
    //}


    private void Jump()
    {
        //No puede saltar si el piso esta muy lejos
        bool hitGround =
            UnityEngine.Physics.Raycast(raycastOrigin.position, Vector3.down, jumpCheckDistance, groundLayer);

        if (hitGround)
        {
            Vector3 direction = Vector3.up; // Lo mismo que escribir new vector3(0,1,0);
            rb.AddForce(direction * jumpForce, ForceMode.Impulse);
            //PlayJumpSound();
        }

    }
    private void Shoot()
    {
        //Disparo
        Bullet instantiatedBullet = Instantiate(bullet, transform.position, transform.rotation);
        if (targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            instantiatedBullet.transform.forward = direction;
        }
        //PlayShootSound();
    }
    private void FixedUpdate()
    {
        // Mover al personaje usando el Rigidbody
        Vector3 newPosition = rb.position + movementDir * movementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    //Se realiza animacion de salto
    private void StartJump()
    {
        mouseAnimator.SetTrigger("Jump");
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + Vector3.down * jumpCheckDistance);
    }

    //private void StartWalking()
    //{
    //    mouseAnimator.SetBool("isWalking", true);
    //}

    //private void Idle()
    //{
    //    mouseAnimator.SetBool("isWalking", false);
    //}

    //private void PlayJumpSound()
    //{
    //    jumpSound.PlayOneShot(audioClip);
    //}

    //private void PlayShootSound()
    //{
    //    shootSound.Play();
    //}

    //if (Input.GetKey(KeyCode.W))
    //{
    //    movementDir.y += 1;
    //}
    //if (Input.GetKey(KeyCode.A))
    //{
    //    movementDir.x -= 1;
    //}
    //if (Input.GetKey(KeyCode.S))
    //{
    //    movementDir.y -= 1;
    //}
    //if (Input.GetKey(KeyCode.D))
    //{
    //    movementDir.x += 1;
    //}


}

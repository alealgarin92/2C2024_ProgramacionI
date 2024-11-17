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

    [SerializeField] private Animator oskar;

    [SerializeField] private AudioSource pasos;
    //[SerializeField] private Initializer initializer;
    private bool Hactivo;
    private bool Vactivo;


    private EnemyBehaviour targetEnemy;

    private float shootingCooldown;

    private new Camera camera;
    private Vector3 movementDir;
    private Vector3 move = Vector3.zero;

    private void Start()
    {
        shootingCooldown = shootingCooldownBase;
        health = maxHealth;
        camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

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

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direccion = new Vector2(horizontal, vertical);

        // Calculate camera relative directions to move:
        Vector3 camFwd = Vector3.Scale(camera.transform.forward, new Vector3(1, 1, 1)).normalized;
        Vector3 camFlatFwd = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 flatRight = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);

        Vector3 m_CharForward = Vector3.Scale(camFlatFwd, new Vector3(1, 0, 1)).normalized;
        Vector3 m_CharRight = Vector3.Scale(flatRight, new Vector3(1, 0, 1)).normalized;

        // Move the player (movement will be slightly different depending on the camera type)
        float w_speed = 0;

        w_speed = movementSpeed;
        move = (vertical * m_CharForward + horizontal * m_CharRight).normalized * w_speed;
        rb.transform.position += move * Time.deltaTime;

        // Rotate body
        rb.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(rb.transform.forward, move * Time.fixedDeltaTime, rotationSpeed, 0.0f));
        // Draws a ray to show the direction the player is aiming at
        //Debug.DrawLine(transform.position, transform.position + camFwd * 5f, Color.red);

        //// Obtener la dirección forward y right de la cámara
        //Vector3 cameraForward = camera.transform.forward;
        //Vector3 cameraRight = camera.transform.right;

        //// Asegurarse de que las direcciones estén en el plano horizontal
        //cameraForward.y = 0f;
        //cameraRight.y = 0f;
        //cameraForward.Normalize();
        //cameraRight.Normalize();

        //// Crear el vector de movimiento basado en la entrada del jugador y la orientación de la cámara
        //// Crear dirección de movimiento
        //movementDir = (cameraRight * horizontal + cameraForward * vertical).normalized;

        //// Rotar al jugador de forma suave
        //if (movementDir != Vector3.zero) // Asegurarse de que hay movimiento
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(movementDir); // Rotación objetivo
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}


        //Si el jugador presiona Space Y el cooldown ya se termino, dispara
        shootingCooldown -= Time.deltaTime;
        if (Input.GetButton("Shoot") && shootingCooldown <= 0)
        {
            shootingCooldown = shootingCooldownBase;
            Shoot();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            StartJump();

        }

        if (direccion.magnitude <= 0)
        {
            oskar.SetFloat("movements", 0, 0.1f, Time.deltaTime);
        }
        else if (direccion.magnitude >= 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            oskar.SetFloat("movements", 1, 0.1f, Time.deltaTime);
            movementSpeed = 4;
            
        }
        else 
        {
            oskar.SetFloat("movements", 0.5f, 0.1f, Time.deltaTime);
            movementSpeed = 2;   
        }

        if(Input.GetKey(KeyCode.C))
        {
            oskar.SetFloat("crouch", 0, 0.1f, Time.deltaTime);

        }
        //else if (direccion.magnitude >= 0.1f && Input.GetKey(KeyCode.C))
        //{
        //    oskar.SetFloat("crouch", 1f, 0.1f, Time.deltaTime);
        //    movementSpeed = 2;
        //}

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

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPoints();
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
        oskar.SetTrigger("Jump");
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
    //private void FixedUpdate()
    //{
    //    // Movimiento físico
    //    Vector3 movementDir = (camRight * Input.GetAxis("Horizontal") + camForward * Input.GetAxis("Vertical")).normalized;
    //    rb.MovePosition(rb.position + movementDir * movementSpeed * Time.fixedDeltaTime);
    //}
    //if (Input.GetKeyDown(KeyCode.Space))
    //{
    //    Instantiate(permanentBullet, transform.position, transform.rotation); 
    //}

}

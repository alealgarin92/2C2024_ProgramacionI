using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private int pointsPerClick = 10;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación
    [SerializeField] private Transform raycastOrigin;

    [SerializeField] private float maxHealth;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float health;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCheckDistance;
    [SerializeField] private LayerMask groundLayer;

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
    private bool isCrouching = false;
    private bool isAiming = false;

    [SerializeField] private Vector2 mouseSensitivity;
    
    private void Start()
    {
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

        // Calcular direcciones relativas a la cámara
        Vector3 camFlatFwd = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 flatRight = Vector3.Scale(camera.transform.right, new Vector3(1, 0, 1)).normalized;

        Vector3 m_CharForward = Vector3.Scale(camFlatFwd, new Vector3(1, 0, 1)).normalized;
        Vector3 m_CharRight = Vector3.Scale(flatRight, new Vector3(1, 0, 1)).normalized;

        // Movimiento del jugador
        float w_speed = movementSpeed;
        move = (vertical * m_CharForward + horizontal * m_CharRight).normalized * w_speed;

        if (Input.GetMouseButton(1)) // Mantener clic izquierdo
        {
            // Actualizar blend tree con la dirección de movimiento
            oskar.SetFloat("VelX", horizontal, 0.1f, Time.deltaTime);
            oskar.SetFloat("VelY", vertical, 0.1f, Time.deltaTime);

            // Actualizar estado de apuntado
            if (!isAiming)
            {
                isAiming = true;
                oskar.SetBool("isAiming", isAiming); // Activar Blend Tree de apuntado
                movementSpeed = 2; // Ajustar velocidad al apuntar
            }

            // Mirar hacia la dirección del mouse
            LookAtMouseDirection();

            // Aplicar movimiento
            rb.MovePosition(rb.position + move * Time.deltaTime);
        }
        else
        {
            // Detener el estado de apuntado si se soltó el clic
            if (isAiming)
            {
                isAiming = false;
                oskar.SetBool("isAiming", isAiming); // Volver al Blend Tree normal
                movementSpeed = 2; // Ajustar velocidad al estado normal
            }

            // Manejo de agachado
            if (Input.GetKeyDown(KeyCode.C))
            {
                isCrouching = !isCrouching; // Cambiar estado de agachado
                oskar.SetBool("isCrouching", isCrouching); // Activar Blend Tree correspondiente
                movementSpeed = isCrouching ? 1 : 2; // Ajustar velocidad
            }

            // Determinar el estado de movimiento
            if (direccion.magnitude <= 0) // Quieto
            {
                oskar.SetFloat("movements", 0, 0.1f, Time.deltaTime); // Estado Idle
            }
            else if (direccion.magnitude > 0 && !isCrouching && Input.GetKey(KeyCode.LeftShift)) // Correr
            {
                oskar.SetFloat("movements", 1, 0.1f, Time.deltaTime); // Estado Correr
                movementSpeed = 4;
            }
            else if (direccion.magnitude > 0 && isCrouching) // Caminar Agachado
            {
                oskar.SetFloat("movements", 0.25f, 0.1f, Time.deltaTime); // Estado Caminar Agachado
            }
            else if (direccion.magnitude > 0) // Caminar Normal
            {
                oskar.SetFloat("movements", 0.5f, 0.1f, Time.deltaTime); // Estado Caminar Normal
                movementSpeed = 2;
            }

            // Aplicar movimiento
            rb.MovePosition(rb.position + move * Time.deltaTime);
        

        // Rotar cuerpo hacia la dirección de movimiento
        if (move != Vector3.zero)
            {
                rb.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(rb.transform.forward, move, rotationSpeed * Time.deltaTime, 0.0f));
            }  
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            StartJump();

        }

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AddPoints();
        //}

    }

    public void OnInitalizeCompleteHandler()
    {
        Debug.Log("Initialized has completed");
    }

    private void AddPoints()
    {
        GameManager.instance.AddPoints(pointsPerClick);
    }

    private void LookAtMouseDirection()
    {
        // Crear un rayo desde la posición del mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Plano en el nivel del suelo

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            // Obtener el punto en el mundo hacia donde apunta el mouse
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 directionToLook = (pointToLook - rb.position).normalized;
            directionToLook.y = 0; // Ignorar la componente vertical

            // Orientar al jugador hacia el punto
            rb.transform.forward = directionToLook;
        }
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
    //[SerializeField] private Bullet bullet;
    //[SerializeField] private float shootingCooldownBase;
    //[SerializeField] private PermanentBullet permanentBullet;
    //private void Shoot()
    //{
    //    //Disparo
    //    Bullet instantiatedBullet = Instantiate(bullet, transform.position, transform.rotation);
    //    if (targetEnemy != null)
    //    {
    //        Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
    //        instantiatedBullet.transform.forward = direction;
    //    }
    //    //PlayShootSound();
    //}
    //shootingCooldown = shootingCooldownBase;
    //[SerializeField] private Vector3 startingRotation;


                //TIMER

    //[SerializeField] private float initialTemp;

    //private float currentTime;

    //private void Awake()
    //{
    //    currentTime = initialTemp;
    //}

    //private void LateUpdate()
    //{
    //    currentTime -= Time.deltaTime;

    //    if (currentTime <= 0) ;
    //    Action();
    //}
}



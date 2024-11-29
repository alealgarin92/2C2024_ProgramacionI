using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación
    [SerializeField] private float movementSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float CrouchingSpeed;
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastLanternOrigin;

    [SerializeField] private float maxHealth;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float health;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCheckDistance;
    [SerializeField] private float enemyCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float damagePerTick;

    [SerializeField] private Animator oskar;

    private bool Hactivo;
    private bool Vactivo;


    //Patalla de derrota, victoria y pausa
    [SerializeField] private GameObject pantallaMenuDerrota;

    //Barra de vida
    [SerializeField] private Image barraDeEstres;

    private bool linternaEncendida = false; // Estado de la linterna (encendida/apagada)

    private EnemyBehaviour targetEnemy;
    private Enemy enemy;

    public float saldoSube;

    private new Camera camera;
    private float shootingCooldown;

    private MenuInicial menu;

    public int cantSube = 0;
    public int cantLlaves = 0;

    private Vector3 movementDir;
    private Vector3 move = Vector3.zero;
    private bool isCrouching = false;
    private bool isAiming = false;

    //Prueba de vida con luz y sombra
    public float lifeChangeRate = 5f; // Cuánto aumenta o disminuye la vida por segundo
    private Light currentLight; // Referencia a la luz que afecta al personaje
    private bool isInLight = false; // Verifica si está en la luz o en la sombra

    private void Start()
    {
        health = maxHealth;
        // Inicializa la salud al máximo al inicio del juego


        //Linea que nos ayuda a bloquear el puntero una vez presionado play
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;

        // Asegúrate de que todo esté limpio al comenzar la escena
        pantallaMenuDerrota.SetActive(false);
        Time.timeScale = 1f;

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
        rb.freezeRotation = true; // Desactiva rotaciones automáticas
    }


    private void Update()
    {
        //Mover utilizando WASD

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

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
                movementSpeed = isCrouching ? CrouchingSpeed : walkSpeed; // Ajustar velocidad
            }

            // Determinar el estado de movimiento
            if (direction.magnitude <= 0) // Quieto
            {
                oskar.SetFloat("movements", 0, 0.1f, Time.deltaTime); // Estado Idle
            }
            else if (direction.magnitude > 0 && !isCrouching && Input.GetKey(KeyCode.LeftShift)) // Correr
            {
                oskar.SetFloat("movements", 1, 0.1f, Time.deltaTime); // Estado Correr
                movementSpeed = runSpeed;
            }
            else if (direction.magnitude > 0 && isCrouching) // Caminar Agachado
            {
                oskar.SetFloat("movements", 0.25f, 0.1f, Time.deltaTime); // Estado Caminar Agachado
            }
            else if (direction.magnitude > 0) // Caminar Normal
            {
                oskar.SetFloat("movements", 0.5f, 0.1f, Time.deltaTime); // Estado Caminar Normal
                movementSpeed = walkSpeed;
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

        FlashLightEnemy();

        barraDeEstres.fillAmount = health / maxHealth;

        //PRUEBA DE LUCES Y SOMBRAS

        // Cambia la salud dependiendo de si está en la luz o en la sombra
        if (isInLight)
        {
            IncreaseHealth(lifeChangeRate * Time.deltaTime);
        }
        else
        {
            DecreaseHealth(lifeChangeRate * Time.deltaTime);
        }

        // Limita la salud entre 0 y el máximo
        health = Mathf.Clamp(health, 0, maxHealth);

        // Si la salud llega a 0, puedes manejar la "muerte" del personaje aquí
        if (health <= 0)
        {
            PantallaDerrota();
            Debug.Log("El personaje ha muerto.");

        }

    }

    void IncreaseHealth(float amount)
    {
        health += amount;
    }

    void DecreaseHealth(float amount)
    {
        health -= amount;
    }
    // Detecta si el personaje entra en una zona de luz
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            isInLight = true;
        }
    }

    // Detecta si el personaje sale de una zona de luz
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            isInLight = false;
        }
    }

    //-------------------------------------

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
        }
    }


    private void FlashLightEnemy()
    {
        // Realiza el Raycast cada frame mientras la linterna esta encendida
        if (Physics.Raycast(raycastLanternOrigin.position, raycastLanternOrigin.forward, out RaycastHit hit, enemyCheckDistance, enemyLayer))
        {
            // Checkea si el objeto con el que choca el rayo tiene el componente Enemy
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Resta vida al enemigo 
                enemy.TakeDamage(damagePerTick * Time.fixedDeltaTime);
            }
        }
    }


    //Se realiza animacion de salto


    private void StartJump()
    {
        oskar.SetTrigger("Jump");
    }


    public void Heal(float healAmount)
    {
        if (health < 100)
        {
            health += healAmount;
        }
    }

    //Funcion para visualizar el raycast de salto y el de deteccion de enmigos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + Vector3.down * jumpCheckDistance);

        Gizmos.DrawLine(raycastLanternOrigin.position, raycastLanternOrigin.position + transform.forward * enemyCheckDistance);
    }


    public void CargaSube(float dinero)
    {
        if (saldoSube < 1000)
        {
            saldoSube += dinero;
        }
    }
    public void ApoyaSube(float restaSaldo)
    {
        if (saldoSube > 500)
        {
            saldoSube -= restaSaldo;

        }
    }

    public void Salir()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Cargar menu principal
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        PantallaDerrota();

    }

    private void PantallaDerrota()
    {
        if (health <= 0)
        {
            pantallaMenuDerrota.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCharacter : MonoBehaviour
{
    private CharacterController controller;
    private GameObject camera;

    [Header ("normal statistics")]
    [SerializeField] private float speed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float turningTime;

    [Header ("Data about the floor")]
    [SerializeField] private Transform floorDetect;
    [SerializeField] private float floorDistance;
    [SerializeField] private LayerMask floorMask;

    float turningSpeed;
    float gravity = -9.81f;
    Vector3 velocity;
    bool hitsFloor;

    Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        hitsFloor = Physics.CheckSphere(floorDetect.position, floorDistance, floorMask);


        if (hitsFloor && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && hitsFloor)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }   

        velocity.y += gravity * 3 * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3 (horizontal, 0f,  vertical).normalized;

        if (direction.magnitude <= 0)
        {
            anim.SetFloat("movements", 0, 0.1f, Time.deltaTime);
        }

        if(direction.magnitude >= 0.1f)
        {
            float angleTarget = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleTarget, ref turningSpeed, turningTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 run = Quaternion.Euler(0, angleTarget, 0) * Vector3.forward;
                controller.Move(run.normalized * runningSpeed * Time.deltaTime);
                anim.SetFloat("movements", 1f, 0.1f, Time.deltaTime);
            }
            else
            {
                Vector3 walk = Quaternion.Euler(0, angleTarget, 0) * Vector3.forward;
                controller.Move(walk.normalized * speed * Time.deltaTime);
                anim.SetFloat("movements", 0.5f, 0.05f, Time.deltaTime);
            }

            
        }
    }
}

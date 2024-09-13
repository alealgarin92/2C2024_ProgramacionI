using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorquePusher : MonoBehaviour
{
    [SerializeField] private float pusherForce;
    [SerializeField] private Vector3 pushDirection;
    [SerializeField] private float initialTemp;
    [SerializeField] Rigidbody rb;
    [SerializeField] private Transform forcePosition;

    private float currentTime;

    private void Awake()
    {
        currentTime = initialTemp;
    }


    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ApplyTorque();
        }
    }

    private void ApplyTorque()
    {
        rb.AddTorque(pushDirection * pusherForce, ForceMode.Impulse);
    }
}

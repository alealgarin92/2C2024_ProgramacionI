using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float timeToDestroy;

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }


    // Update is called once per frame
    void Update()
    {
        //timeToDestroy -= Time.deltaTime;
        //if (timeToDestroy <= 0)
        //{
        //    //Destruyo la bala  
        //    Destroy(gameObject);
        //}
        Move();
    }

    private void Move()
    {
        transform.position += movementSpeed * transform.forward;
    }
}

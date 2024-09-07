using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float damage;

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }


    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            //Destruyo la bala  
            Destroy(gameObject);
        }
        Move();
    }

    private void Move()
    {
        transform.position += movementSpeed * transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        var colliderGameObject = other.gameObject;
        //Necesito chequear la tag/label/etiqueta de el gameobject

        Enemy enemy = colliderGameObject.GetComponent<Enemy>();

        if (enemy != null) //Tiene el componente enemy
        {
            //Es un enemy
            Debug.Log("Choco contra el enemigo");
            enemy.TakeDamage(damage);
        }
        else
        {
            //No es un enemy
            Debug.Log("No choco contra el enemigo");
        }

        Destroy(gameObject);
    }
}

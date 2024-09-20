using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int age;
    [SerializeField] private double gravity;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isAlive;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private string characterName;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 movementDirection;
    [SerializeField] private Vector3 scalingDir;
    [SerializeField] private float scalingVelocity;
    [SerializeField] private Vector3 rotationDir;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject myGameObject;


    private void Awake()
    {
        Shoot();
    }
    private void Start()
    {
        Hello();
       
    }

    private void Update()
    {
        Move();
        CheckDead();
        UpdateMyObject();
    }

    private void UpdateMyObject()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ActivateGameObject(true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ActivateGameObject(false);
        }
    }
    private void Hello()
    {
        Debug.Log("Hello" + GetInfo());
    }

    private string GetInfo()
    {
        string info;
        info = characterName;
        info = info + age;
        return info;
    }

    private void Heal(float healAmount)
    {
        health += healAmount;
        bool hasMaxHealth = health == maxHealth;
        bool hasHealthMissing = health != maxHealth;
    }

    private void hasSpecialName()
    {
        bool hasSpecialName = characterName == "George";
        bool hasRegularName = characterName != "George";
    }

    private void HasSpecialAbility()
    {
        //Si el personaje se llama George o Jorge es especial.
        bool hasSpecialName = characterName == "George" || characterName == "Jorge";
        bool hasSpecialAbility = isAlive && hasSpecialName;
    }
    

    private void ComplexOperation()
    {
        int number = 5;
        number += 2;
        number *= 3;
    }

    private void CheckDead()
    {
        // Voy a checkear que el personaje tenga mas que 0 de vida
        isAlive = health > 0;
        isDead = !isAlive;

        //Imprimir un mensaje de que el personaje esta vivo
        //if (isAlive)
        //{
        //    Debug.Log("it's Alive");
        //}
        //else
        //{
        //    Debug.Log("it's Dead");
        //}

        //Si el personaje esta vivo, imprime el msj Its alive
        //Si el personaje no esta vivo y tiene una velocidad mayor a 0, pongo su velocidad en 0
        //Si el personaje no esta vivo y su velocidad es <= 0, imprimo que esta completamente muerto

        if (isAlive)
        {
            //Debug.Log("it's Alive");
        }
        else if(speed > 0)
        {
            speed = 0;
        }
        else
        {
            //Debug.Log("Its dead");
        }

        if(!isAlive && speed > 0)
        {
            speed = 0;
        }
        else
        {
            //Debug.Log("Its dead");
        }

    }

    private void Move()
    {
        //Agarrar la posicion del personaje
        //Vector3 position = transform.position;
        //Sumarle una cantidad de movimiento
        //position += movementDirection * speed;
        //Escribir de vuelta la posicion
        //transform.position = position;    

        //Si esta vivo y la velocidad es mayor a 0, entonces se mueve
        if(isAlive && speed > 0)
        {
            transform.position += movementDirection * speed;
            transform.localScale += scalingDir * scalingVelocity;
            transform.Rotate(rotationDir, rotationSpeed);
        }
        
    }

    private void Shoot()
    {
        //Hacer aparecer un prefab de bala
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        //Moverlo
    }

    private void ActivateGameObject(bool isActive)
    {
        myGameObject.SetActive(isActive);
    }
}

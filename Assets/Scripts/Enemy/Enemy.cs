using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     private float maxHealth = 100;
    [SerializeField] private float health;
    
    private void Awake()
    {
        health = maxHealth;  
    }

    public void TakeDamage(float damage)
    {
        health -= damage * Time.fixedDeltaTime;

        //if (health <= 30)
        //{
        //    enemy.Flee();
            
        //}
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

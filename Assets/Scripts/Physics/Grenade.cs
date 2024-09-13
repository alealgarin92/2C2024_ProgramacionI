using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float timer;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float upwardsModifier;
    [SerializeField] private LayerMask collisionLayer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider[] collisions = UnityEngine.Physics.OverlapSphere(transform.position, explosionRadius, collisionLayer);

        if (collisions.Length > 0)
        {
            foreach (Collider coll in collisions)
            {
                //Empujar a cada rigidbody
                var collidedRigidBody = coll.GetComponent<Rigidbody>();
                if(collidedRigidBody != null)
                {
                    collidedRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                }
                
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, explosionRadius);
    //}
}

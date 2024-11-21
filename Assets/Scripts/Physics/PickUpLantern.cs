using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLantern : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject linternaPlayer;
    [SerializeField] private SoundController sonidoLinterna;

    [SerializeField] private float pickUpRadius;
    [SerializeField] private LayerMask collisionLayer;
    public GameObject visualPilas;

    private void Update()
    {
        DestroyLantern();
    }

    private void DestroyLantern()
    {
        Collider[] collisions = UnityEngine.Physics.OverlapSphere(transform.position, pickUpRadius, collisionLayer);

        if (collisions.Length > 0)
        {
            foreach (Collider coll in collisions)
            {
                var collidedRigidBody = coll.GetComponent<Rigidbody>();
                if (Input.GetKeyDown(KeyCode.F) && collidedRigidBody != null)
                {
                    sonidoLinterna.PlaySound1();
                    linternaPlayer.SetActive(true);
                    visualPilas.SetActive(true);
                    linternaPlayer.GetComponent<Linterna>().linternaEnMano = true;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}

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

    [SerializeField] private GameObject pressF;

    private bool isInRange = false; // Controla si el jugador está en el rango de interacción.

    private void Update()
    {
        CheckLanternProximity();
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            LanternPickUp();
        }

    }

    private void CheckLanternProximity()
    {
        Collider[] collisions = UnityEngine.Physics.OverlapSphere(transform.position, pickUpRadius, collisionLayer);

        if (collisions.Length > 0)
        {
            if (!isInRange)
            {
                isInRange = true; // Marca que el jugador está en el rango.
                pressF.SetActive(true); // Muestra "Press F".
            }
        }
        else if (isInRange)
        {
            isInRange = false; // Marca que el jugador salió del rango.
            pressF.SetActive(false); // Oculta "Press F".
        }
    }

    private void LanternPickUp()
    {
        sonidoLinterna.PlaySound1();
        linternaPlayer.SetActive(true);
        visualPilas.SetActive(true);
        linternaPlayer.GetComponent<Linterna>().linternaEnMano = true;
        pressF.SetActive(false);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}

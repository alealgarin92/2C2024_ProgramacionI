using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBatery : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject linternaPlayer;
    [SerializeField] private SoundController sonidoBateria;

    [SerializeField] private float pickUpRadius;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float carga;
 
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
                    linternaPlayer.GetComponent<Linterna>().cantBateria += carga;
                    sonidoBateria.PlaySound2();
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
}

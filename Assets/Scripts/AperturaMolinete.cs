using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molinete : MonoBehaviour
{
    [SerializeField] private Animator molinetedoor;
    [SerializeField] private float restaSaldo;
    [SerializeField] private AudioClip sonidopuerta;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float initialTemp;

    
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Player"))
        {
            ApoyaSube(other.gameObject);
            Opendoor();
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("CierreM"))
        {
            Closedoor();
        }
    }


    private void ApoyaSube(GameObject target)
    {
        MainCharacter mainCharacter = target.GetComponent<MainCharacter>();
        if (mainCharacter != null)
        {
            if (mainCharacter.saldoSube > 300)  // Asegúrate de que la variable 'saldo' es la correcta
            {
                mainCharacter.saldoSube -= restaSaldo;
                
            }
        }
    }

    public void Opendoor()
    {
        molinetedoor.SetBool("Abrir", true);
    }

    [ContextMenu("Cerrarmolinete")]
    public void Closedoor()
    {
        molinetedoor.SetBool("Abrir", false);
    }

    public void Abresonido()
    {
        audioSource.PlayOneShot(sonidopuerta);
    }
}

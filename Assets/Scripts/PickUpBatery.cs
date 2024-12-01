using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBatery : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject linternaPlayer;
    [SerializeField] private SoundController sonidoBateria;
    [SerializeField] private float carga;
    public void BateryPickUp()
    {
        GameManagerTest.instance.AddCharge(carga);
        sonidoBateria.PlaySound2(); // Reproduce el sonido.
        Destroy(gameObject); // Destruye la linterna.
    } 
}
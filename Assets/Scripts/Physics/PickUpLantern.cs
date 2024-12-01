using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLantern : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject linternaPlayer;
    [SerializeField] private SoundController sonidoLinterna;
    [SerializeField] private GameObject visualPilas;

    public void LanternPickUp()
    {
        sonidoLinterna.PlaySound1();
        linternaPlayer.SetActive(true);
        visualPilas.SetActive(true);
        linternaPlayer.GetComponent<Linterna>().linternaEnMano = true;
        Destroy(gameObject);
    }

}

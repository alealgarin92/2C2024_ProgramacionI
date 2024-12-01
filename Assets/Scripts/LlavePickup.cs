using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlavePickup : MonoBehaviour
{
    [SerializeField] private SoundController sonidoLlave;
    [SerializeField] private MainCharacter player;
   
    public void pickUpKeys()
    {
        sonidoLlave.PlaySound3();
        player.cantLlaves += 1;
        Destroy(gameObject);  
    }
}

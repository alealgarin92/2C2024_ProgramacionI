using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubePickup : MonoBehaviour
{
    [SerializeField] private SoundController sonidoSube;
    [SerializeField] private MainCharacter player;
    
    public void PickUpSube()
    { 
        sonidoSube.PlaySound4();
        player.cantSube += 1;
        Destroy(gameObject);        
    }
}

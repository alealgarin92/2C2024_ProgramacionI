using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private MusicController controller;
    [SerializeField] private int levelMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (levelMusic)
            {
                case 1:
                    controller.PlayLevel1Music();
                    break;
                case 2:
                default:
                    controller.PlayLevel2Music();
                    break;
            }

            Destroy(gameObject);
        }
    }
}

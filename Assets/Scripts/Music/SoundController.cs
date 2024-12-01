using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource mainSound;

    [SerializeField] private AudioClip levantarLinterna, levantarBateria, tomarLlaves, tomarSube;


    public void ChangeSound(AudioClip newClip)
    {
        //Detener audio
        mainSound.Stop();
        //Cambiar la musica
        mainSound.clip = newClip;
        //Reproducir la nueva musica
        mainSound.PlayOneShot(newClip);
    }

    public void PlaySound1()
    {
        ChangeSound(levantarLinterna);
    }

    public void PlaySound2()
    {
        ChangeSound(levantarBateria);
    }

    public void PlaySound3()
    {
        ChangeSound(tomarLlaves);
    }

    public void PlaySound4()
    {
        ChangeSound(tomarSube);
    } 
}

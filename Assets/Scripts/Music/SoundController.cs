using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource mainSound;

    [SerializeField] private AudioClip levantarLinterna, levantarBateria, TomarLLaves;


    public void ChangeSound(AudioClip newClip)
    {
        //Detener audio
        mainSound.Stop();
        //Cambiar la musica
        mainSound.clip = newClip;
        //Reproducir la nueva musica
        mainSound.PlayOneShot(levantarLinterna);
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
        ChangeSound(TomarLLaves);
    }

}

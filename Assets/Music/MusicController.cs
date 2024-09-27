using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudio;

    [SerializeField] private AudioClip level1, level2;

    public void ChangeMusic(AudioClip newClip)
    {
        //Detener audio
        mainAudio.Stop();
        //Cambiar la musica
        mainAudio.clip = newClip;
        //Reproducir la nueva musica
        mainAudio.Play();
    }

    public void PlayLevel1Music()
    {
        ChangeMusic(level1);
    }

    public void PlayLevel2Music()
    {
        ChangeMusic(level2);
    }
    
}

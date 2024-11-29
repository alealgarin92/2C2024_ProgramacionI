using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSound : MonoBehaviour
{
    public AudioSource source;

    public AudioClip footStep;

    void SoftWalk()
    {
        source.clip = footStep;
        source.Play();
    }
}

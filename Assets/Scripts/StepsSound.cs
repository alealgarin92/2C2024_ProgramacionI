using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSound : MonoBehaviour
{
    public AudioSource source;

    public AudioClip footStep;

    void FootWalk()
    {
        source.clip = footStep;
        source.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    [ContextMenu("OpenDoor")] 
    private void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    [ContextMenu("CloseDoor")]
    private void CloseDoor()
    {
        doorAnimator.SetBool("isOpen", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            OpenDoor();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CloseDoor();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedHouse : MonoBehaviour
{
    [SerializeField] private GameObject house;

    private void Update()
    {
        activateHouse();
    }

    private void activateHouse()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            house.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            house.SetActive(false);
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isElastic;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private CinemachineVirtualCamera camera1;
    [SerializeField] private CinemachineVirtualCamera camera2;

    public void Update()
    {
        //ManuallyMoveCamera();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Camara 1
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Camara 2
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
        }
    }
    public void ManuallyMoveCamera()
    {
        Vector3 desireCameraPosition = target.transform.position + offset;

        if (isElastic)
        {
            transform.position = Vector3.Lerp(transform.position, desireCameraPosition, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = desireCameraPosition;
        }
        
    }
}

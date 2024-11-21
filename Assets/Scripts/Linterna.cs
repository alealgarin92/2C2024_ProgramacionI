using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linterna : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    [SerializeField] private Light pointLight;
    public bool linternaEnMano;
    public float cantBateria = 100;
    public float perdidaBateria = 0.5f;

    [Header("Visuales")]
    public Image pila1;
    public Image pila2;
    public Image pila3;
    public Image pila4;
    public Sprite pilaVacia;
    public Sprite pilaCargada;
    public Text porcentaje;


    private bool activelight;


    private void Update()
    {
        cantBateria = Mathf.Clamp(cantBateria, 0, 100);
        int valorBateria = (int)cantBateria;
        porcentaje.text = valorBateria.ToString() + "%";

        if (Input.GetMouseButtonDown(0) && linternaEnMano == true)
        {
            activelight = !activelight;
            if (activelight == true)
            {
                
                spotLight.enabled = true;
                pointLight.enabled = true;
            }

            if (activelight == false)
            {
                
                spotLight.enabled = false;
                pointLight.enabled = false;
            }  
        }

        if (activelight == true && cantBateria > 0) 
        { 
            cantBateria -= perdidaBateria * Time.deltaTime;
        }

        if(cantBateria == 0)
        {
            
            spotLight.intensity = 0;
            pointLight.intensity = 0;
            pila1.sprite = pilaVacia;
        }

        if(cantBateria > 0 && cantBateria <= 25)
        {
            
            spotLight.intensity = 5;
            pila1.sprite = pilaCargada;
            pila2.sprite = pilaVacia;
        }

        if(cantBateria > 25 &&  cantBateria <= 50)
        {
            
            spotLight.intensity = 10;
            pila2.sprite= pilaCargada;
            pila3.sprite = pilaVacia;
        }

        if (cantBateria > 50 && cantBateria <= 75)
        {
            
            spotLight.intensity = 15;
            pila3.sprite = pilaCargada;
            pila4.sprite = pilaVacia;
        }

        if (cantBateria > 75 && cantBateria <= 100)
        {
            
            spotLight.intensity = 20;
            pila4.sprite = pilaCargada;
        }
    }
  
}

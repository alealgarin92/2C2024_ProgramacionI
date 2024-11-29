using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linterna : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    [SerializeField] private Light pointLight;
    public bool linternaEnMano;
    public float perdidaBateria = 0.5f;

    [Header("Visuales")]
    public Image pila;
    public Sprite pila_0;
    public Sprite pila_1;
    public Sprite pila_2;
    public Sprite pila_3;
    public Sprite pila_4;
    public Text porcentaje;


    private bool activelight;

    private void Update()
    {
        var manager = GameManagerTest.instance;

        var cantBateria = manager.GetCurrentBatery();

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
            BateryLoss();
        }

        if (cantBateria == 0)
        {

            spotLight.intensity = 0;
            pointLight.intensity = 0;
            pila.sprite = pila_0;
        }

        if (cantBateria > 0 && cantBateria <= 25)
        {

            spotLight.intensity = 5;
            pila.sprite = pila_1;

        }

        if (cantBateria > 25 && cantBateria <= 50)
        {

            spotLight.intensity = 10;
            pila.sprite = pila_2;
        }

        if (cantBateria > 50 && cantBateria <= 75)
        {

            spotLight.intensity = 15;
            pila.sprite = pila_3;

        }

        if (cantBateria > 75 && cantBateria <= 100)
        {

            spotLight.intensity = 20;
            pila.sprite = pila_4;
        }
    }

    private void BateryLoss()
    {
        GameManagerTest.instance.BateryLoss(perdidaBateria);
    }
  
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTest : MonoBehaviour
{
    public static GameManagerTest instance;
    
    [SerializeField] private float health;
    [SerializeField] private float currentBatery;
   

    //Quiero un evento que me indique cuando el jugador pierde vida

 
    public float GetCurrentBatery()
    {
        return currentBatery;
    }

    public void SetCurrentBatery(float cantidad)
    {
        currentBatery = cantidad;
    }

    public void BateryLoss(float cantidad)
    {
        currentBatery -= cantidad * Time.deltaTime; ;
    }

    public void AddCharge(float cantidad)
    {
        currentBatery += cantidad ;
    }

    private void Awake()
    {
        //Si el game manager ya existe?? Entonces no tengo razon de ser, me destruyo.
        if (instance != null)
        {
            Destroy(gameObject); //Destruye el script, el monobehaviour pero no el gameobject
            return;
        }

        instance = this;

        //Decirle a Unity, que NO quiero que este objeto se destruya al cambiar de escena!!
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentBatery = 100;
    }
}

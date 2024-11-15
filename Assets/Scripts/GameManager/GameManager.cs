using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;
    //public static GameManager GetGameManager
    //{
    //    get { return instance;  }
    //}

    //private static GameManager instance;

    [SerializeField] private int nextLevelPoints = 200;
    private int level;
    private int currentPoints;


    //Quiero un evento para enterarme CUANDO en el juego cambiamos de nivel, y a que nivel cambiamos

    // delegate void Delegate(int sadasdn string dsfads, double sfdasdf);
    // Action<int, string, double> Delegate
    // delegate int LevelDelegate();
    // private Func<int, int, float> LevelDelegate; El ultimo parametro del Func es lo que devuelve
    public event Action<int> OnLevelChanged;


    public int GetCurrentLevel()
    {
        return level;
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        if (currentPoints >= nextLevelPoints)
        {
            level++;
            currentPoints = 0;
            //La firma de un metodo es: que devuelve(void, int float) y que recibe(parametros: enteros, float etc)
            //Levanto el evento de que acabamos de cambiar de nivel
            if (OnLevelChanged != null)
            {
                OnLevelChanged.Invoke(level);
            }

            //Cargo la escena del siguiente nivel
            //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            AsyncOperation operation = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
            // operation.progress //entre 0 y 0.9
            //operation.completed
        }

    }

    public void SetPoints(int points)
    {
        currentPoints = points;
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
}

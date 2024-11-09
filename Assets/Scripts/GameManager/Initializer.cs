using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Initializer : MonoBehaviour
{
    [SerializeField] private int intialPoints;

    public event Action OnInitializeComplete;
    public UnityEvent OnInitializeCompleteUnity;

    private void Awake()
    {
        //Vamos a utilizar awake para inicializacion interna
    }
    private void Start()
    {
        var manager = GameManager.instance;
        manager.SetPoints(intialPoints);
        //Cuando se llama al cambio de nivel, escribimos un mensaje en la consola
        manager.OnLevelChanged += OnLevelChangedHandler;

        //Manera dificil
        if(OnInitializeComplete != null)
        {
            OnInitializeComplete.Invoke();
        }
        //Manera facil
        //OnInitializeComplete?.Invoke();

        //Manera dificil
        if (OnInitializeCompleteUnity != null)
        {
            OnInitializeCompleteUnity.Invoke();
        }
        //Manera facil OnInitializeCompleteUnity?.Invoke();
    }

    [ContextMenu("remove interest")]
    private void RemoveInterest()
    {
        var manager = GameManager.instance;
        manager.OnLevelChanged -= OnLevelChangedHandler;
        //Memory leak
    }
    private void OnLevelChangedHandler(int JHHJ)
    {
        Debug.Log($"El level es {JHHJ}");
    }

}

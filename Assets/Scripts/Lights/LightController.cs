using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class LightProperties
{
    public float intensity;
    public Color color;
    public LightShadows shadowType;
}

public class LightController : MonoBehaviour
{
    [SerializeField] private Light baseLight;
    [SerializeField] private float changeLightTimer;
    [SerializeField] private LightProperties properties;

    private float currentTimer;

    private void Awake()
    {
        currentTimer = changeLightTimer;
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0)
        {
            ChangeLightProperties();
            currentTimer = changeLightTimer;
        }
    }

    private void ChangeLightProperties()
    {
        baseLight.color = properties.color;
        baseLight.intensity = properties.intensity;
        baseLight.shadows = properties.shadowType;
    }
}
 
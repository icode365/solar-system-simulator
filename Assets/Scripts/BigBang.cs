using System;
using Planets;
using UnityEngine;

public class BigBang : MonoBehaviour
{
    public float earthInitVelocity = 10f;
    private void Start()
    {
        CreateSun();
    }

    private void CreateSun()
    {
        var sunObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sun = sunObject.AddComponent<Planet>();
        sun.Init("Sun", 1000f, Vector3.zero, Vector3.zero);
        
        CreateEarth(sun);
    }

    private void CreateEarth(Planet sun)
    {
        var earthObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var earth = earthObject.AddComponent<Planet>();
        earth.Init("Earth", 10, Vector3.forward * earthInitVelocity, Vector3.one * 10, sun);
    }
}

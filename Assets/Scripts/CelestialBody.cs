using System;
using Planets;
using UnityEngine;

[Serializable]
public class CelestialData
{
    public string bodyName;
    public float mass;
    public float radius;
    public Vector3 position;
}

[Serializable]
public class CelestialBody
{
    public CelestialData Data { get; private set; }

    protected CelestialBodyVisuals visualTransform;
    private Material _material;

    public CelestialBody(CelestialData data, Material mat)
    {
        Data = data;
        _material = mat;
        ConfigureBodyVisual();
    }

    protected void AddProximityTrigger(Orbiter orbiter)
    {
        var trigger = visualTransform.gameObject.AddComponent<ProximityTrigger>();
        trigger.SetOrbiter(orbiter);
    }

    private void ConfigureBodyVisual()
    {
        var planetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualTransform = planetObject.AddComponent<CelestialBodyVisuals>();
        visualTransform.name = Data.bodyName;
        visualTransform.transform.localScale = Vector3.one * Data.radius;
        visualTransform.transform.position = Data.position;
        visualTransform.AssignMaterial(_material);
    }

    public Vector3 GetPosition()
    {
        if (Data == null)
        {
            // Debug.LogError("Celestial Body Data is null");
            return Vector3.zero;
        }

        return Data.position;
    }
}
using UnityEngine;

public class CelestialData
{
    public string bodyName;
    public float mass;
    public float radius;
    public Vector3 position;
}

public class CelestialBody
{
    public CelestialData Data { get; private set; }

    protected Transform visualTransform;

    public CelestialBody(CelestialData data)
    {
        Data = data;
        ConfigureBodyVisual();
    }

    private void ConfigureBodyVisual()
    {
        visualTransform = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        visualTransform.name = Data.bodyName;
        visualTransform.localScale = Vector3.one * Data.radius;
        visualTransform.position = Data.position;
    }

    public Vector3 GetPosition() => Data.position;
}
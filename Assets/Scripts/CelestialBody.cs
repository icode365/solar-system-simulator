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

    protected CelestialBodyVisuals visualTransform;

    public CelestialBody(CelestialData data)
    {
        Data = data;
        ConfigureBodyVisual();
    }

    private void ConfigureBodyVisual()
    {
        var planetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualTransform = planetObject.AddComponent<CelestialBodyVisuals>();
        visualTransform.name = Data.bodyName;
        visualTransform.transform.localScale = Vector3.one * Data.radius;
        visualTransform.transform.position = Data.position;
    }

    public Vector3 GetPosition() => Data.position;
}
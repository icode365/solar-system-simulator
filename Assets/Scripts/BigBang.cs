using System;
using System.Collections.Generic;
using Planets;
using Planets.Util;
using UnityEngine;

public class BigBang : MonoBehaviour
{
    public float earthInitVelocity = 10f;
    public float distanceFromSun = 150f;
    public TextAsset planetdataJson;

    private readonly HashSet<string> SolarSystemPlanetIds = new()
    {
        "mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune", "sun"
    };

    private List<PlanetDetails> SolarSystem = new();
    private Planet sun;

    private void Start()
    {
        GetPlanetData();
        CreateSolarSystem();
    }

    private void CreateSolarSystem()
    {
        // TODO : CREATE ANOTHER FLOW TO CREATE AND ASSIGN THE SUN
        foreach (var planet in SolarSystem)
        {
            CreatePlanet(planet);
        }
    }

    private void GetPlanetData()
    {
        var planetData = PlanetDataParser.Parse(planetdataJson.text);
        Debug.Log(planetData.bodies.Length);

        foreach (var planet in planetData.bodies)
        {
            if (SolarSystemPlanetIds.Contains(planet.englishName.ToLower()))
            {
                Debug.Log("Planet : " + planet.englishName + " | Mass: " + planet.mass.massValue);
                var planetDetails = new PlanetDetails()
                {
                    orbiterName = planet.englishName,
                    mass = planet.mass.massValue,
                    initialPosition = Vector3.zero,
                    initialVelocity = Vector3.zero,
                    primary = sun
                };
                
                SolarSystem.Add(planetDetails);
            }
        }
    }

    private void CreatePlanet(PlanetDetails details)
    {
        var planetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var planet = planetObject.AddComponent<Planet>();
        planet.Init(details);
    }
}
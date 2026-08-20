using System.Collections.Generic;
using Planets;
using Planets.Util;
using UnityEngine;

/// <summary>
/// Boostrapper Class
/// </summary>
public class BigBang : MonoBehaviour
{
    public float earthInitVelocity = 10f;
    public float distanceFromSun = 150f;

    // TODO Move to SolarSystem Manager
    public float SimulationScale = 10000f;
    [Range(0.1f, 1f)] public float SimulationTime = 0.25f;

    public TextAsset planetdataJson;

    private readonly HashSet<string> SolarSystemPlanetIds = new()
    {
        "mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune"
    };

    private List<Orbiter> activePlanets = new();

    // ✅ 1. Add Sun ID 
    private Sun _sun;

    private void Start()
    {
        var solarSystemData = GetSolarSystemData();

        CreateSun(solarSystemData);
        //✅  4. Send that data to CreateSolarSystemFrom(PlanetData)
        CreateSolarSystemFrom(solarSystemData);
        CreateTime();
    }

    public void CreateTime()
    {
        var solarSystemManager = new GameObject();
        var time = solarSystemManager.AddComponent<SolarSystemManager>();
        time.FixedFrameUpdated += UpdatePlanetPhysics; //TODO All planets update visuals
    }

    private void UpdatePlanetPhysics() => activePlanets.ForEach(v => v.PhysicsUpdate());

    private void CreateSolarSystemFrom(Bodies solarSystemData)
    {
        foreach (var planet in solarSystemData.bodies)
        {
            if (SolarSystemPlanetIds.Contains(planet.englishName.ToLower()))
            {
                var planetDetails = new CelestialData()
                {
                    bodyName = planet.englishName,
                    mass = planet.mass.massValue / SimulationScale,
                    position = _sun.GetPosition() + new Vector3(planet.perihelion / SimulationScale, 0f, 0f),
                    radius = planet.meanRadius / SimulationScale
                };

                var perihelion = planet.perihelion / SimulationScale;
                CreatePlanet(planetDetails, perihelion, _sun);
            }
        }
    }

    private Bodies GetSolarSystemData() =>
        // ✅ 1. Only return Planets data
        PlanetDataParser.Parse(planetdataJson.text);

    private void CreatePlanet(CelestialData details, double perihelion, CelestialBody primary)
    {
        var planet = new Orbiter(details, perihelion, primary);
        activePlanets.Add(planet);
    }

    //✅  2. Use "Sun" Class instead of Planet
    private void CreateSun(Bodies solarSystemData)
    {
        // TODO : add sun's mass and basic details for gravity calculation
        foreach (var planet in solarSystemData.bodies)
        {
            if (planet.englishName.ToLower() == "sun")
            {
                var planetDetails = new CelestialData()
                {
                    bodyName = planet.englishName,
                    mass = planet.mass.massValue / SimulationScale,
                    position = Vector3.zero
                };

                _sun = new Sun(planetDetails);
            }
        }
    }
}
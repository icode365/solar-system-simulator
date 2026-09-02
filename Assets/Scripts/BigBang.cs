using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planets;
using Planets.Util;
using UnityEngine;

/// <summary>
/// Boostrapper Class
/// </summary>
public class BigBang : MonoBehaviour
{
    // TODO Move to SolarSystem Manager
    public float SimulationDistanceScale = 10000f;
    public float SimulationScale = 10000f;
    public TextAsset planetdataJson;

    private readonly HashSet<string> SolarSystemPlanetIds = new()
    {
        "mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune"
    };

    private List<Orbiter> activePlanets = new();

    private const string planetTexturesPath = "Planets/Textures";
    private HashSet<Texture2D> planetTextures;

    // ✅ 1. Add Sun ID 
    private Sun _sun;
    private MaterialBuilder _materialBuilder;

    private void Start()
    {
        var solarSystemData = GetSolarSystemData();
        _materialBuilder = new();
        planetTextures =
            _materialBuilder.GetTextures(planetTexturesPath).ToHashSet();
        
        CreateSun(solarSystemData);
        CreateSolarSystemFrom(solarSystemData);
        CreateTime();
    }

    private void CreateTime()
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
                    mass = planet.mass.massValue * SimulationDistanceScale,
                    position = _sun.GetPosition() + new Vector3(planet.perihelion * SimulationDistanceScale, 0f, 0f),
                    radius = planet.meanRadius * SimulationScale
                };

                OrbitData orbitData = new()
                {
                    eccentricity = planet.eccentricity,
                    semimajorAxis = planet.semimajorAxis * SimulationDistanceScale,
                    sideralOrbit = planet.sideralOrbit,
                    primary = _sun
                };

                var tex = GetTexFor(planet.englishName.ToLower());
                var material = _materialBuilder.ApplyMaps(tex);
                CreatePlanet(planetDetails, orbitData, material);
            }
        }
    }

    private Bodies GetSolarSystemData() =>
        // ✅ 1. Only return Planets data
        PlanetDataParser.Parse(planetdataJson.text);

    private void CreatePlanet(CelestialData details, OrbitData orbitData, Material material)
    {
        var planet = new Orbiter(details, orbitData, material);
        activePlanets.Add(planet);
    }

    // WE HAVE TO DO THIS IN 2 Passes, since Sun's reference
    // needs to be assigned before the planet creation starts
    private void CreateSun(Bodies solarSystemData)
    {
        foreach (var planet in solarSystemData.bodies)
        {
            if (planet.englishName.ToLower() == "sun")
            {
                var planetDetails = new CelestialData()
                {
                    bodyName = planet.englishName,
                    mass = planet.mass.massValue * SimulationDistanceScale,
                    radius = planet.meanRadius * SimulationScale,
                    position = Vector3.zero
                };

                var tex = GetTexFor(planet.englishName.ToLower());
                tex.emissionEnabled = true;
                tex.emission = 100f;
                var material = _materialBuilder.ApplyMaps(tex);
                _sun = new Sun(planetDetails, material);
                break;
            }
        }
    }
    
    // Material
    private MaterialData GetTexFor(string planetName)
    {
        MaterialData matData = new();
        List<Texture2D> planetRelatedTex = new();
        
        foreach (var planetTex in planetTextures)
        {
            if (planetTex.name.Contains(planetName))
            {
                planetRelatedTex.Add( planetTex);
                Debug.Log("Found " + planetName + " texture");
            }
        }
        
        Texture2D baseTex = planetTextures.FirstOrDefault(t => 
            t != null && 
            t.name.Contains(planetName, System.StringComparison.OrdinalIgnoreCase));
        
        Texture2D normalTex = planetTextures.FirstOrDefault(t => 
            t != null && 
            t.name.Contains(planetName, System.StringComparison.OrdinalIgnoreCase) && 
            t.name.Contains("normal", System.StringComparison.OrdinalIgnoreCase));

        Texture2D specularTex = planetTextures.FirstOrDefault(t => 
            t != null && 
            t.name.Contains(planetName, System.StringComparison.OrdinalIgnoreCase) && 
            t.name.Contains("specular", System.StringComparison.OrdinalIgnoreCase));

        matData.baseTex = baseTex;
        matData.normalTex = normalTex;
        matData.specularTex = specularTex;
        
        return matData;
    }
}
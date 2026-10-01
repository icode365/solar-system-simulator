using System.Collections.Generic;
using Planets;
using UnityEngine;

// TODO : Change the name that suits this better like TargetPlanetResolver
public class PlanetLocatorService
{
    private Transform _ship;
    private List<Orbiter> _activePlanets;
    private Orbiter nearestPlanet;

    public void SetPlanetList(List<Orbiter> activePlanets, Transform ship)
    {
        _activePlanets = activePlanets;
        _ship = ship;
    }

    private float interDistance;

    public Orbiter GetTargetPlanet()
    {
        return GetPlanetInViewCone() ?? GetNearestPlanet();
    }

    private Orbiter planetInShipSight;
    // private Dictionary<Orbiter, float> orbiterDotDic = new();
    
    private Orbiter GetPlanetInViewCone()
    {
        planetInShipSight = null;
        var highestValue = Mathf.NegativeInfinity;

        // why is the planet not updating to the nearestplanet after the planet goes out of bounds
        foreach (var planet in _activePlanets)
        {
            var offset = planet.GetPosition() - _ship.position;
            var dotProd = Vector3.Dot(_ship.forward, offset.normalized);

            // orbiterDotDic.TryAdd(planet, dotProd);
            
            if (!(dotProd > highestValue) && dotProd < 0.9f)
                continue;
            
            highestValue = dotProd;
            planetInShipSight = planet;
        }

        if (highestValue < 0.9f)
            planetInShipSight = null;

        // Debug.Log(string.Join('\n', orbiterDotDic.Values));
        
        return planetInShipSight;
    }

    public float GetDotProd()
    {
        if (planetInShipSight == null) return -2;
        
        var offset = planetInShipSight.GetPosition() - _ship.position;
        var dotProd = Vector3.Dot(_ship.forward, offset.normalized);
        return dotProd;
    }

    public Orbiter GetNearestPlanet()
    {
        float shortestSqrDistance = float.MaxValue;

        if (_activePlanets is { Count: 0 }) return null;

        foreach (var planet in _activePlanets)
        {
            if (planet == null) continue; // Safety check

            // 2. Get the squared distance
            float interDistance = GetSqrDistance(planet.GetPosition(), _ship.position);

            if (interDistance < shortestSqrDistance)
            {
                shortestSqrDistance = interDistance;
                nearestPlanet = planet;
            }
        }

        return nearestPlanet;
    }


    public float GetDistanceFromTarget(Orbiter targetOrbiter)
    {
        if (targetOrbiter == null) return 0;

        return Vector3.Distance(_ship.position, targetOrbiter.GetPosition());
    }

    private float GetSqrDistance(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(b - a);
    }
}
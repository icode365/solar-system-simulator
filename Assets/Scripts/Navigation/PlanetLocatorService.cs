using System.Collections.Generic;
using Planets;
using UnityEngine;

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
    

    public float GetDistanceFromNearestPlanet()
    {
        if (nearestPlanet == null) return 0;

        return Vector3.Distance(_ship.position, nearestPlanet.GetPosition());
    }

    private float GetSqrDistance(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(b - a);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planets.Util
{
    [Serializable]
    public class Bodies
    {
        public PlanetData[] bodies;
    }

    [Serializable]
    public class PlanetData
    {
        public string id;
        public string englishName;
        public Moons[] moons;
        public Mass mass;
        public float meanRadius;
        public float perihelion;
        public float eccentricity;
        public int semimajorAxis;
        public float sideralOrbit;
    }

    [Serializable]
    public class Mass
    {
        public float massValue;
        public int massExponent;
    }

    [Serializable]
    public class Moons
    {
        public string moon;
    }

    public static class PlanetDataParser
    {
        public static Bodies Parse(string json)
        {
            return JsonUtility.FromJson<Bodies>(json);
        }
    }
}
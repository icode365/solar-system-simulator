using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class PlanetDetails
    {
        public string orbiterName;
        public float mass;
        public Vector3 initialVelocity;
        public Vector3 initialPosition;
        public Planet primary;
    }

    public class Planet : OrbiterBase
    {
        public List<Moon> moons;

        public Planet(PlanetDetails details)
        {
            base.Init(details);
        }

        public override void Init(PlanetDetails details)
        {
            base.Init(details);

            // TODO : Add Moon logic later
            // if (primary != null)
            //     CreateMoon("Moon", 10, initialVelocity * 0.00005f, transform.position + Vector3.right);
        }

        private void CreateMoon(PlanetDetails details)
        {
            var moonObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            moonObject.transform.localScale = Vector3.one * 0.25f;
            var moon = moonObject.AddComponent<Moon>();
            moon.Init(details);
        }
    }
}
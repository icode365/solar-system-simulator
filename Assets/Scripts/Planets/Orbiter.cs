using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class OrbitData
    {
        public float eccentricity;
        public float semimajorAxis;
        public float sideralOrbit;
        // Composition via Object Association instead of Inheritence
        public CelestialBody primary;
        public double perihelion;
    }

    public class Orbiter : CelestialBody
    {
        protected const float GravitationalConstant = 1f;
        private Vector3 velocity;

        private OrbitData _orbitData;
        
        public Orbiter(CelestialData data, OrbitData orbiterData, Material material)
            : base(data, material)
        {
            _orbitData = orbiterData;
        }


        private float _timer = 0f;

        public void PhysicsUpdate()
        {
            if (_orbitData.primary == null) return;

            // 1. Advance time and calculate Mean Anomaly (M)
            _timer += Time.deltaTime;
            float meanAnomaly = (2f * Mathf.PI / _orbitData.sideralOrbit) * _timer;

            // 2. Solve Kepler's Equation for Eccentric Anomaly (E) using Newton's method
            float eccentricAnomaly = SolveKepler(meanAnomaly, _orbitData.eccentricity);

            // 3. Calculate 2D position in the orbital plane
            // The Sun sits at one of the focal points, which is shifted by (a * e)
            float x = _orbitData.semimajorAxis * (Mathf.Cos(eccentricAnomaly) - _orbitData.eccentricity);
            float z = _orbitData.semimajorAxis * Mathf.Sqrt(1f - _orbitData.eccentricity * _orbitData.eccentricity) * Mathf.Sin(eccentricAnomaly);

                // TODO : Add Function to set the position
            visualTransform.transform.position = _orbitData.primary.GetPosition() + new Vector3(x, 0f, z);

            Data.position = visualTransform.transform.position;
        }

        // Iterative solver for Kepler's Equation: M = E - e*sin(E)
        private float SolveKepler(float M, float e)
        {
            float E = M; // Initial guess
            for (int i = 0; i < 5; i++) // 5 iterations is highly accurate for planetary orbits
            {
                E = E - (E - e * Mathf.Sin(E) - M) / (1f - e * Mathf.Cos(E));
            }

            return E;
        }
    }
}
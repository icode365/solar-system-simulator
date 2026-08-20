using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class Orbiter : CelestialBody
    {
        protected const float GravitationalConstant = 1f;
        protected Vector3 _accelaration = Vector3.zero;
        private Vector3 velocity;
        private double perihelion;

        // Composition via Object Association instead of Inheritence
        private CelestialBody primary;


        public Orbiter(CelestialData data, double perihelion, CelestialBody primary)
            : base(data)
        {
            this.perihelion = perihelion;
        }

        //Debug

        [Header("Live Trail Settings")] public bool showLiveTrail = true;
        public int maxHistoryPoints = 200; // How many past positions to remember

        private readonly Queue<Vector3> _positionHistory = new();



        public void PhysicsUpdate()
        {
            if (primary == null) return;

            _accelaration = GetAcceleration();
            velocity += _accelaration * Time.fixedDeltaTime;
            visualTransform.position += velocity * Time.fixedDeltaTime;

            _positionHistory.Enqueue(visualTransform.position);

            // Remove older points so the history doesn't grow forever
            if (_positionHistory.Count > maxHistoryPoints)
            {
                _positionHistory.Dequeue();
            }

            Data.position = visualTransform.position;
            
            OnDrawGizmos();
        }

        private Vector3 GetAcceleration()
        {
            if (GetDirectionToSun.sqrMagnitude < 0.01f) return Vector3.zero;

            // TODO : REMOVE _DETAILS
            var totalAcceleration =
                GravitationalConstant * primary.Data.mass / GetDirectionToSun.sqrMagnitude;

            return GetDirectionToSun.normalized * totalAcceleration;
        }

        private Vector3 GetDirectionToSun => primary.GetPosition() - visualTransform.position;

        private void OnDrawGizmos()
        {
            if (showLiveTrail && _positionHistory.Count > 1)
            {
                Gizmos.color = Color.yellow;

                Vector3[] pointsArray = _positionHistory.ToArray();
                for (int i = 0; i < pointsArray.Length - 1; i++)
                {
                    // Draw a continuous line connecting all past physical locations
                    Gizmos.DrawLine(pointsArray[i], pointsArray[i + 1]);
                }
            }
        }
    }
}
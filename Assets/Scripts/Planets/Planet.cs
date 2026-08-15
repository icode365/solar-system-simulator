using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class Planet : MonoBehaviour
    {
        private const float GravitationalConstant = 1f;
        [SerializeField] private string _planetName;
        [SerializeField] private float _mass;

        private Vector3 _velocity;
        private Vector3 _accelaration = Vector3.zero;

        private Planet _sun;

        //Debug

        [Header("Predictive Orbit Settings")] public bool showPredictiveOrbit = true;
        public int circleSegments = 50;

        [Header("Live Trail Settings")] public bool showLiveTrail = true;
        public int maxHistoryPoints = 200; // How many past positions to remember

        private Queue<Vector3> positionHistory = new();

        public void Init(string name, float mass, Vector3 initialVelocity, Vector3 initialPosition, Planet sun = null)
        {
            _planetName = name;
            _mass = mass;
            _velocity = initialVelocity;
            _sun = sun;

            transform.position = initialPosition;
        }

        private void FixedUpdate()
        {
            if (!_sun) return;

            _accelaration = GetAcceleration();
            _velocity += _accelaration * Time.fixedDeltaTime;
            transform.position += _velocity * Time.fixedDeltaTime;
            
            positionHistory.Enqueue(transform.position);
            
            // Remove older points so the history doesn't grow forever
            if (positionHistory.Count > maxHistoryPoints)
            {
                positionHistory.Dequeue();
            }
        }

        private Vector3 GetAcceleration()
        {
            if (GetDirectionToSun.sqrMagnitude < 0.01f) return Vector3.zero;

            var totalAccelaration =
                GravitationalConstant * _sun._mass / GetDirectionToSun.sqrMagnitude;

            return GetDirectionToSun.normalized * (float)totalAccelaration;
        }

        private Vector3 GetDirectionToSun => _sun.transform.position - transform.position;

        private void OnDrawGizmos()
        {
            // if (_sun != null)
            // {
            //     Gizmos.color = Color.cyan; // Set the color of the orbit line
            //
            //     float radius = Vector3.Distance(_sun.transform.position, transform.position);
            //     Vector3 lastPoint = Vector3.zero;
            //
            //     for (int i = 0; i <= circleSegments; i++)
            //     {
            //         float angle = ((float)i / circleSegments) * 2 * Mathf.PI;
            //         float x = Mathf.Sin(angle) * radius;
            //         float z = Mathf.Cos(angle) * radius;
            //     
            //         Vector3 currentPoint = new Vector3(x, 0f, z) + sunTransform.position;
            //
            //         // Draw a line from the last calculated slice to the current one
            //         if (i > 0)
            //         {
            //             Gizmos.DrawLine(lastPoint, currentPoint);
            //         }
            //     
            //         lastPoint = currentPoint;
            //     }
            // }

            // --- 2. DRAW LIVE HISTORICAL TRAIL ---
            if (showLiveTrail && positionHistory.Count > 1)
            {
                Gizmos.color = Color.yellow;

                Vector3[] pointsArray = positionHistory.ToArray();
                for (int i = 0; i < pointsArray.Length - 1; i++)
                {
                    // Draw a continuous line connecting all past physical locations
                    Gizmos.DrawLine(pointsArray[i], pointsArray[i + 1]);
                }
            }
        }
    }
}
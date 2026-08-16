using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public abstract class OrbiterBase : MonoBehaviour
    {
        protected const float GravitationalConstant = 1f;
        [SerializeField] protected string _orbiterName;
        [SerializeField] protected float _mass;

        protected Vector3 _velocity;
        protected Vector3 _accelaration = Vector3.zero;
        protected Planet _primary;

        public virtual void Init(PlanetDetails details)
        {
            _orbiterName = details.orbiterName;
            _mass = details.mass;
            _velocity = details.initialVelocity;
            _primary = details.primary;

            transform.position = details.initialPosition;
            gameObject.name = _orbiterName;
        }

        //Debug

        [Header("Live Trail Settings")] public bool showLiveTrail = true;
        public int maxHistoryPoints = 200; // How many past positions to remember

        private readonly Queue<Vector3> _positionHistory = new();

        private void FixedUpdate()
        {
            if (!_primary) return;

            _accelaration = GetAcceleration();
            _velocity += _accelaration * Time.fixedDeltaTime;
            transform.position += _velocity * Time.fixedDeltaTime;

            _positionHistory.Enqueue(transform.position);

            // Remove older points so the history doesn't grow forever
            if (_positionHistory.Count > maxHistoryPoints)
            {
                _positionHistory.Dequeue();
            }
        }

        private Vector3 GetAcceleration()
        {
            if (GetDirectionToSun.sqrMagnitude < 0.01f) return Vector3.zero;

            var totalAcceleration =
                GravitationalConstant * _primary._mass / GetDirectionToSun.sqrMagnitude;

            return GetDirectionToSun.normalized * totalAcceleration;
        }

        private Vector3 GetDirectionToSun => _primary.transform.position - transform.position;

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
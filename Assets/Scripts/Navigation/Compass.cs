using System;
using SpaceShip;
using UnityEngine;

namespace Navigation
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private SpaceShipController shipController;
        private SpaceShipState _shipState;

        public bool vertical = true;

        [SerializeField] private RectTransform segmentContainer;
        public float mult = 2;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _shipState = shipController._shipState;
        }

        private void LateUpdate()
        {
            if (vertical)
            {
                Vector3 forward = shipController.transform.forward;

                float pitchDeg = -Mathf.Asin(Mathf.Clamp(forward.y, -1f, 1f)) * Mathf.Rad2Deg;

                segmentContainer.localPosition = new Vector3(0f, pitchDeg * 10, 0f);
            }
            else

            {
                Vector3 flatForward = shipController.transform.forward;
                flatForward.y = 0f;
                flatForward.Normalize();
                float yawDeg = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
                segmentContainer.localPosition = new Vector3(yawDeg * 10, 0f, 0f);
            }
        }
    }
}
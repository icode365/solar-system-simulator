using UnityEngine;

namespace SpaceShip
{
    public class ShipVisual : MonoBehaviour
    {
        public SpaceShipController controller;
        private Vector3 _velocity;
        public float _dampingValue;

        // Update is called once per frame
        void Update()
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            var state = controller._shipState;
            
            transform.position = Vector3.SmoothDamp(
                transform.position, state.Position, ref _velocity, _dampingValue);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation, state.Rotation, _dampingValue);
        }
    }
}
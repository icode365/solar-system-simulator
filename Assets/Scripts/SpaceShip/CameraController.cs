using UnityEngine;

namespace SpaceShip
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private SpaceShipController controller;

        public void LateUpdate()
        {
            UpdateCamera(controller._shipState, controller.lookInput);
        }

        private void UpdateCamera(SpaceShipState shipState, Vector2 orbitPosition)
        {
            // transform.rotation = Quaternion.LookRotation(-shipState.Position, transform.up);
            // transform.position *= orbitPosition.normalized;
        }
    }
}
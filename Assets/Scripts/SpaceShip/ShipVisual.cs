using UnityEngine;

namespace SpaceShip
{
    public class ShipVisual : MonoBehaviour
    {
        public SpaceShipController controller;
        private Vector3 _velocity;
        public float _dampingValue;

        public GameObject boosterViz;

        public AnimationCurve boostVizCurve;
        // Update is called once per frame
        void Update()
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (controller == null || controller._shipState == null) return;
            
            var state = controller._shipState;

            transform.position = Vector3.SmoothDamp(
                transform.position, state.Position, ref _velocity, _dampingValue);
            var shipRotation = Quaternion.Slerp(
                transform.rotation, state.Rotation * Quaternion.Euler(0f, 0f, state.CurrentVisualRoll), _dampingValue);
            transform.rotation = shipRotation;
            
            UpdateBooster(state.IsBoosted);
        }

        private void UpdateBooster(bool isBooted)
        {
            boosterViz.SetActive(isBooted);
            // boosterViz.transform.localScale = new Vector3(boostVizCurve.Evaluate(boostValue), 1, 1);
        }
    }
}
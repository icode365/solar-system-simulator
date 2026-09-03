using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShip
{
    public class SpaceShipState
    {

        // spaceShip State for 
        // Position
        // speed
        // forwardDirection
        // Movement Data
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        // Configuration Constants
        public float ConstantForwardSpeed { get; } = 5f;
        public float TurnSpeed { get; } = 10f;
        public float LeanAmount { get; } = 25f; // For visual tilting
        public float SpeedBootValue { get; } = 2f;
        public float CurrentVisualRoll;

        public SpaceShipState()
        {
            Position = new Vector3(0, 0, -800f);
            Rotation = Quaternion.identity;
        }
    }

    public class ShipInput
    {
        public Vector2 XYInput;
        public bool boostInput;
    }

    public class SpaceShipController : MonoBehaviour, SpaceShipInput_Actions.IPlayerActions
    {
        public SpaceShipInput_Actions spaceShipInput;
        private SpaceShipInput_Actions.PlayerActions _playerActions;
        public SpaceShipState _shipState { get; private set; }
        private ShipInput _pendingInput;
        public Vector2 lookInput { get; private set; }

        private void Awake()
        {
            Init();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Init()
        {
            _shipState = new SpaceShipState();
            _pendingInput = new ShipInput();
            spaceShipInput = new SpaceShipInput_Actions();
            _playerActions = spaceShipInput.Player;
            _playerActions.AddCallbacks(this);
        }

        private void OnEnable()
        {
            spaceShipInput.Enable();
        }

        private void OnDisable()
        {
            spaceShipInput.Disable();
        }

        private void OnDestroy()
        {
            spaceShipInput.Dispose();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _pendingInput.XYInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _pendingInput.boostInput = context.performed;
        }

        private void FixedUpdate()
        {
            //Update the vehicle based on passed state
            UpdateShipState(_pendingInput);
            //Update the camera position based on the passed mouse state (or create another camera script)
        }

        private void LateUpdate()
        {
            //update the shipState
        }

        private void UpdateShipState(ShipInput input)
        {
            var speedBoostMultiplier = input.boostInput ? _shipState.SpeedBootValue : 1;
            var x = input.XYInput.x * _shipState.TurnSpeed * Time.deltaTime;
            var y = input.XYInput.y * _shipState.TurnSpeed * Time.deltaTime;

            // Only yaw/pitch accumulate into the real flight rotation
            _shipState.Rotation *= Quaternion.Euler(y, x, 0);

            var forward = _shipState.Rotation * Vector3.forward;
            Vector3 velocity = forward * (_shipState.ConstantForwardSpeed * speedBoostMultiplier);
            _shipState.Position += velocity * Time.deltaTime;

            // Visual-only roll lean, recomputed every frame (not accumulated)
            float targetRollLean = -input.XYInput.x * _shipState.LeanAmount;
            _shipState.CurrentVisualRoll = Mathf.Lerp(
                _shipState.CurrentVisualRoll, targetRollLean, 7 * Time.deltaTime);
        }

        private void OnGUI()
        {
            Rect pos = new Rect(10, 10, 500, 500);
            GUI.Label(pos, $"{_shipState.Position} \n {_shipState.Rotation}");
        }
    }
}
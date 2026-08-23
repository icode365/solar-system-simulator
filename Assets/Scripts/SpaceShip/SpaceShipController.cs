using System;
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
        public Vector3 position;
        public Vector3 forwardDirection;
        public float speed;
    }

    public class ShipInput
    {
        public Vector2 XYInput;
        public Vector2 lookInput;
        public bool boostInput;
    }

    public class SpaceShipController : MonoBehaviour, SpaceShipInput_Actions.IPlayerActions
    {
        public SpaceShipInput_Actions spaceShipInput;
        private SpaceShipInput_Actions.PlayerActions _playerActions;
        private SpaceShipState _shipState;
        private ShipInput _pendingInput;

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
            Debug.Log(" On Move : " + context.ReadValue<Vector2>());
            _pendingInput.XYInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Debug.Log(" OnLook : " + context.ReadValue<Vector2>());
            _pendingInput.lookInput = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            Debug.Log(" OnAttack : " + context.performed);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Debug.Log(" OnInteract : " + context.performed);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Debug.Log(" OnSprint : " + context.performed);
            _pendingInput.boostInput = context.performed;
        }

        private void Update()
        {
            //Update the vehicle based on passed state
            UpdateShipMovement(_pendingInput);
            //Update the camera position based on passed mouse state (or create another camera script)
        }

        private void LateUpdate()
        {
            //update the shipState
            _shipState.position = transform.position;
            _shipState.forwardDirection = transform.forward;
        }

        private void UpdateShipMovement(ShipInput input)
        {
            var speedBoostMultiplier = input.boostInput ? 2 : 1;
            var x = input.XYInput.x * speedBoostMultiplier;
            var y = input.XYInput.y * speedBoostMultiplier;
            
            transform.Translate(x * Time.deltaTime, 0, y * Time.deltaTime);
            transform.Rotate(0, input.lookInput.x * Time.deltaTime, 0);
        }
    }
}

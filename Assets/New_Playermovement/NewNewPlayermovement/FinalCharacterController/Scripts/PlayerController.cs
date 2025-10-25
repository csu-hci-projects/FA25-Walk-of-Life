using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace WalkOfLife.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        #region class variables
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;

        [Header("Camera Settings")]
        public float lookSensH = 2.0f;
        public float lookSensV = 2.0f;
        public float lookLimitV = 89f;
        [Header("Movement speeds")]
        public float runAcceleration = 30f;
        public float runSpeed = 4f;

        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;

        public float drag = 20f;
        public float movingThreasHold = 0.01f;
        public float gravity = 25f;
        public float jumpSpeed = 1.0f;

        // Private variables
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private float _verticalVelocity = 0f;
        #endregion

        #region Startup

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();

        }
        #endregion
        #region UpdateLogic
        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();

        }
        private void UpdateMovementState()
        {
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero; //order matters you need walk so you can run
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;
            bool isGrounded = IsGrounded();
            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                            isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);

            // Airborn state logic
            if (!isGrounded && _characterController.velocity.y >= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
            }
            else if (!isGrounded && _characterController.velocity.y < 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
            }


        }
        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.IsGroundedState();
            if (isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = 0;
            }
            _verticalVelocity -= gravity * Time.deltaTime;
            
            if(_playerLocomotionInput.jumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
            }
        }

        private void HandleLateralMovement()
        {
            //Create quick references for current state
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.IsGroundedState();
            //state dependent acceleration and speed
            float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
            float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;


            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;
            

            // add drag to the player
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
            newVelocity.y += _verticalVelocity;
            // move character (suggested to only call this once per frame)
            _characterController.Move(newVelocity * Time.deltaTime);
        }
        #endregion
        #region Late Update Logic
        private void LateUpdate()
        {
            _cameraRotation.x += lookSensH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSensH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
        #endregion
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreasHold;
        }
        private bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

    }
    
}

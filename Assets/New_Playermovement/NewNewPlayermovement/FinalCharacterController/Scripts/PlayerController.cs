using System;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        public float RotationMissmatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;
        [Header("Camera Settings")]
        public float lookSensH = 2.0f;
        public float lookSensV = 2.0f;
        public float lookLimitV = 89f;
        [Header("Environment Details")]
        [SerializeField] private LayerMask _groundLayers;

        [Header("Movement speeds")]
        public float WalkAcceleration = 10f;
        public float walkSpeed = 2f;
        public float runAcceleration = 30f;
        public float runSpeed = 4f;
        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;
        public float rotateToTargetTime = 0.25f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;

        public float inAirAcceleration = 0.15f;
        public float drag = 20f;
        public float movingThreasHold = 0.01f;
        public float gravity = 25f;
        public float terminalVelocity = 20f;
        public float jumpSpeed = 1.0f;


        // Private variables
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private bool _jumpedLastFrame = false;
        private bool _isRotatingClockwise = false;
        private float _rotatingToTargetTimer = 0f;
        private float _verticalVelocity = 0f;
        private float _antiBump;
        private float _stepOffset;
        private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;
        #endregion

        #region Startup

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _antiBump = sprintSpeed;
            _stepOffset =_characterController.stepOffset;

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
        {   ///  ORDER MATTERS HERE  \\\
            _lastMovementState = _playerState.CurrentPlayerMovementState;

            bool canRun = CanRun();
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;        //order matters you need walk so you can run
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;         // order
            bool isWalking = isMovingLaterally && (!canRun || _playerLocomotionInput.WalkToggledOn); //matters Never forget
            bool isGrounded = IsGrounded();
            ///^^^^ORDER MATTERS HERE^^^^\\

            PlayerMovementState lateralState =  
                                                isSprinting ? PlayerMovementState.Sprinting :
                                                isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);

            // Airborn state logic
            if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y >= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y < 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }


        }
        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.IsGroundedState();

            _verticalVelocity -= gravity * Time.deltaTime;

            if (isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -_antiBump;
            }

            // where the jump happens
            if (_playerLocomotionInput.jumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
                _jumpedLastFrame = true;
            }

            //print(_lastMovementState);
            if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
            {
                _verticalVelocity += _antiBump;
                print("antibump added");

            }
             // if the players absolute velocity is greater than vertical velocity then the players velocity is set to terminal
            if (Mathf.Abs(_verticalVelocity) > Mathf.Abs(terminalVelocity))       
            {
                _verticalVelocity = -1f * Mathf.Abs(terminalVelocity);
            }
        }

        private void HandleLateralMovement()
        {
            //Create quick references for current state
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.IsGroundedState();
            bool isWalking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Walking;

            //state dependent acceleration and speed (ternary statements)
            float lateralAcceleration = !isGrounded ? inAirAcceleration :isSprinting ? sprintAcceleration : runAcceleration;
            float clampLateralMagnitude = !isGrounded ? sprintSpeed : isSprinting ? sprintSpeed : runSpeed;


            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;
            

            // add drag to the player
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x,0f,newVelocity.z), clampLateralMagnitude);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;
            // move character (suggested to only call this once per frame)
            _characterController.Move(newVelocity * Time.deltaTime);
        }
        #endregion
        #region Late Update Logic
        private void LateUpdate()
        {
            UpdateCameraRotation();
        }
        private void UpdateCameraRotation()
        {
            _cameraRotation.x += lookSensH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSensH * _playerLocomotionInput.LookInput.x;

            // if rotation mismatch is not within tolerance, or roate to target is active, ROTATE
            float rotationTolerance = 90f;
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            // Also rotate if we're not idling
            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            else if (MathF.Abs(RotationMissmatch) > rotationTolerance || IsRotatingToTarget)
            {
                updateIdleRotation(rotationTolerance);
            }


            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

            //Get angle between camera and player
            Vector3 camForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
            float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMissmatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);


        }
        
        private void updateIdleRotation(float rotationTolerance)
        {
            // Initiate a new rotation direction
            if (Mathf.Abs(RotationMissmatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockwise = RotationMissmatch > rotationTolerance;
            }
            _rotatingToTargetTimer -= Time.deltaTime;

            
            //rotate player
            if(_isRotatingClockwise && RotationMissmatch > 0f || !_isRotatingClockwise && RotationMissmatch < 0f)
            {
                RotatePlayerToTarget();    
            }
        }
        private void RotatePlayerToTarget()
        {
            Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
        }
        #endregion
        
        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f)
            {
                velocity = Vector3.ProjectOnPlane(velocity, normal);
            }
            return velocity;
        }
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreasHold;
        }
        private bool IsGrounded()
        {
            bool grounded = _playerState.IsGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();

            return _characterController.isGrounded;
        }
        private bool IsGroundedWhileGrounded()
        {
            // the ground check around the base of the player capsule with an offset.
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
            bool grounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);
            return grounded;
        }

        private bool IsGroundedWhileAirborne()
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;
            
            return _characterController.isGrounded && validAngle;

        }

        private bool CanRun()
        {
            //This means player is moving diagonally at 45 degrees or forward, if so we can run
            return _playerLocomotionInput.MovementInput.y >= Mathf.Abs(_playerLocomotionInput.MovementInput.x);
        }

    }
    
}

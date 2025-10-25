using UnityEngine;

namespace WalkOfLife.FinalCharacterController
{
    public class PlayerAnimiation : MonoBehaviour
    {

        [SerializeField] private Animator _animator;
        
        // used to smooth the blending process between animations
        [SerializeField] private float locoMotionBlendSpeed = 0.02f;
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;

        // NOTE: !!! The Strings in ("") are Case sensitive to whatever you named the animation in the animator in Unity!!!
        private static int inputXHash = Animator.StringToHash("InputX");
        private static int inputYHash = Animator.StringToHash("InputY");
        private static int inputMagnitudeHash = Animator.StringToHash("InputMagnitude");
        private static int isGroundedHash = Animator.StringToHash("IsGrounded");
        private static int isFallingHash = Animator.StringToHash("IsFalling");
        private static int isJumpingHash = Animator.StringToHash("IsJumping");
        
        // used to smooth the blending process between animations
        private Vector3 _currentBlendInput = Vector3.zero;
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }
        private void Update()
        {
            UpdateAnimationState();
        }
        private void UpdateAnimationState()
        {
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
            bool isGrounded = _playerState.IsGroundedState();

       

            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locoMotionBlendSpeed);
            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);
            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);

        }

    }
}
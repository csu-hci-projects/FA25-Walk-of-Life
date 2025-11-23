using System;
using System.Linq;
using NUnit.Framework;
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
        private PlayerController _playerController;
        private PlayerActionInput _playerActionInput;

        // NOTE: !!! The Strings in ("") are Case sensitive to whatever you named the animation in the animator in Unity!!!
        private static int inputXHash = Animator.StringToHash("InputX");
        private static int inputYHash = Animator.StringToHash("InputY");
        private static int inputMagnitudeHash = Animator.StringToHash("InputMagnitude");
        private static int isIdlingHash = Animator.StringToHash("isIdling");
        private static int isGroundedHash = Animator.StringToHash("IsGrounded");
        private static int isFallingHash = Animator.StringToHash("IsFalling");
        private static int isJumpingHash = Animator.StringToHash("IsJumping");
        private static int IsRotatingToTargetHash = Animator.StringToHash("IsRotatingToTarget");
        private static int rotationMissmatchHash = Animator.StringToHash("RotationMissmatch");

        // player interaction hashes
        //private static int isAttackingTH = Animator.StringToHash("IsAttackingTH");
        private static int isAttackingHandsHash = Animator.StringToHash("IsAttackingHands");
        private static int isPlayingActionHash = Animator.StringToHash("IsPlayingAction");
        private int[] actionHashList;
        
        // used to smooth the blending process between animations
        private Vector3 _currentBlendInput = Vector3.zero;

        // blend tree values
        private float _sprintMaxBlendValue = 1.5f;
        private float _runMaxBlendValue = 1.0f;
        private float _walkMaxValue = 0.5f;
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _playerController = GetComponent<PlayerController>();
            _playerActionInput = GetComponent<PlayerActionInput>();
            //Debug.Log(" IN Awake _playerActionInput is:  "+_playerActionInput);
            actionHashList = new int []{isAttackingHandsHash};
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
            bool isPlayingAction = actionHashList.Any(hash => _animator.GetBool(hash));

       

            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isRunBlendValue = isRunning || isSprinting || isFalling;
            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * _sprintMaxBlendValue :
                                  isRunBlendValue ? _playerLocomotionInput.MovementInput * _runMaxBlendValue :_playerLocomotionInput.MovementInput * _walkMaxValue;

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locoMotionBlendSpeed);

            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isIdlingHash, isIdling);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);
            _animator.SetBool(IsRotatingToTargetHash, _playerController.IsRotatingToTarget);
            Debug.Log("PlayerAnimation:80 current player action is:  "+_playerActionInput.AttackPressed);
        
           // _animator.SetBool(isAttackingTH,_playerActionInput.AttackPressed);
            _animator.SetBool(isAttackingHandsHash,_playerActionInput.AttackPressed);
            _animator.SetBool(isPlayingActionHash, isPlayingAction);

            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
            _animator.SetFloat(rotationMissmatchHash, _playerController.RotationMissmatch);

        }

    }
}
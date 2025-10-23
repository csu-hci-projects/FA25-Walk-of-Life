using UnityEngine;

namespace WalkOfLife.FinalCharacterController
{
    public class PlayerAnimiation : MonoBehaviour
    {

        [SerializeField] private Animator _animator;
        
        // used to smooth the blending process between animations
        [SerializeField] private float locoMotionBlendSpeed = 0.02f;
        private PlayerLocomotionInput _playerLocomotionInput;
        private static int inputXHash = Animator.StringToHash("InputX");
        private static int inputYHash = Animator.StringToHash("InputY");
        
        // used to smooth the blending process between animations
        private Vector3 _currentBlendInput = Vector3.zero;
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }
        private void Update()
        {
            UpdateAnimationState();
        }
        private void UpdateAnimationState()
        {

            Vector2 inputTarget = _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locoMotionBlendSpeed);
            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
        }

    }
}
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace WalkOfLife.FinalCharacterController

{
    [DefaultExecutionOrder(-2)]
    public class ThirdPersonInput : MonoBehaviour, PlayerControls.IThirdPersonMapActions
    {
        #region Class variables

        [SerializeField] private CinemachineCamera _virtualCamera;
        [SerializeField] private float _cameraZoomSpeed = .5f;
        [SerializeField] private float _cameraMaxZoom = 2.0f;
        [SerializeField] private float _cameraMinZoom = .5f;
        private CinemachineThirdPersonFollow _thirdPersonFollow;

        public Vector2 ScrollInput { get; private set;}


        #endregion
        #region StartUp
        private void Awake()

        {
            
            _thirdPersonFollow = _virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
        }
        private void OnEnable()
        {

           //PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerLocomotionInput: Player Controller is not Initialized - cannon enable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Enable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerLocomotionInput: Player Controller is not Initialized - cannon Disable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Disable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.RemoveCallbacks(this);
        }
        #endregion
        #region Update
        private void Update()
        {
            _thirdPersonFollow.CameraDistance = Mathf.Clamp(_thirdPersonFollow.CameraDistance + ScrollInput.y,_cameraMinZoom,_cameraMaxZoom);
        }
        private void LateUpdate()
        {
            ScrollInput = Vector2.zero;
        }

        #endregion

        #region Input Callbacks

        public void OnScrollCamera(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            Vector2 scrollInput = context.ReadValue<Vector2>();
            ScrollInput = -1f * scrollInput.normalized * _cameraZoomSpeed;
            print(ScrollInput);
        }
        #endregion
    }
}

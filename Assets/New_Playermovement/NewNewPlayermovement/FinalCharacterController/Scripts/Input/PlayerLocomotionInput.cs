using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
namespace WalkOfLife.FinalCharacterController

{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region Class variables
        [SerializeField] private bool holdToSprint = true;

        public bool SprintToggledOn { get; private set; }
        public bool WalkToggledOn { get; private set; }
        public bool jumpPressed { get; private set; }

      
        public Vector2 MovementInput { get; private set; }

        public Vector2 LookInput { get; private set; }
        #endregion
        #region StartUp
        private void OnEnable()
        {

           //PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerLocomotionInput: Player Controller is not Initialized - cannon enable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerLocomotionInput: Player Controller is not Initialized - cannon Disable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
        #endregion
        #region LateStart
        // lateupdate() updates at the end of every frame where update updates at the beginning this makes it possible to prevent repeat actions like jumping
        private void LateUpdate()
        {
            jumpPressed = false;
        }
        #endregion
        #region Input Callbacks

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            //print(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue < Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggledOn = holdToSprint || !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSprint && SprintToggledOn;
            }
        }

        public void OnJumping(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            jumpPressed = true;
        }

        public void OnToggleWalk(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            WalkToggledOn = !WalkToggledOn;
        }
        #endregion
    }
}

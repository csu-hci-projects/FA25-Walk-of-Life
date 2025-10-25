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
        public bool jumpPressed { get; private set; }

        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }

        public Vector2 LookInput { get; private set; }
        #endregion
        #region StartUp
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
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
            print(MovementInput);
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
        #endregion
    }
}

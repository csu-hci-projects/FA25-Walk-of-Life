using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
namespace WalkOfLife.FinalCharacterController

{
    [DefaultExecutionOrder(-2)]
    public class PlayerActionInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        #region Class variables
        public bool AttackPressed { get; private set;}
        public PlayerControls PlayerControls { get; private set; }

        #endregion
        #region StartUp
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerActionMap.Enable();
            PlayerControls.PlayerActionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            PlayerControls.PlayerActionMap.Disable();
            PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }
        #endregion
        #region LateStart
        public void LateUpdate()
        {
            AttackPressed = false;
        }
        #endregion
        #region Input Callbacks
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            AttackPressed = true;
        }
        #endregion
    }
}

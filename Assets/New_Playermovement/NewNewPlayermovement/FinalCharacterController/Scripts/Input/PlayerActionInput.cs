using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace WalkOfLife.FinalCharacterController

{
    [DefaultExecutionOrder(-2)]
    public class PlayerActionInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        #region Class variables

        [field: SerializeField] public bool AttackPressed { get; private set;} //= false;

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
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerLocomotionInput: Player Controller is not Initialized - cannon Disable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }


        #endregion
        #region Update
        
        public void SetAttackPressedFalse()
        {
            AttackPressed = false;
        }

       
        #endregion

        #region Input Callbacks
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                //SetAttackPressedFalse();
                Debug.Log("PlayerActionInput:59: action was not executed");
                return;
            }
            AttackPressed = true;
        }
       

        #endregion
    }
}

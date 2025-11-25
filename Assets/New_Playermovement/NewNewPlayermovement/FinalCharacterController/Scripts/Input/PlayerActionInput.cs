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

        [field: SerializeField] public bool AttackPressedLeft { get; private set;} //= false;
        [field: SerializeField] public bool AttackPressedRight { get; private set;}

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
        
        public void SetAttackLeftPressedFalse()
        {
            AttackPressedLeft = false;
            
        }
        public void SetAttackRightPressedFalse()
        {
            AttackPressedRight = false;
        }
       
       
        #endregion

        #region Input Callbacks
        

        public void OnAttackLeft(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                //SetAttackPressedFalse();
                //Debug.Log("PlayerActionInput:59: action was not executed");
                return;
            }
            if (!(AttackPressedLeft && AttackPressedRight)){
            AttackPressedLeft = true;
            }
            else
            {
                AttackPressedLeft = false;
                AttackPressedRight = false;
            }
        }

        public void OnAttackRight(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                //SetAttackPressedFalse();
                //Debug.Log("PlayerActionInput:75: action was not executed");
                return;
            }
             if (!(AttackPressedLeft && AttackPressedRight)){
            AttackPressedRight = true;
            }
            else
            {
                AttackPressedRight = false;
                AttackPressedLeft = false;
            }
        }


        #endregion
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI  interactionPromptUI;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;

    private IInteractable interactable;

    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, (int)interactableMask);
        
        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<IInteractable>();

            if(interactable != null)
            {
                if(!interactionPromptUI.IsDisplayed) interactionPromptUI.SetUp(interactable.interactionPrompt);

                if(Keyboard.current.eKey.wasPressedThisFrame) interactable.Interact(this);
            }
        }
        else
        {
            if(interactable != null) interactable = null;
            if(interactionPromptUI.IsDisplayed) interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}

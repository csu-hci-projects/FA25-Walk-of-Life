using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    public string interactionPrompt => prompt;


    public bool Interact(Interactor interactor)
    {
        var Inventory = interactor.GetComponent<Inventory>();

        if(Inventory == null) return false;

        if(Inventory.hasKey)
        {
            Debug.Log("Opening door");
            return true;
        }

        Debug.Log("has no key");
        return false;
    }
}

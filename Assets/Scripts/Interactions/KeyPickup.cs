using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Pick up key";
    public string interactionPrompt => prompt;

    [SerializeField] private KeyType keyType;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<KeyInventory>();
        if (inventory == null)
        {
            Debug.LogWarning("No KeyInventory found on interactor.");
            return false;
        }

        inventory.AddKey(keyType);
        Destroy(gameObject);

        return true;
    }
}

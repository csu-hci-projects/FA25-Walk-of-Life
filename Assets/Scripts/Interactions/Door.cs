using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public string interactionPrompt => prompt;
    [SerializeField] private GameObject door;
    [SerializeField] private float openRot, speed, closeRot;
    [SerializeField] private bool requiresRedKey, requiresGreenKey;
    private bool isOpening = false;
    [SerializeField] private PlayerMessageUI messageUI;
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<KeyInventory>();

    if (inventory != null)
    {
        bool hasRed = inventory.HasRedKey;
        bool hasGreen = inventory.HasGreenKey;

        // If missing keys → show message, stop door from opening
        if (requiresRedKey && !hasRed || requiresGreenKey && !hasGreen)
        {
            string msg = "Door is locked. Missing: ";

            if (requiresRedKey && !hasRed && requiresGreenKey && !hasGreen)
            {
                msg += "RED key and Green key";
                messageUI.ShowMessage(msg);
                return false;
            }
                
            if (requiresRedKey && !hasRed)
                msg += "RED key ";

            if (requiresGreenKey && !hasGreen)
                msg += "GREEN key";

                messageUI.ShowMessage(msg);

            return false;
        }
    }
        isOpening = !isOpening;
        return true;
    }

    private void Update()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        float targetRotation = isOpening ? openRot : closeRot;

        float newY = Mathf.LerpAngle(currentRot.y, targetRotation, speed * Time.deltaTime);
        door.transform.localEulerAngles = new Vector3(currentRot.x, newY, currentRot.z);
    }
}
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    public string interactionPrompt => prompt;

    [SerializeField] private GameObject door;
    [SerializeField] private float openRot, speed, closeRot;

    private bool isOpening = false;

    public bool Interact(Interactor interactor)
    {
        // Toggle the door state
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
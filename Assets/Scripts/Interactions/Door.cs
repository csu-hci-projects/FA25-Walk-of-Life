using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    public string interactionPrompt => prompt;


    public bool Interact(Interactor interactor)
    {
        gameObject.SetActive(false);
        return true;
    }
}

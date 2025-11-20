using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public string interactionPrompt {get;}
    public bool Interact(Interactor interactor);
}
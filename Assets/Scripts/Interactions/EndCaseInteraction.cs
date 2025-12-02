using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCaseInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Pick up bus key";
    public string interactionPrompt => prompt;

    [SerializeField] private bool requiresBlackKey = true;
    [SerializeField] private PlayerMessageUI messageUI;
    //[SerializeField] private string victorySceneName = "VictoryScene";

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<KeyInventory>();

        if (inventory != null)
        {
            bool hasBlack = inventory.HasBlackKey; 
            if (requiresBlackKey && !hasBlack)
            {
                messageUI.ShowMessage("Bus is locked. Missing: BLACK key");
                return false;
            }
        }

        //Still need to make victory scene
        //SceneManager.LoadScene(victorySceneName);
        Debug.Log("Bus started! Loading victory screen");
        return true;
    }
}
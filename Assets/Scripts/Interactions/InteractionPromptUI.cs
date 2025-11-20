using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI _promptText;

    private void Start()
    {
        uiPanel.SetActive(false);
    }

    public void LateUpdate()
    {
        var rotation = mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        
    }

    public bool IsDisplayed = false;

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        uiPanel.SetActive(true);
        IsDisplayed = true;

    }

    public void Close()
    {
        uiPanel.SetActive(false);
        IsDisplayed = false;
    }
}

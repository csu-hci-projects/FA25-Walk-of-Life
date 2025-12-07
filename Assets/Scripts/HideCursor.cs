using UnityEngine;
using UnityEngine.SceneManagement;
public class HideCursor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
       
        if (currentScene.name != "stage1")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.name != "stage1")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(currentScene.name == "stage1")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log("MainMenuManager: the active scene is" + activeScene.name);
        if (activeScene.name == "DeathScreen")
        {
            EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }
}

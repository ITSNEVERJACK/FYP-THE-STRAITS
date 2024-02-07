using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject playPanel;

    public void ShowSettingsPanel()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);

        // Call the function to load the next scene
        LoadNextScene();
    }

    public void Return()
    {
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void CloseProgram()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        // Assuming you have scenes in the build settings, you can use SceneManager to load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextSceneIndex);
    }
}

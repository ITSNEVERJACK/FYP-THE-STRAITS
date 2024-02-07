using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to your pause menu UI GameObject
    [SerializeField] private InputAction escapeAction; // Serialize the InputAction

    private void OnEnable()
    {
        // Subscribe to the "Performed" event of the escape action
        escapeAction.performed += TogglePauseMenu;
    }

    private void OnDisable()
    {
        // Unsubscribe from the "Performed" event to prevent memory leaks
        escapeAction.performed -= TogglePauseMenu;
    }

    private void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1f) // Check if the key was pressed (value is 1)
        {
            if (PauseMenu.isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public static bool isGamePaused = false;

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        pauseMenu.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        pauseMenu.SetActive(true);
    }

    public void LoadMenu(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool gamePaused = false;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Make sure the pause panel is inactive at the start
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("Invalid scene index.");
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the time
        gamePaused = true;
        pausePanel.SetActive(true); // Activate the pause panel
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f; // Unpause the time
        gamePaused = false;
        pausePanel.SetActive(false); // Deactivate the pause panel
    }
}

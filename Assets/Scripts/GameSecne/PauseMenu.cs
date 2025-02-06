using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Private
    [SerializeField] private InputActionReference escActionReference;

    public static bool IsGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject tryAgainMenuUI;
    

    private void OnEnable()
    {
        escActionReference.action.Enable();
        escActionReference.action.performed += ctx => ResumeOrPause();
    }

    private void OnDisable()
    {
        escActionReference.action.performed -= ctx => ResumeOrPause();
        escActionReference.action.Disable();
    }

    public void ResumeOrPause()
    {
        if (IsGamePaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TryAgain()
    {
        tryAgainMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    

}

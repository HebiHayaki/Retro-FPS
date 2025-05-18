using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void Options()
    {

    }

    public void Home() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isPaused = true;
            }
            else
            {
                Resume();
            }
        }
    }
}

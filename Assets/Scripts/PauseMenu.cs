using UnityEngine;
using UnityEditor.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public static bool isPaused;
    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        //pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        pauseMenu.SetActive(false);

    }

    public void Quit()
    {
        Application.Quit();
    }
}

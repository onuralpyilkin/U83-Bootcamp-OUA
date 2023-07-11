using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{
    MenuInputManager menuInputManager;
    public GameObject pauseMenuPanel;
    public GameObject temporaryPanel;
    public static bool gameIsPaused;


    public string sceneName;

    void Start()
    {
        menuInputManager = MenuInputManager.Instance;
    }

    /*private void Update()
    {
        PresstoPause();
    }*/

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        temporaryPanel.SetActive(true);
        menuInputManager.GetCurrentPanel();
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Restart()
    {
        pauseMenuPanel.SetActive(false);
        temporaryPanel.SetActive(true);
        menuInputManager.GetCurrentPanel();
        Time.timeScale = 1f;
        gameIsPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        temporaryPanel.SetActive(false);
        menuInputManager.GetCurrentPanel();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void PresstoPause()
    {

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
                Restart();

            }
            else
            {
                Pause();
            }
        }*/

        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
        menuInputManager.GetCurrentPanel();
    }
}
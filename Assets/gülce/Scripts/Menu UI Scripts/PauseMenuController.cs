using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance;
    MenuInputManager menuInputManager;
    //public AudioSource audioSource;
    public GameObject pauseMenuPanel;
    public GameObject temporaryPanel;
    public CanvasGroup gameUI;
    public static bool gameIsPaused = false;


    public string sceneName;
    public UnityEvent OnPanelClose;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        menuInputManager = MenuInputManager.Instance;
        gameIsPaused = false;
    }

    /*private void Update()
    {
        PresstoPause();
    }*/

    public void Resume()
    {
        //audioSource.Pause();
        DG.Tweening.DOTween.To(() => gameUI.alpha, x => gameUI.alpha = x, 1, 0.5f);
        DG.Tweening.DOTween.To(() => pauseMenuPanel.GetComponent<CanvasGroup>().alpha, x => pauseMenuPanel.GetComponent<CanvasGroup>().alpha = x, 0, 0.5f).onComplete += () =>
        {
            SwitchPanel(pauseMenuPanel, temporaryPanel);
        };
        Time.timeScale = 1f;
        gameIsPaused = false;
        PlayerInputManager.Instance.EnableInput();
    }

    public void Restart()
    {
        //audioSource.Pause();
        SwitchPanel(pauseMenuPanel, temporaryPanel);
        Time.timeScale = 1f;
        gameIsPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void Pause()
    {
        //audioSource.Play();
        DG.Tweening.DOTween.To(() => gameUI.alpha, x => gameUI.alpha = x, 0, 0.5f);
        pauseMenuPanel.GetComponent<CanvasGroup>().alpha = 1;
        SwitchPanel(temporaryPanel, pauseMenuPanel);
        Time.timeScale = 0f;
        gameIsPaused = true;
        PlayerInputManager.Instance.DisableInput();
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
        //audioSource.Pause();
        //SwitchPanel(pauseMenuPanel, temporaryPanel);
        //SceneManager.LoadScene(sceneName);
        //menuInputManager.GetCurrentPanel();
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene(), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void SwitchPanel(GameObject sourcePanel, GameObject targetPanel)
    {
        if (OnPanelClose != null)
            OnPanelClose.Invoke();
        MenuInputManager.Instance.SetInputActive(false);
        //yield return new WaitForSeconds(delay);
        sourcePanel.SetActive(false);
        targetPanel.SetActive(true);
        MenuInputManager.Instance.GetCurrentPanel();
        MenuInputManager.Instance.SetInputActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Events;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    MenuInputManager menuInputManager;
    public string sceneName;
    public GameObject settingsPanel;
    public float settingsPanelCloseDelay = 0.5f;
    public GameObject mainMenuPanel;
    public float mainMenuPanelCloseDelay = 0.5f;
    public GameObject creditsPanel;
    public float creditsPanelCloseDelay = 0.5f;
    public GameObject levelsPanel;
    public float levelsPanelCloseDelay = 0.5f;
    public GameObject gamepadLoadingScreen;
    public GameObject keyboardLoadingScreen;
    public CanvasGroup LoadingScreenContinueCanvasGroup;
    public GameObject LoadingScreenGamepadContinueButton;
    public GameObject LoadingScreenKeyboardContinueButton;

    public GameObject settingsMenuButton, creditsMenuButton, creditsMenuBackButton;

    public UnityEvent OnPanelClose;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
        mainMenuPanel.SetActive(true);
    }

    void Start()
    {
        menuInputManager = MenuInputManager.Instance;
    }

    public void NewGame(int levelId)
    {
        string levelName = "LEVEL" + levelId; //Seviye adını belirle
        StartCoroutine(LoadAsync(levelName));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Done!");
    }

    public void OpenLoadGame()
    {
        // Levels Paneli aç
        StartCoroutine(SwitchPanel(mainMenuPanel, levelsPanel, mainMenuPanelCloseDelay));
        menuInputManager.IsCurrentPanelVertical = false;
    }

    public void CloseLoadGame()
    {
        StartCoroutine(SwitchPanel(levelsPanel, mainMenuPanel, levelsPanelCloseDelay));
        menuInputManager.IsCurrentPanelVertical = true;
    }

    public void OpenSettings()
    {
        // Settings panelini aç
        StartCoroutine(SwitchPanel(mainMenuPanel, settingsPanel, mainMenuPanelCloseDelay));
    }

    public void CloseSettings()
    {
        // Settings panelini kapatıp main menu panelini aç
        StartCoroutine(SwitchPanel(settingsPanel, mainMenuPanel, settingsPanelCloseDelay));
    }

    public void OpenCredits()
    {
        // Credits panelini aç
        StartCoroutine(SwitchPanel(mainMenuPanel, creditsPanel, mainMenuPanelCloseDelay));
    }
    public void CloseCredits()
    {
        // Credits panelini kapat
        StartCoroutine(SwitchPanel(creditsPanel, mainMenuPanel, creditsPanelCloseDelay));
    }

    IEnumerator LoadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName); //Seviyeyi yükle
        operation.allowSceneActivation = false;

        //gamepadLoadingScreen.SetActive(true); //bunun yerine SetActiveLoadingScreen() gelicek
        SetActiveLoadingScreen();

        float alphaTarget = 1f;
        bool isEventAdded = false;
        Slider slider;
        if(gamepadLoadingScreen.activeSelf)
        {
            slider = gamepadLoadingScreen.GetComponentInChildren<Slider>();
        }
        else
        {
            slider = keyboardLoadingScreen.GetComponentInChildren<Slider>();
        }

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            if (operation.progress >= 0.9f)
            {
                LoadingScreenContinueCanvasGroup.gameObject.SetActive(true);
                LoadingScreenContinueCanvasGroup.alpha = Mathf.MoveTowards(LoadingScreenContinueCanvasGroup.alpha, alphaTarget, 2f * Time.deltaTime);
                if (LoadingScreenContinueCanvasGroup.alpha >= 1f)
                {
                    alphaTarget = 0f;
                }
                if (LoadingScreenContinueCanvasGroup.alpha <= 0f)
                {
                    alphaTarget = 1f;
                }

                if (!isEventAdded)
                {
                    MenuInputManager.Instance.OnSubmit.AddListener(() =>
                {
                    operation.allowSceneActivation = true;
                    MenuInputManager.Instance.OnSubmit.RemoveAllListeners();
                });
                    isEventAdded = true;
                }
            }

            yield return null;
        }

    }

    IEnumerator SwitchPanel(GameObject sourcePanel, GameObject targetPanel, float delay)
    {
        if (OnPanelClose != null)
            OnPanelClose.Invoke();
        MenuInputManager.Instance.SetInputActive(false);
        yield return new WaitForSeconds(delay);
        sourcePanel.SetActive(false);
        targetPanel.SetActive(true);
        MenuInputManager.Instance.GetCurrentPanel();
        MenuInputManager.Instance.SetInputActive(true);
    }

    public void SetActiveLoadingScreen()
    {
        if (MenuInputManager.Instance.LatestInputIsGamepad)  //hangi koşul gelicek sor
        {
            //gamepad kullanılıyorsa
            gamepadLoadingScreen.SetActive(true);
            keyboardLoadingScreen.SetActive(false);
            LoadingScreenGamepadContinueButton.SetActive(true);
            LoadingScreenKeyboardContinueButton.SetActive(false);
        }
        else
        {
            //keyboard kullanılıyorsa
            gamepadLoadingScreen.SetActive(false);
            keyboardLoadingScreen.SetActive(true);
            LoadingScreenGamepadContinueButton.SetActive(false);
            LoadingScreenKeyboardContinueButton.SetActive(true);
        }
    }


}

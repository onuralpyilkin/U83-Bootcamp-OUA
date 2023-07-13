using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    MenuInputManager menuInputManager;
    MenuUIButtons menuUIButtons;
    public string sceneName;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject levelsPanel;
    public GameObject gamepadLoadingScreen;
    public GameObject keyboardLoadingScreen;

    public Slider slider;

    public GameObject settingsMenuButton, creditsMenuButton, creditsMenuBackButton;

    void Start()
    {
        menuInputManager = MenuInputManager.Instance;
        menuUIButtons = FindObjectOfType<MenuUIButtons>();
    }

    public void NewGame(int levelId)
    {
        menuUIButtons.OnPointerEnter();
        string levelName = "LEVEL" + levelId; //Seviye adını belirle
        StartCoroutine(LoadAsync(levelName));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Done!");
        menuUIButtons.OnPointerEnter();
    }

    public void OpenLoadGame()
    {
        // Levels Paneli aç
        levelsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        menuInputManager.GetCurrentPanel();
        menuInputManager.IsCurrentPanelVertical = false;
    }

    public void CloseLoadGame()
    {
        levelsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        menuInputManager.GetCurrentPanel();
        menuInputManager.IsCurrentPanelVertical = true;
    }

    public void OpenSettings()
    {
        // Settings panelini aç
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        menuInputManager.GetCurrentPanel();
    }

    public void CloseSettings()
    {
        // Settings panelini kapatıp main menu panelini aç
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(settingsMenuButton);
        menuInputManager.GetCurrentPanel();
    }

    public void OpenCredits()
    {
        // Credits panelini aç
        creditsPanel.SetActive(true);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(creditsMenuBackButton);
        menuInputManager.GetCurrentPanel();
    }
    public void CloseCredits()
    {
        // Credits panelini kapat
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);


        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(creditsMenuButton);
        menuInputManager.GetCurrentPanel();
    }
    
    IEnumerator LoadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName); //Seviyeyi yükle

        gamepadLoadingScreen.SetActive(true); //bunun yerine SetActiveLoadingScreen() gelicek

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }

    }

    //public void SetActiveLoadingScreen()
    //{
    //    if ()  //hangi kıoşul gelicek sor
    //    {
    //        //gamepad kullanılıyorsa
    //        gamepadLoadingScreen.SetActive(true);
    //        keyboardLoadingScreen.SetActive(false);
    //    }
    //    else
    //    {
    //        //keyboard kullanılıyorsa
    //        gamepadLoadingScreen.SetActive(false);
    //        keyboardLoadingScreen.SetActive(true);
    //    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    MenuInputManager menuInputManager;
    public string sceneName;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject levelsPanel;


    public GameObject settingsMenuButton, creditsMenuButton, creditsMenuBackButton;

    void Start()
    {
        menuInputManager = MenuInputManager.Instance;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting Done!");
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
        // Credits panelini aç
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);


        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(creditsMenuButton);
        menuInputManager.GetCurrentPanel();
    }

}

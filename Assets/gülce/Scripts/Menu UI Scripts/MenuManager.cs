using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    public string sceneName;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject levelsPanel;


    public GameObject settingsMenuButton, creditsMenuButton, creditsMenuBackButton;



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

    }

    public void OpenSettings()
    {
        // Settings panelini aç
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
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
    }

}

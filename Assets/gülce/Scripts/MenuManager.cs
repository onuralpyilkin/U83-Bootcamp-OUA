using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    public void OnButtonClick()
    {
        Debug.Log("Button Clicked!");

        // Settings panelini aç
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

    }

    public void OnBackButtonClick()
    {
        // Settings panelini kapatıp main menu panelini aç
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

}

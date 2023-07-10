using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelMenuFirstButton;


    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Oyunda açılan son seviyenin indeksini al, varsayılan olarak 1 i ayarla
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false; //Butonları devre dışı bırak
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true; //Açılan seviyelere kadar olan butonları tıklanabilir yap
        }
    }
    private void Start()
    {
        //Seçili nesneyi temizle
        EventSystem.current.SetSelectedGameObject(null);
        //Yeni bir seçili nesne belirle
        EventSystem.current.SetSelectedGameObject(levelMenuFirstButton);
        MenuInputManager.MenuPanel levelPanel = MenuInputManager.Instance.GetPanel("Levels");
        for (int i = 0; i < levelPanel.buttons.Count; i++)
        {
            MenuInputManager.MenuButton button = levelPanel.buttons[i];
            if(buttons[i].interactable == false)
                levelPanel.buttons.Remove(button);
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "LEVEL" + levelId; //Seviye adını belirle
        SceneManager.LoadScene(levelName); //Seviyeyi yükle
    }
}

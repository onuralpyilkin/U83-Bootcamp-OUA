using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    MenuUIButtons menuUIButtons;
    public Button[] buttons;
    public GameObject levelMenuFirstButton;
    public GameObject gamepadLoadingScreen;
    public GameObject keyboardLoadingScreen;

    public Slider slider;



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
        menuUIButtons = FindObjectOfType<MenuUIButtons>();
        //Seçili nesneyi temizle
        EventSystem.current.SetSelectedGameObject(null);
        //Yeni bir seçili nesne belirle
        EventSystem.current.SetSelectedGameObject(levelMenuFirstButton);
        MenuInputManager.MenuPanel levelPanel = MenuInputManager.Instance.GetPanel("Levels");
        List<MenuInputManager.MenuButton> buttonsToRemove = new List<MenuInputManager.MenuButton>();
        for (int i = 0; i < levelPanel.buttons.Count; i++)
        {
            MenuInputManager.MenuButton button = levelPanel.buttons[i];
            if (buttons[i].interactable == false)
            {
                buttonsToRemove.Add(button);
                //Debug.Log("Button " + button.name + " removed from list");
            }
        }

        foreach (MenuInputManager.MenuButton button in buttonsToRemove)
        {
            levelPanel.buttons.Remove(button);
        }
    }

    public void OpenLevel(int levelId)
    {
        menuUIButtons.OnPointerEnter();
        string levelName = "LEVEL" + levelId; //Seviye adını belirle
        StartCoroutine(LoadAsync(levelName));
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

    //}
}

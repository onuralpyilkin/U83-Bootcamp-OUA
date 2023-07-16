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
    public CanvasGroup LoadingScreenContinueCanvasGroup;
    public GameObject LoadingScreenGamepadContinueButton;
    public GameObject LoadingScreenKeyboardContinueButton;



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

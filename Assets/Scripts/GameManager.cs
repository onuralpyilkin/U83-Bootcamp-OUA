using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gamepadLoadingScreen;
    public GameObject keyboardLoadingScreen;
    public CanvasGroup LoadingScreenContinueCanvasGroup;
    public GameObject LoadingScreenGamepadContinueButton;
    public GameObject LoadingScreenKeyboardContinueButton;

    public List<GameObject> ObjectToEnableAtStart = new List<GameObject>();
    public List<GameObject> ObjectToDisableAfterTimeline = new List<GameObject>();
    public List<GameObject> ObjectToDestroyAfterTimeline = new List<GameObject>();
    public static string LastSceneName;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        for (int i = 0; i < ObjectToEnableAtStart.Count; i++)
        {
            ObjectToEnableAtStart[i].SetActive(true);
        }
    }

    public void ActivateComponents()
    {
        PlayerInputManager.Instance.EnableInput();
        //write here functions that enables all needed components
    }

    public void DeactivateComponents()
    {
        PlayerInputManager.Instance.DisableInput();
        //write here functions that disables all needed components
    }

    public void DisableObjects()
    {
        for (int i = 0; i < ObjectToDisableAfterTimeline.Count; i++)
        {
            ObjectToDisableAfterTimeline[i].SetActive(false);
        }
    }

    public void DestroyObjects()
    {
        StartCoroutine(DestroyObjectsCoroutine(0.5f));
    }

    IEnumerator DestroyObjectsCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < ObjectToDestroyAfterTimeline.Count; i++)
        {
            Destroy(ObjectToDestroyAfterTimeline[i]);
        }
        ObjectToDestroyAfterTimeline.Clear();
    }

    public void LoadLevel(int levelId)
    {
        string levelName = "LEVEL" + levelId; //Seviye adını belirle
        StartCoroutine(LoadAsync(levelName));
    }

    public void LoadLevel(string levelName, bool isLevel = true)
    {
        StartCoroutine(LoadAsync(levelName, false));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName, false));
    }

    public void LoadLastScene()
    {
        if(LastSceneName != null)
            StartCoroutine(LoadAsync(LastSceneName, false));
    }

    IEnumerator LoadAsync(string levelName, bool isLevel = true)
    {
        LastSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName); //Seviyeyi yükle
        operation.allowSceneActivation = !isLevel;

        //gamepadLoadingScreen.SetActive(true); //bunun yerine SetActiveLoadingScreen() gelicek

        if (isLevel)
        {
            SetActiveLoadingScreen();

            float alphaTarget = 1f;
            bool isEventAdded = false;
            Slider slider;
            if (gamepadLoadingScreen.activeSelf)
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
        else
        {
            while (operation.isDone == false)
            {
                yield return null;
            }
        }
        yield break;
    }

    public void SetActiveLoadingScreen()
    {
        if (LastInputType.isGamepad)  //hangi koşul gelicek sor
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

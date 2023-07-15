using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StoryBoardController : MonoBehaviour
{
    public CanvasGroup parentCanvas;
    public Image[] storyBoards;
    public CanvasGroup continueText;
    public GameObject continueButtonKeyboard;
    public GameObject continueButtonGamepad;
    public float revealTime = 1f;
    private int currentStoryBoard = 0;

    public UnityEvent OnPanelEnable;
    public UnityEvent OnPanelClose;
    private bool isContinueTriggered = false;

    void Start()
    {
        PlayerInputManager.Instance.DisablePlayerInput();
        PlayerInputManager.Instance.EnableUIInput();
        PlayerInputManager.Instance.OnSubmit.AddListener(Continue);
        if (OnPanelEnable != null)
            OnPanelEnable.Invoke();
    }

    void Update()
    {
        if (isContinueTriggered)
            return;
        if (parentCanvas.alpha < 1f)
        {
            parentCanvas.alpha = Mathf.MoveTowards(parentCanvas.alpha, 1f, revealTime * Time.deltaTime);
            return;
        }

        if (currentStoryBoard > storyBoards.Length - 1)
        {
            continueText.alpha = Mathf.MoveTowards(continueText.alpha, 1f, revealTime * Time.deltaTime);
            if (PlayerInputManager.Instance.LatestInputIsGamepad)
            {
                continueButtonGamepad.SetActive(true);
                continueButtonKeyboard.SetActive(false);
            }
            else
            {
                continueButtonGamepad.SetActive(false);
                continueButtonKeyboard.SetActive(true);
            }
            return;
        }

        storyBoards[currentStoryBoard].color = Vector4.MoveTowards(storyBoards[currentStoryBoard].color, Color.white, revealTime * Time.deltaTime);
        if (storyBoards[currentStoryBoard].color.a >= Color.white.a)
        {
            currentStoryBoard++;
        }
    }

    public void Continue()
    {
        if (currentStoryBoard > storyBoards.Length - 1 && !isContinueTriggered)
        {
            isContinueTriggered = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        while (parentCanvas.alpha > 0f)
        {
            parentCanvas.alpha -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnSubmit.RemoveListener(Continue);
        PlayerInputManager.Instance.EnablePlayerInput();
        PlayerInputManager.Instance.DisableUIInput();
        OnPanelClose.Invoke();
    }
}

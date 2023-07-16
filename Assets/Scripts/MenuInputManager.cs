using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuInputManager : MonoBehaviour
{
    [System.Serializable]
    public struct MenuPanel
    {
        public string name;
        public GameObject panel;
        public UnityEvent OnPanelBack;
        public List<MenuButton> buttons;
        MenuPanel(string name, GameObject panel, List<MenuButton> buttons, UnityEvent OnPanelBack)
        {
            this.name = name;
            this.panel = panel;
            this.buttons = buttons;
            this.OnPanelBack = OnPanelBack;
        }
    }

    [System.Serializable]
    public struct MenuButton
    {
        public string name;
        public UnityEvent OnPointerEnter;
        public UnityEvent OnPointerExit;
        public UnityEvent OnPositiveButtonClick;
        public UnityEvent OnNegativeButtonClick;
        MenuButton(string name, UnityEvent OnPointerEnter, UnityEvent OnPointerExit, UnityEvent OnPositiveButtonClick, UnityEvent OnNegativeButtonClick)
        {
            this.name = name;
            this.OnPointerEnter = OnPointerEnter;
            this.OnPointerExit = OnPointerExit;
            this.OnPositiveButtonClick = OnPositiveButtonClick;
            this.OnNegativeButtonClick = OnNegativeButtonClick;
        }
    }


    public static MenuInputManager Instance;
    MenuInput input;
    public bool IsPauseMenu = false;
    public List<MenuPanel> panels;
    private int currentPanelIndex = -1;
    private int currentButtonIndex = -1;
    private MenuButton currentButton;
    [HideInInspector]
    public bool IsCurrentPanelVertical = true;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        input = new MenuInput();
        input.Menu.Up.performed += ctx => DirectionCorrector();
        input.Menu.Down.performed += ctx => DirectionCorrector(true, false);
        input.Menu.Right.performed += ctx => DirectionCorrector(false);
        input.Menu.Left.performed += ctx => DirectionCorrector(false, false);
        input.Menu.Submit.performed += ctx => Submit();
        input.Menu.Cancel.performed += ctx => Cancel();
        if (IsPauseMenu)
            input.Menu.PauseResume.performed += ctx => PauseResume();
        input.Menu.Enable();
        GetCurrentPanel();
    }

    void DirectionCorrector(bool isUpDown = true, bool isPositiveMovement = true)
    {
        if (IsCurrentPanelVertical)
        {
            if (isUpDown)
            {
                UpDownMove(isPositiveMovement);
            }
            else
            {
                LeftRightMove(isPositiveMovement);
            }
        }
        else
        {
            if (isUpDown)
            {
                LeftRightMove(isPositiveMovement);
            }
            else
            {
                UpDownMove(isPositiveMovement);
            }
        }
    }

    void UpDownMove(bool isUp = true)
    {
        if (currentButtonIndex != -1)
            currentButton.OnPointerExit.Invoke();
        currentButtonIndex += isUp ? -1 : 1;
        if (currentButtonIndex < 0)
            currentButtonIndex = panels[currentPanelIndex].buttons.Count - 1;
        currentButtonIndex = currentButtonIndex % panels[currentPanelIndex].buttons.Count;
        currentButton = panels[currentPanelIndex].buttons[currentButtonIndex];
        currentButton.OnPointerEnter.Invoke();
    }

    void LeftRightMove(bool isRight = true)
    {
        if (currentButton.OnNegativeButtonClick.GetPersistentEventCount() == 0)
            return;
        if (isRight)
            currentButton.OnPositiveButtonClick.Invoke();
        else
            currentButton.OnNegativeButtonClick.Invoke();
    }

    void Submit()
    {
        if (currentButtonIndex == -1)
            return;
        ResetCurrentPanelButtonScales();
        currentButton.OnPositiveButtonClick.Invoke();
        currentButtonIndex = -1;
    }

    void Cancel()
    {
        if (IsPauseMenu && !PauseMenuController.gameIsPaused)
            return;
        if (currentPanelIndex == -1)
            return;
        ResetCurrentPanelButtonScales();
        panels[currentPanelIndex].OnPanelBack.Invoke();
        currentButtonIndex = -1;
    }

    void PauseResume()
    {
        if (currentPanelIndex == -1)
            return;
        ResetCurrentPanelButtonScales();
        panels[currentPanelIndex].OnPanelBack.Invoke();
        currentButtonIndex = -1;
    }

    public void ResetCurrentPanelButtonScales()
    {
        for (int i = 0; i < panels[currentPanelIndex].buttons.Count; i++)
        {
            panels[currentPanelIndex].buttons[i].OnPointerExit.Invoke();
        }
    }

    public void GetCurrentPanel()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i].panel.activeSelf)
            {
                currentPanelIndex = i;
                Debug.Log("Current Panel: " + panels[i].name);
                currentButtonIndex = -1;
                break;
            }
            //Debug.LogWarning("No active panel found!");
        }
    }

    public MenuPanel GetPanel(string name)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i].name == name)
            {
                return panels[i];
            }
        }
        Debug.LogWarning("No panel found with name: " + name);
        return panels[0];
    }

    public void SetInputActive(bool isActive)
    {
        if (isActive)
            input.Menu.Enable();
        else
            input.Menu.Disable();
    }
}

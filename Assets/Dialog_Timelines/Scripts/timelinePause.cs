using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timelinePause : MonoBehaviour
{
    public KeyCode resumePlaying = KeyCode.E;
    public Text displayText;
    public Image pressX;
    public Image pressE;

    private void OnEnable() {
        Time.timeScale = 0;
    }
    private void OnDisable() {
        Time.timeScale = 1;
    }

    private void Update() {



        bool isGamepadConnected = Input.GetJoystickNames().Length > 0;
        if (isGamepadConnected)
        {
            pressX.gameObject.SetActive(true);
            pressE.gameObject.SetActive(false);
        }
        else
        {
            pressE.gameObject.SetActive(true);
            pressX.gameObject.SetActive(false);
        }
        
        if(Input.GetKeyDown(resumePlaying) || (Input.GetButtonDown("Submit")))
        {
            Time.timeScale = 1;
        }
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timelinePause : MonoBehaviour
{
    public KeyCode resumePlaying = KeyCode.E;
    public Text displayText;

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
            displayText.text = "Devam etmek icin JoystickButton2'ye basın!";
        }
        else
        {
            displayText.text = "Devam etmek icin E'ye basın!";
        }
        
        if(Input.GetKeyDown(resumePlaying) || (Input.GetButtonDown("Submit")))
        {
            Time.timeScale = 1;
        }
    
    }
}

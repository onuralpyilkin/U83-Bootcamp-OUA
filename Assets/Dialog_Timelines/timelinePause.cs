using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timelinePause : MonoBehaviour
{
    public KeyCode resumePlaying = KeyCode.E;

    private void OnEnable() {
        Time.timeScale = 0;
    }
    private void OnDisable() {
        Time.timeScale = 1;
    }


    private void Update() {
        if(Input.GetKeyDown(resumePlaying))
        {
            Time.timeScale = 1;
        }
    }
}

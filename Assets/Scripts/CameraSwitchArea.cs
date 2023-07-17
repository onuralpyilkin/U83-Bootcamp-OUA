using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchArea : MonoBehaviour
{
    public GameObject cameraToSwitch;
    public GameObject[] camerasToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToSwitch.SetActive(true);
            foreach (GameObject camera in camerasToDisable)
            {
                camera.SetActive(false);
            }
            CameraController.Instance.CombatCamera = cameraToSwitch;
            StartCoroutine(CombatCameraSwitchDelay());
        }
    }

    IEnumerator CombatCameraSwitchDelay()
    {
        yield return new WaitForSeconds(0.5f);
        CameraController.Instance.SetCombatCameraState(true);
    }
}

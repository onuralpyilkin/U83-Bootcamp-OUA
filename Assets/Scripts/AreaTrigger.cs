using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public bool isCombatArea = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isCombatArea)
            {
                CameraController.Instance.SetCombatCameraState(true);
            }
            else
            {
                CameraController.Instance.SetCombatCameraState(false);
            }
        }
    }
}

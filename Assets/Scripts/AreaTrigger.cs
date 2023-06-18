using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public bool IsCombatArea = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsCombatArea)
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

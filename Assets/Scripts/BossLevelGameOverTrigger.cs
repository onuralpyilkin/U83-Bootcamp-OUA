using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossLevelGameOverTrigger : MonoBehaviour
{
    public UnityEvent OnGameOver;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGameOver.Invoke();
        }
    }
}

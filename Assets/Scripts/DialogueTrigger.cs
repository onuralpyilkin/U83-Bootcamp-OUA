using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public UnityEvent OnDialogueTriggered;
    public UnityEvent OnDialogueTriggeredWithDelay;
    public float Delay = 0f;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDialogueTriggered.Invoke();
            StartCoroutine(DelayCoroutine(Delay));
        }
    }

    IEnumerator DelayCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        OnDialogueTriggeredWithDelay.Invoke();
    }
}

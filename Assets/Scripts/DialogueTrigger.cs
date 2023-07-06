using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public UnityEvent OnDialogueTriggered;
    public UnityEvent OnDialogueFinished;

    private PlayableDirector timeline;

    private void Start() {
        timeline = GetComponent<PlayableDirector>();
        timeline.stopped += OnTimelineFinish;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDialogueTriggered.Invoke();
            timeline.Play();
        }
    }

    private void OnTimelineFinish(PlayableDirector director)
    {
        OnDialogueFinished.Invoke();
        Destroy(gameObject);
    }
}

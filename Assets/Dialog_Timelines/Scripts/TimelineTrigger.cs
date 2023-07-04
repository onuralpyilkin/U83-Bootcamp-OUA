using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector timeline;

    private void Start() {
        timeline.stopped += OnTimelineFinish;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeline.Play();
        }
    }

    private void OnTimelineFinish(PlayableDirector director)
    {
        Destroy(gameObject);
    }
}

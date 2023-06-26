using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTrigger : MonoBehaviour
{
    public TimelineController timelineController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timelineController.StartTimeline();
        }
    }
}

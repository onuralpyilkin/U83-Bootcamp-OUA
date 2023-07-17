using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelCompleteManager : MonoBehaviour
{
    public int EventCallCountToLevelComplete = 1;
    public UnityEvent OnLevelComplete;

    public void OnEventCalled()
    {
        EventCallCountToLevelComplete--;
        if (EventCallCountToLevelComplete <= 0)
        {
            OnLevelComplete.Invoke();
        }
    }
}

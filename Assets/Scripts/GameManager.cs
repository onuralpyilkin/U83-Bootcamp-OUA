using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> ObjectToEnableAtStart = new List<GameObject>();
    public List<GameObject> ObjectToDisableAfterTimeline = new List<GameObject>();
    public List<GameObject> ObjectToDestroyAfterTimeline = new List<GameObject>();

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        for (int i = 0; i < ObjectToEnableAtStart.Count; i++)
        {
            ObjectToEnableAtStart[i].SetActive(true);
        }
    }

    public void ActivateComponents()
    {
        PlayerInputManager.Instance.EnableInput();
        //write here functions that enables all needed components
    }

    public void DeactivateComponents()
    {
        PlayerInputManager.Instance.DisableInput();
        //write here functions that disables all needed components
    }

    public void DisableObjects()
    {
        for (int i = 0; i < ObjectToDisableAfterTimeline.Count; i++)
        {
            ObjectToDisableAfterTimeline[i].SetActive(false);
        }
    }

    public void DestroyObjects()
    {
        StartCoroutine(DestroyObjectsCoroutine(0.5f));
    }

    IEnumerator DestroyObjectsCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < ObjectToDestroyAfterTimeline.Count; i++)
        {
            Destroy(ObjectToDestroyAfterTimeline[i]);
        }
        ObjectToDestroyAfterTimeline.Clear();
    }
}

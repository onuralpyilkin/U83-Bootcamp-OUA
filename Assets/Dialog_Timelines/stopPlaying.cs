using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopPlaying : MonoBehaviour
{
    public GameObject canvas;
    void OnEnable()
    {
        Destroy(canvas);
    }
}

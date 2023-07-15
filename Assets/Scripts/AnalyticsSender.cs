using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsSender : MonoBehaviour
{
    void Start()
    {
        string lvlnum = "";
        string sceneName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < sceneName.Length; i++)
                {
                    if (char.IsDigit(sceneName[i]))
                    {
                        lvlnum += sceneName[i];
                    }
                }
        
       
        var level = new Dictionary<string, object>();
        level["level"]=lvlnum;
        Analytics.CustomEvent("level", level);
        
        
    }

    void Update()
    {
        
    }
}

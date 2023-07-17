using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsSender : MonoBehaviour
{
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

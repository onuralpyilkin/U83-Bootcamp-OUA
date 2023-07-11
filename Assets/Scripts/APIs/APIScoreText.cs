using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;


[System.Serializable]
public class FirebaseData
{
    public int BestComboCount;
}

public class APIScoreText : MonoBehaviour
{
    public Text outputText;

    private void Start()
    {
        StartCoroutine(GetBestComboCount());
    }

    private IEnumerator GetBestComboCount()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/.json"))
        {
            yield return www.SendWebRequest();
            
            string json = www.downloadHandler.text;
            var data = JsonUtility.FromJson<Dictionary<string, FirebaseData>>(json);
            string output = "";
            foreach (var item in data)
            {
                if (item.Key == "BestComboCount")
                {
                    output += item.Value.BestComboCount.ToString() + "\n";
                }
            }
            outputText.text = "Best Combo Count: " + output;
        }
    }


    
    // public Text comboCountText;
    // private string databaseURL = "https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/";

    // private void Start()
    // {
    //     StartCoroutine(GetBestComboCount());
    // }

    // IEnumerator GetBestComboCount()
    // {
    //     using (UnityWebRequest request = UnityWebRequest.Get(databaseURL + ".json"))
    //     {
    //         yield return request.SendWebRequest();
    //         string jsonResponse = request.downloadHandler.text;
    //         var response = JsonUtility.FromJson<FirebaseData>(jsonResponse);
    //             int BestComboCount = response.BestComboCount;
    //             comboCountText.text = "Best Combo Count: " + BestComboCount.ToString();
    //     }
    // }
}
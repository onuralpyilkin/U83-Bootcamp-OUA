using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class APIScoreText : MonoBehaviour
{
    // public GameObject textPrefab;
    // public Transform textParent;
    // private string databaseURL = "https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/.json";

    // private void Start()
    // {
        // StartCoroutine(GetBestComboCounts());
    // }

    // IEnumerator GetBestComboCounts()
    // {
        // using (UnityWebRequest request = UnityWebRequest.Get(databaseURL))
        // {
            // yield return request.SendWebRequest();

            // if (request.result == UnityWebRequest.Result.Success)
            // {
                // string jsonResponse = request.downloadHandler.text;
                // var firebaseData = JsonUtility.FromJson<Dictionary<string, ComboData>>(jsonResponse);

                // foreach (var data in firebaseData)
                // {
                    // int bestComboCount = data.Value.BestComboCount;
                    // CreateTextObject(data.Key, bestComboCount.ToString());
                // }
            // }
            // else
            // {
                // Debug.LogError("Error getting Firebase data: " + request.error);
            // }
        // }
    // }

    // private void CreateTextObject(string ID, string comboCount)
    // {
        // GameObject newText = Instantiate(textPrefab, textParent);
        // newText.GetComponent<Text>().text = "ID: " + ID + " | Best Combo Count: " + comboCount;
    // }

    // [System.Serializable]
    // private class ComboData
    // {
        // public int BestComboCount;
        // public string ID;
        // public int TotalHitCount;
    // }


    
    public Text comboCountText;
    private string databaseURL = "https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/";

    private void Start()
    {
        StartCoroutine(GetBestComboCount());
    }

    IEnumerator GetBestComboCount()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(databaseURL + ".json"))
        {
            yield return request.SendWebRequest();

            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<BestComboCountResponse>(jsonResponse);
            if (response != null)
            {
                int BestComboCount = response.BestComboCount;
                comboCountText.text = "Best Combo Count: " + BestComboCount.ToString();
            }
        }
    }

    [System.Serializable]
    private class BestComboCountResponse
    {
        public int BestComboCount;
    }

}
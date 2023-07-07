using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class TelegramAPI : MonoBehaviour
{
    [HideInInspector]private string TelegramBotToken = "6303462751:AAESnqjmKbmSrx216v-wsciNqrJMOAITmKU";
    [HideInInspector]private static string PlayerPrefKey = "PlayerID";
    [HideInInspector]private string ChatID = "-1001941348433";
    private bool deneme = false;


    
    void Start()
    {
        PlayerPrefKey = PlayerPrefs.GetString(PlayerPrefKey);
        StartCoroutine(UpdateVariableValue());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deneme = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            deneme = false;
        }
    }

    //TELEGRAM API
    private IEnumerator UpdateVariableValue()
    {
        while (true)
        {
            if (deneme)
            {
                // telegrama mesaj gönderme
                // string message = $"BestComboCount: {BestComboCount}\nKullanıcı ID: {PlayerPrefs.GetString(PlayerPrefKey)}";
                string message = $"🎉 MUHTESEM!\n Kullanıcı ID: {PlayerPrefs.GetString(PlayerPrefKey)}\n oyunu tamamladı!";

                string endpointURL = $"https://api.telegram.org/bot{TelegramBotToken}/sendMessage";
                string queryParameters = $"?chat_id={ChatID}&text={UnityWebRequest.EscapeURL(message)}";

                UnityWebRequest webRequest = UnityWebRequest.Get(endpointURL + queryParameters);

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Telegrama mesaj gönderildi");
                }
                else
                {
                    Debug.LogError("Telegrama gönderilirken hata olustu!!!");
                }
            }

            yield return new WaitForSeconds(5f); // mesaj gönderim aralığı
        }
    }
}
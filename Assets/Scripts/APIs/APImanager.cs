using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;


public class APImanager : MonoBehaviour
{
    [Header("Telegram & Firebase API")]
    [HideInInspector]private string TelegramBotToken = "6303462751:AAESnqjmKbmSrx216v-wsciNqrJMOAITmKU";
    [HideInInspector]private string databaseURL = "https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/";
    [HideInInspector]private static string PlayerPrefKey = "PlayerID";
    [HideInInspector]private string ChatID = "-1001941348433";
    public Text PlayerIDText;

    
    [Header("ComboCount")]
    private comboUI comboUI;
    private int comboCount;
    [SerializeField]private int BestComboCount;
    [SerializeField]private int totalHitCount;



    private void Awake()
    {
        InitializePlayerID();
    }

    private void Start() 
    {
        StartCoroutine(UpdateVariableValue());
        StartCoroutine(GetPlayerData());
        comboUI = GetComponent<comboUI>();

    }

    void Update()
    {
        // BestComboCount = comboUI.BestComboCount;
        totalHitCount = comboUI.totalHitCount;
        comboCount = comboUI.comboCount;
        // PlayerID'yi UI'da göster
        if (PlayerIDText != null)
        {
            PlayerIDText.text = "Player ID: " + PlayerPrefs.GetString(PlayerPrefKey);
        }
        
        if(comboCount >= BestComboCount)
        {
            BestComboCount = comboCount;
        }
        StartCoroutine(SavePlayerData());
    }

    

    // PlayerID generate
    private static string GeneratePlayerID()
    {
        string playerID = string.Empty;
        string Ids = "qwertyuopasdfghjklizxcvbnm";
        for (int i = 0; i < 32; i++)
        {
            if (i == 8 || i == 13 || i == 17 || i == 21)
            {
                playerID += "-";
            }else if(i >= 13 && i <= 21)
            {
                playerID += Ids[Random.Range(0, Ids.Length)];
            }else
            {
                playerID += Random.Range(0, 10).ToString();
            }
        }
        return playerID;
    }

    private void InitializePlayerID()
    {
        // ID kontrolu eger ıd yoksa yeni id olusturuyor
        if (!PlayerPrefs.HasKey(PlayerPrefKey))
        {
            string playerID = GeneratePlayerID();
            PlayerPrefs.SetString(PlayerPrefKey, playerID);
            PlayerPrefs.Save();
        }        
    }



    // Telegram API
    private IEnumerator UpdateVariableValue()
    {
        while (true)
        {
            if (BestComboCount == 50)
            {
                // telegrama mesaj gönderme
                string message = $"🎉 MUHTESEM!\n Kullanıcı ID: {PlayerPrefs.GetString(PlayerPrefKey)}\n {BestComboCount} Comboya ulastı!";

                string endpointURL = $"https://api.telegram.org/bot{TelegramBotToken}/sendMessage";
                string queryParameters = $"?chat_id={ChatID}&text={UnityWebRequest.EscapeURL(message)}";
                UnityWebRequest webRequest = UnityWebRequest.Get(endpointURL + queryParameters);

                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Telegrama gönderilen sayac güncellendi: " + BestComboCount);
                }
                else
                {
                    Debug.LogError("Telegrama gönderilen sayacta hata olustu!!!");
                }
            }
            yield return new WaitForSeconds(5f); // mesaj gönderim aralığı
        }
    }



    // Fİrebase Datecenter
    IEnumerator GetPlayerData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(databaseURL + "/" + PlayerPrefs.GetString(PlayerPrefKey) + ".json"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                var response = JsonUtility.FromJson<PlayerData>(jsonResponse);
                BestComboCount = response.BestComboCount;
                totalHitCount = response.TotalHitCount;
            }
        }
    }

    IEnumerator SavePlayerData()
    {
        PlayerData playerData = new PlayerData(BestComboCount, totalHitCount, PlayerPrefs.GetString(PlayerPrefKey));
        string json = JsonUtility.ToJson(playerData);

        using (UnityWebRequest request = UnityWebRequest.Put(databaseURL + "/" + PlayerPrefs.GetString(PlayerPrefKey) + ".json", json))
        {
            request.method = "PUT";
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    /*[System.Serializable] Unity'nin serileştirme sistemi tarafından serileştirilebileceğini belirtmek için bir sınıfa,
     yapıya veya alana uygulanabilen bir C# özniteliğidir. Hangisini yaparak,
     geliştiricinin verileri yerel olarak (veya API aracılığıyla uzaktan) kaydetmesine ve daha sonra yeniden yüklemesine olanak tanır.*/
    [System.Serializable]private class PlayerData
    {
        public int BestComboCount;
        public int TotalHitCount;
        public string ID;

        public PlayerData(int bestComboCount, int totalHitCount, string playerPrefKey)
        {
            BestComboCount = bestComboCount;
            TotalHitCount = totalHitCount;
            ID = playerPrefKey;
        }
    }
    [System.Serializable]private class BestComboData
    {
        public string ID = PlayerPrefs.GetString(PlayerPrefKey);

        public BestComboData(int bestComboCount)
        {
            ID = bestComboCount.ToString();
        }
    }


    
    // Admin Komutları
    /*[MenuItem("Admin/ChangeADMIN")]
    public static void ChangeAdminID()
    {
        if (EditorUtility.DisplayDialog("ID'yi AdminID olarak degistirmek istiyor musun? ",
        "ID: AdminID olarak güncellendi! " +
        "\nKayitli veriler temizlendi!", "Devam Et"))
        Debug.Log("ID: AdminID olarak güncellendi!");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString(PlayerPrefKey, "AdminID");
    }
    [MenuItem("Admin/ChangeRandomID")]
    public static void ChangeRandomID()
    {
        if (EditorUtility.DisplayDialog("ID'yi rastgele degistirmek istiyor musun? ",
        "ID: Yeni rastgele bir ID olacak sekilde güncellendi! " +
        "\nKayitli veriler temizlendi!", "Devam Et"))
        Debug.Log("ID olacak sekilde güncellendi!");
        PlayerPrefs.DeleteAll();
        string playerID = GeneratePlayerID();
        PlayerPrefs.SetString(PlayerPrefKey, playerID);
        PlayerPrefs.Save();
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;

public class comboUI : MonoBehaviour
{
    /* combo yapılamsı istenen fonksiyona comboUI.comboSayac(); kodu eklenecek örnek attack fonksiyonuna eklenebilir.*/
    /* public comboUI comboUI; ve start fonksiyonuna comboUI = GetComponent<comboUI>(); eklenmesi gerekiyor*/
    [SerializeField]private Text comboText;
    [HideInInspector]public int comboCount = 0;
    [SerializeField]private int totalHitCount = 0;
    [SerializeField]private int BestComboCount = 0;
    public float comboTimer = 5f;

    [Header("Telegram API")]
    private string TelegramBotToken = "6303462751:AAEcH6uf4okk3AehI54MhDczPoUCFbwsnQs";
    private static string PlayerPrefKey = "PlayerID";
    private string ChatID = "-1001941348433";
    public Text PlayerIDText;

    private float B_combos;
    private float A_combos;
    private float S_combos;
    private float SS_combos;

    
    public Image B_comboFill;
    public Image B_comboUnfill;

    public Image A_comboFill;
    public Image A_comboUnfill;

    public Image S_comboFill;
    public Image S_comboUnfill;

    public Image SS_comboFill;
    public Image SS_comboUnfill;

    private void Start() {
        B_comboFill.fillAmount = 0;
        A_comboFill.fillAmount = 0;
        S_comboFill.fillAmount = 0;
        SS_comboFill.fillAmount = 0;

        
        B_comboFill.gameObject.SetActive(false);
        B_comboUnfill.gameObject.SetActive(false);
        A_comboFill.gameObject.SetActive(false);
        A_comboUnfill.gameObject.SetActive(false);
        S_comboFill.gameObject.SetActive(false);
        S_comboUnfill.gameObject.SetActive(false);
        SS_comboFill.gameObject.SetActive(false);
        SS_comboUnfill.gameObject.SetActive(false);

        InitializePlayerID();
        StartCoroutine(UpdateVariableValue());
        Load();
    }

    public void Load()
    {
        totalHitCount = int.Parse(PlayerPrefs.GetString("totalHitCount", "0"));
        BestComboCount = int.Parse(PlayerPrefs.GetString("BestComboCount", "0"));
    }

     public void Save()
    {
        PlayerPrefs.SetString("totalHitCount", totalHitCount.ToString());
        PlayerPrefs.SetString("BestComboCount", BestComboCount.ToString());
    }

    private void Update() {
        if(comboCount == 0)
        {
            comboText.gameObject.SetActive(false);
            B_comboFill.gameObject.SetActive(false);
            B_comboUnfill.gameObject.SetActive(false);
            A_comboFill.gameObject.SetActive(false);
            A_comboUnfill.gameObject.SetActive(false);
            S_comboFill.gameObject.SetActive(false);
            S_comboUnfill.gameObject.SetActive(false);
            SS_comboFill.gameObject.SetActive(false);
            SS_comboUnfill.gameObject.SetActive(false);
        }

        if(comboCount >= 1 && comboCount <= 4)
        {
            comboText.color = new Color(0.8f, 0.6f, 0.0f, 1.0f);
            B_comboFill.gameObject.SetActive(true);
            B_comboUnfill.gameObject.SetActive(true);
        }

        if(comboCount >= 5 && comboCount <= 9)
        {
            comboText.color = new Color(1.0f, 0.4f, 0.0f, 1.0f);
            B_comboFill.gameObject.SetActive(false);
            B_comboUnfill.gameObject.SetActive(false);
            A_comboFill.gameObject.SetActive(true);
            A_comboUnfill.gameObject.SetActive(true);
        }
        if(comboCount >= 10 && comboCount <= 24)
        {
            comboText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            A_comboFill.gameObject.SetActive(false);
            A_comboUnfill.gameObject.SetActive(false);
            S_comboFill.gameObject.SetActive(true);
            S_comboUnfill.gameObject.SetActive(true);
        }
        if(comboCount >= 25)
        {
            comboText.color = new Color(0.0f, 1.0f, 0.7f, 1.0f);
            S_comboFill.gameObject.SetActive(false);
            S_comboUnfill.gameObject.SetActive(false);
            SS_comboFill.gameObject.SetActive(true);
            SS_comboUnfill.gameObject.SetActive(true);
        }
        
        B_comboFill.fillAmount = (float)B_combos / 5;
        A_comboFill.fillAmount = (float)A_combos / 5;
        S_comboFill.fillAmount = (float)S_combos / 15;
        SS_comboFill.fillAmount = (float)SS_combos / 25;


        // B_comboFill.fillAmount = (float)comboCount / 5;
        // A_comboFill.fillAmount = (float)comboCount / 10;
        // S_comboFill.fillAmount = (float)comboCount / 25;
        // SS_comboFill.fillAmount = (float)comboCount / 50;
        

        // PlayerID'yi UI'da göster
        if (PlayerIDText != null)
        {
            PlayerIDText.text = "Player ID: " + PlayerPrefs.GetString(PlayerPrefKey);
        }
        Save();
    }


    public void comboSayac()
    {
        totalHitCount++;
        comboCount++;
        if(comboCount >= BestComboCount)
        {
            BestComboCount = comboCount;
        }

        if(comboCount >= 1 && comboCount <= 4)
        {
            B_combos++;
        }
        if(comboCount >= 5 && comboCount <= 9)
        {
            A_combos++;
        }
        if(comboCount >= 10 && comboCount <= 24)
        {
            S_combos++;
        }
        if(comboCount >= 25)
        {
            SS_combos++;
        }

        comboText.text = "x" + comboCount.ToString();
        comboText.gameObject.SetActive(true);
        float rotationAngle = Random.Range(-20f, 20f);
        comboText.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        B_comboFill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        B_comboUnfill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        A_comboFill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        A_comboUnfill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        S_comboFill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        S_comboUnfill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        SS_comboFill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        SS_comboUnfill.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

        CancelInvoke("ComboReset"); // fonksiyon her çağırıldığında combonun sıfırlanması içiçn 5 saniye baştan başalar
        Invoke("ComboReset", comboTimer);
    }

    private void ComboReset()
    {
        comboCount = 0;
        B_combos = 0;
        A_combos = 0;
        S_combos = 0;
        SS_combos = 0;
        comboText.gameObject.SetActive(false);
    }

    //TELEGRAM API   

    private static string GeneratePlayerID()
    {
        // 6 haneli rastgele ID
        string playerID = string.Empty;
        for (int i = 0; i < 6; i++)
        {
            playerID += Random.Range(0, 6).ToString();
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

    private IEnumerator UpdateVariableValue()
    {
        while (true)
        {
            if (BestComboCount > 25)
            {
                // telegrama mesaj gönderme
                string message = $"BestComboCount: {BestComboCount}\nKullanıcı ID: {PlayerPrefs.GetString(PlayerPrefKey)}";

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

            yield return new WaitForSeconds(10f); // mesaj gönderim aralığı
        }
    }


    // Admin Komutları
    [MenuItem("Admin/ChangeID")]
    public static void ChangeAdminID()
    {
        if (EditorUtility.DisplayDialog("ID'yi AdminID olarak degistirmek istiyor musun? ",
        "Bundan Eminmisin geri dönüşü olmayabilir " +
        "Bu işlem geri alınamaz!", "Devam Et"))
        Debug.Log("ID AdminId olarak degistirildi!");
        PlayerPrefs.SetString(PlayerPrefKey, "AdminID");
    }
    [MenuItem("Admin/ChangeRandomID")]
    public static void ChangeRandomID()
    {
        if (EditorUtility.DisplayDialog("ID'yi rastgele degistirmek istiyor musun? ",
        "Bundan Eminmisin geri dönüşü olmayabilir " +
        "Bu işlem geri alınamaz!", "Devam Et"))
        Debug.Log("ID rastgele degistirildi!");
        string playerID = GeneratePlayerID();
        PlayerPrefs.SetString(PlayerPrefKey, playerID);
        PlayerPrefs.Save();

    }
}

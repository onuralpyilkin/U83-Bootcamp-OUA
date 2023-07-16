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
    [HideInInspector]public int totalHitCount = 0;
    [HideInInspector]public int BestComboCount = 0;
    public float comboTimer = 5f;

    [Header("Telegram & Firebase API")]
    public APImanager APImanager;
    [HideInInspector]private string TelegramBotToken = "6303462751:AAESnqjmKbmSrx216v-wsciNqrJMOAITmKU";
    [HideInInspector]private string databaseURL = "https://u83firebase-default-rtdb.europe-west1.firebasedatabase.app/";
    [HideInInspector]private static string PlayerPrefKey = "PlayerID";
    [HideInInspector]private string ChatID = "-1001941348433";

    private float B_combos;
    private float A_combos;
    private float S_combos;
    private float SS_combos;

    
    public Image comboEffectFill;
    public Image SupercomboEffectFill;
    [Header("Combos_B")]
    public Image B_comboFill;
    public Image B_comboUnfill;

    [Header("Combos_A")]
    public Image A_comboFill;
    public Image A_comboUnfill;

    [Header("Combos_S")]
    public Image S_comboFill;
    public Image S_comboUnfill;

    [Header("Combos_SS")]
    public Image SS_comboFill;
    public Image SS_comboUnfill;

    private void Start() {
        B_comboFill.fillAmount = 0;
        A_comboFill.fillAmount = 0;
        S_comboFill.fillAmount = 0;
        SS_comboFill.fillAmount = 0;

        /*comboEffectFill.gameObject.SetActive(false);
        SupercomboEffectFill.gameObject.SetActive(false);*/

        B_comboFill.gameObject.SetActive(false);
        B_comboUnfill.gameObject.SetActive(false);
        A_comboFill.gameObject.SetActive(false);
        A_comboUnfill.gameObject.SetActive(false);
        S_comboFill.gameObject.SetActive(false);
        S_comboUnfill.gameObject.SetActive(false);
        SS_comboFill.gameObject.SetActive(false);
        SS_comboUnfill.gameObject.SetActive(false);

        // PlayerPrefKey = PlayerPrefs.GetString(PlayerPrefKey);
        APImanager = GetComponent<APImanager>();
        PlayerPrefKey = comboUI.PlayerPrefKey;
        Load();
    }

    public void Load()
    {
        totalHitCount = int.Parse(PlayerPrefs.GetString("totalHitCount", "0"));
    }

     public void Save()
    {
        PlayerPrefs.SetString("totalHitCount", totalHitCount.ToString());
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


        // if(comboCount == 5)
        // {
            // comboEffectFill.gameObject.SetActive(true);
            // Invoke("comboEffect", 0.5f);
        // }
        // if(comboCount == 10)
        // {
            // comboEffectFill.gameObject.SetActive(true);
            // Invoke("comboEffect", 0.5f);
        // }
        if(comboCount == 25)
        {
            SupercomboEffectFill.gameObject.SetActive(true);
            Invoke("SupercomboEffect", 1.0f);
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

        /*float comboEffecRotationAngle = Random.Range(0f, -360f);
        comboEffectFill.transform.rotation = Quaternion.Euler(0f, 0f, comboEffecRotationAngle);
        comboEffectFill.gameObject.SetActive(true);
        Invoke("comboEffect", 0.25f);*/
        

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

    private void comboEffect()
    {
        comboEffectFill.gameObject.SetActive(false);
    }
    private void SupercomboEffect()
    {
        SupercomboEffectFill.gameObject.SetActive(false);
    }
}
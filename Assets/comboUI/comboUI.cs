using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboUI : MonoBehaviour
{
    /* combo yapılamsı istenen fonksiyona comboUI.comboSayac(); kodu eklenecek örnek attack fonksiyonuna eklenebilir.*/
    /* public comboUI comboUI; ve start fonksiyonuna comboUI = GetComponent<comboUI>(); eklenmesi gerekiyor*/
    [SerializeField]private Text comboText;
    [HideInInspector]public int comboCount = 0;
    [SerializeField]private int totalComboCount = 0;
    public float comboTimer = 5f;

    
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


        B_comboFill.fillAmount = (float)comboCount / 5;
        A_comboFill.fillAmount = (float)comboCount / 10;
        S_comboFill.fillAmount = (float)comboCount / 25;
        SS_comboFill.fillAmount = (float)comboCount / 50;
    }


    public void comboSayac()
    {
        totalComboCount++;
        comboCount++;
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
        comboText.gameObject.SetActive(false);
    }

    public void Add(float i)
    {
        B_comboFill.fillAmount = (float)comboCount / 5;
        A_comboFill.fillAmount = (float)comboCount / 10;
        S_comboFill.fillAmount = (float)comboCount / 25;
        SS_comboFill.fillAmount = (float)comboCount / 50;
    }

    public void Deduct(float i)
    {
        B_comboFill.fillAmount = (float)comboCount / 5;
        A_comboFill.fillAmount = (float)comboCount / 10;
        S_comboFill.fillAmount = (float)comboCount / 25;
        SS_comboFill.fillAmount = (float)comboCount / 50;
    }
}

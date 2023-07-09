using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QualitySettingsController : MonoBehaviour
{
    public TextMeshProUGUI qualityText;
    public Button increaseButton;
    public Button decreaseButton;

    private int qualityLevel = 2; //High quality level

    void Start()
    {
        SetQualityText();
        increaseButton.onClick.AddListener(ClickQualityUp);
        decreaseButton.onClick.AddListener(ClickQualityDown);

        if (PlayerPrefs.HasKey("QualityLevel")) //Kaydedilen quality ayarlarını geri yükle
        {
            qualityLevel = PlayerPrefs.GetInt("QualityLevel");
            SetQualityText();
        }
    }

    public void ClickQualityUp()
    {
        qualityLevel++;
        if (qualityLevel > 2)
            qualityLevel = 2;

        SetQualityText();
        SaveQualitySettings();
    }

    public void ClickQualityDown()
    {
        qualityLevel--;
        if (qualityLevel < 0)
            qualityLevel = 0;

        SetQualityText();
        SaveQualitySettings();

    }

    public void SetQualityText()
    {
        string qualityString;
        switch (qualityLevel)
        {
            case 0:
                qualityString = "Low";
                break;
            case 1:
                qualityString = "Medium";
                break;
            case 2:
                qualityString = "High";
                break;
            default:
                qualityString = "";
                break;
        }

        QualitySettings.SetQualityLevel(qualityLevel, true);
        qualityText.text = qualityString;
    }

    private void SaveQualitySettings() //Kalite ayarlarınıı kaydet
    {
        PlayerPrefs.SetInt("QualityLevel", qualityLevel);
        PlayerPrefs.Save();
    }

}

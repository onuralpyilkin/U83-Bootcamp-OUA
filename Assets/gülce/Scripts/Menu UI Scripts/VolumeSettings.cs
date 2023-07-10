using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class VolumeSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject settingsMenuFirstButton;


    public Transform targetPosition; // Butonların target pozisyonları
    public float duration = 1f;
    public float delay = 0.5f; // ,butonların arasındaki süre

    //Şekil değiştirme
    private Button button;
    private Vector3 originalScale;

    public float selectedScale = 1.2f; // Buton seçiliykenki scale
    public float animationDuration = 0.2f; // Animasyon süresi


    public AudioSource audioSource;
    public Button increaseButton;
    public Button decreaseButton;
    public TextMeshProUGUI volumeText; // Ses miktarını gösteren metin nesnesi

   

    private void Start()
    {

        increaseButton.onClick.AddListener(IncreaseVolume);
        decreaseButton.onClick.AddListener(DecreaseVolume);

        UpdateVolumeText(); // Başlangıçta ses miktarını güncelle

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Kayma Animasyonu
        MoveButton();

        button = GetComponent<Button>();
        originalScale = transform.localScale;

        button.onClick.AddListener(OnButtonClick);

        if (PlayerPrefs.HasKey("Volume")) //Kaydedeilen ses ayarlarını geri yükle
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            audioSource.volume = savedVolume;
            UpdateVolumeText();
        }

    }

    // Kayma Animasyonu
    private void MoveButton()
    {
        transform.DOMove(targetPosition.position, duration).SetDelay(delay);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Büyüme animasyonunu başlat
         transform.DOScale(selectedScale, animationDuration);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Küçülme animasyonunu başlat
        transform.DOScale(originalScale, animationDuration);
    }

    public void OnPointerEnter()
    {
        // Büyüme animasyonunu başlat
        transform.DOScale(selectedScale, animationDuration);
    }

    public void OnPointerExit()
    {
        // Küçülme animasyonunu başlat
        transform.DOScale(originalScale, animationDuration);
    }

    public void OnButtonClick()
    {
        Debug.Log("Button Clicked!");
    }

    public void IncreaseVolume()
    {
        audioSource.volume += 0.1f;
        audioSource.volume = Mathf.Clamp01(audioSource.volume);
        UpdateVolumeText();
        SaveVolumeSettings();
    }

    public void DecreaseVolume()
    {
        audioSource.volume -= 0.1f;
        audioSource.volume = Mathf.Clamp01(audioSource.volume);
        UpdateVolumeText();
        SaveVolumeSettings();
    }

    private void UpdateVolumeText()
    {
        volumeText.text = " " + Mathf.RoundToInt(audioSource.volume * 100f); //Ses miktarını metin nesnesine ekle
    }

    private void SaveVolumeSettings() //Ses ayarlarını kaydet
    {
        PlayerPrefs.SetFloat("Volume", audioSource.volume);
        PlayerPrefs.Save();
    }

}

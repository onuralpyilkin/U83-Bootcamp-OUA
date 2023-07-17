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
    MenuUIButtons menuUIButtons;

    public GameObject settingsMenuFirstButton;

    public Transform sourcePosition; // Butonların başlangıç pozisyonları
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
        menuUIButtons = FindObjectOfType<MenuUIButtons>();
        increaseButton.onClick.AddListener(IncreaseVolume);
        decreaseButton.onClick.AddListener(DecreaseVolume);

        UpdateVolumeText(); // Başlangıçta ses miktarını güncelle

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
    private void OpeningMoveButton()
    {
        if (targetPosition != null)
            transform.DOMove(targetPosition.position, duration).SetDelay(delay);
    }

    public void ClosingMoveButton()
    {
        if(sourcePosition != null)
            transform.DOMove(sourcePosition.position, duration).SetDelay(delay);
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
        menuUIButtons.OnPointerEnter();
        audioSource.volume += 0.1f;
        audioSource.volume = Mathf.Clamp01(audioSource.volume);
        UpdateVolumeText();
        SaveVolumeSettings();
    }

    public void DecreaseVolume()
    {
        menuUIButtons.OnPointerEnter();
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

    public void OnEnable()
    {
        if(sourcePosition != null)
            transform.position = sourcePosition.position;
        OpeningMoveButton();
        MenuManager.Instance.OnPanelClose.AddListener(ClosingMoveButton);
    }

    public void OnDisable()
    {
        MenuManager.Instance.OnPanelClose.RemoveListener(ClosingMoveButton);
    }

}

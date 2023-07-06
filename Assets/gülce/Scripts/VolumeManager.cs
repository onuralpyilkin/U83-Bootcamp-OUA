using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class VolumeManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform targetPosition; // butonların target pozisyonları
    public float duration = 1f;
    public float delay = 0.5f; // butonların arasındaki süre

    //Şekil değiştirme
    private Button button;
    private Vector3 originalScale;

    public float selectedScale = 1.2f; // buton seçiliykenki scale
    public float animationDuration = 0.2f; // animasyon süresi


    public AudioSource audioSource;
    public Button increaseButton;
    public Button decreaseButton;
    public TextMeshProUGUI volumeText; // Ses miktarını gösteren metin nesnesi

    private void Start()
    {
        increaseButton.onClick.AddListener(IncreaseVolume);
        decreaseButton.onClick.AddListener(DecreaseVolume);

        UpdateVolumeText(); // Başlangıçta ses miktarını güncellemek için

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Kayma Animasyonu
        MoveButton();
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        button.onClick.AddListener(OnButtonClick);
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

    private void OnButtonClick()
    {
        Debug.Log("Button Clicked!");
    }

    private void IncreaseVolume()
    {
        audioSource.volume += 0.1f;
        audioSource.volume = Mathf.Clamp01(audioSource.volume);
        UpdateVolumeText(); 
    }

    private void DecreaseVolume()
    {
        audioSource.volume -= 0.1f;
        audioSource.volume = Mathf.Clamp01(audioSource.volume);
        UpdateVolumeText(); 
    }

    private void UpdateVolumeText()
    {
        volumeText.text = " " + Mathf.RoundToInt(audioSource.volume * 100f); // Ses miktarını metin nesnesine ekle
    }
}

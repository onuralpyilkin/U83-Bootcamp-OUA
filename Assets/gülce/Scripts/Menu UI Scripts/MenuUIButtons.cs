using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuUIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Kayma Animasyonu
    public Transform sourcePosition; // butonların başlangıç pozisyonları
    public Transform targetPosition; // butonların target pozisyonları
    public float duration = 1f;
    public float delay = 0.5f; // butonların arasındaki süre

    //Şekil değiştirme
    private Button button;
    private Vector3 originalScale;

    public float selectedScale = 1.2f; // buton seçiliykenki scale
    public float animationDuration = 0.2f; // animasyon süresi

    //Menu Ses
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        if (button != null)
            button.onClick.AddListener(OnButtonClick);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Kayma Animasyonu
    private void OpeningMoveButton()
    {
        if (targetPosition != null)
            transform.DOMove(targetPosition.position, duration).SetDelay(delay);
    }

    public void ClosingMoveButton()
    {
        if (sourcePosition != null)
            transform.DOMove(sourcePosition.position, duration).SetDelay(delay);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hover sesini çal
        audioSource.clip = hoverSound;
        audioSource.Play();
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
        //Hover sesini çal
        audioSource.clip = hoverSound;
        audioSource.Play();
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
        // Button tıklama sesini çal
        audioSource.clip = clickSound;
        audioSource.Play();
        Debug.Log("Button Clicked!");
    }

    public void OnEnable()
    {
        if(sourcePosition != null)
            transform.position = sourcePosition.position;
        OpeningMoveButton();
        if(MenuManager.Instance != null)
            MenuManager.Instance.OnPanelClose.AddListener(ClosingMoveButton);

        if(PauseMenuController.Instance != null)
            PauseMenuController.Instance.OnPanelClose.AddListener(ClosingMoveButton);
        Debug.Log(gameObject.name + " enabled");
    }

    public void OnDisable()
    {
        if (MenuManager.Instance != null)
            MenuManager.Instance.OnPanelClose.RemoveListener(ClosingMoveButton);

        if (PauseMenuController.Instance != null)
            PauseMenuController.Instance.OnPanelClose.RemoveListener(ClosingMoveButton);
    }
}



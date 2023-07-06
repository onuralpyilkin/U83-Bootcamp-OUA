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
    public Transform targetPosition; // butonların target pozisyonları
    public float duration = 1f;
    public float delay = 0.5f; // butonların arasındaki süre 

    //Şekil değiştirme
    private Button button;
    private Vector3 originalScale;

    //Renk değiştirme
    //private Color normalColor;
    //private Color selectedColor;



    public float selectedScale = 1.2f; // buton seçiliykenki scale
    public float animationDuration = 0.2f; // animasyon süresi



    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Kayma Animasyonu
        MoveButton();

        button = GetComponent<Button>();
        originalScale = transform.localScale;
        //normalColor = button.colors.normalColor;
        //selectedColor = button.colors.highlightedColor;



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

        // Renk değişikliği için DOTween kullanarak geçiş yap
        //var colorBlock = button.colors;
        //colorBlock.normalColor = selectedColor;
        //button.colors = colorBlock;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Küçülme animasyonunu başlat
        transform.DOScale(originalScale, animationDuration);

        // Renk değişikliği için DOTween kullanarak geçiş yap
        //var colorBlock = button.colors;
        //colorBlock.normalColor = normalColor;
        //button.colors = colorBlock;

    }

    private void OnButtonClick()
    {
        Debug.Log("Button Clicked!");
    }

}



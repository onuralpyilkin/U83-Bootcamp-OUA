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
    public Transform targetPosition;
    public float duration = 1f;
    public float delay = 0.5f;

    //Şekil ve renk değiştirme
    // public TextMeshProUGUI itemText;
    private Button button;
    private Vector3 originalScale;

    public Color normalColor;
    public Color selectedColor;

    public float selectedScale = 1.2f;
    public float animationDuration = 0.2f;

    private bool isSelected = false;
    private Vector3 initialScale;



    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Kayma Animasyonu
        MoveButton();

        button = GetComponent<Button>();
        originalScale = transform.localScale;

        // Butonun üzerine gelindiğinde büyüme animasyonunu başlat
        //button.OnPointerEnter.AddListener(OnPointerEnter);

        // Butonun üzerinden çıkıldığında küçülme animasyonunu başlat
        //button.OnPointerExit.AddListener(OnPointerExit);

        button.onClick.AddListener(OnButtonClick);



        //initialScale = transform.localScale;
        //SetItemColor(normalColor);
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




    //public void SetSelected(bool selected)
    //{
    //    isSelected = selected;

    //    if (isSelected) // MenuItem seçildiyse
    //    {
    //        // Büyüme animasyonunu başlat
    //        transform.DOScale(Vector3.one * selectedScale, animationDuration);

    //        // Renk değişikliği için DOTween kullanarak geçiş yap
    //        button.image.DOColor(selectedColor, animationDuration);
    //    }
    //    else // MenuItem seçili değilse
    //    {
    //        // Küçülme animasyonunu başlat
    //        transform.DOScale(Vector3.one, animationDuration);

    //        // Renk değişikliği için DOTween kullanarak geçiş yap
    //        button.image.DOColor(normalColor, animationDuration);
    //    }
    //}





    //private void AnimateSelection(float targetScale)
    //{
    //    if (isSelected)
    //    {
    //        transform.DOScale(initialScale * targetScale, animationDuration);
    //    }
    //    else
    //    {
    //        transform.DOScale(initialScale, animationDuration);
    //    }
    //}
}



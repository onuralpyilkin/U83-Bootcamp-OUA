using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
    public Image dashActiveImage;
    public Image dashNotActiveImage;
    private bool isDashAvailable;

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

    }

    private void Update()
    {
            
        if (playerController.DashAvailable)
        {
            isDashAvailable = true;
            // DashAvailable durumuna göre dashImage'i etkinleştir veya devre dışı bırak
            dashActiveImage.gameObject.SetActive(isDashAvailable);
            dashNotActiveImage.gameObject.SetActive(false);
        }
        else
        {
            isDashAvailable = false;
            dashActiveImage.gameObject.SetActive(false);
            dashNotActiveImage.gameObject.SetActive(true);
        }
       
    }
}

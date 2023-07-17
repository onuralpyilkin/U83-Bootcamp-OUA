using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
    public Image dashActiveImage;
    public Image dashNotActiveImage;
    //private bool isDashAvailable;

    //private PlayerController playerController;

    /*private void Start()
    {
        playerController = PlayerController.Instance;

    }*/

    //Update metodu yerine Player üzerinden çağrı yapılacak şekilde güncellendi
    /*private void Update()
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
       
    }*/

    public void UpdateDashIndicator(bool isDashAvailable)
    {
        if (isDashAvailable)
        {
            // DashAvailable durumuna göre dashImage'i etkinleştir veya devre dışı bırak
            dashActiveImage.gameObject.SetActive(isDashAvailable);
            dashNotActiveImage.gameObject.SetActive(false);
        }
        else
        {
            dashActiveImage.gameObject.SetActive(false);
            dashNotActiveImage.gameObject.SetActive(true);
        }
    }
}

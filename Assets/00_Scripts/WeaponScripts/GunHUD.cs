using UnityEngine;
using TMPro;

public class GunHUD : MonoBehaviour
{
    [SerializeField] private GunData gunDataPistol;
    [SerializeField] private GunData gunDataShotgun;
    [SerializeField] private TMP_Text bulletsText;
    [SerializeField] private TMP_Text magSizeText;

    private int activeGun;

    public void SetActiveGun(int currentGun)
    {
        activeGun = currentGun;
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (activeGun == 0)
        {
            bulletsText.text = gunDataPistol.currentAmmo.ToString();
            magSizeText.text = gunDataPistol.magSize.ToString();
        }
        else if (activeGun == 1)
        {
            bulletsText.text = gunDataShotgun.currentAmmo.ToString();
            magSizeText.text = gunDataShotgun.magSize.ToString();
        }
        else
        {
            Debug.Log("No active gun found");
        }
    }
}

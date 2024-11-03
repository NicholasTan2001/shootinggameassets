using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public TextMeshProUGUI bulletCountText; 
    public GunController gunController;

    void Update()
    {
        if (bulletCountText != null && gunController != null)
        {
            bulletCountText.text = gunController.GetBulletRemaining().ToString();
        }
    }
}


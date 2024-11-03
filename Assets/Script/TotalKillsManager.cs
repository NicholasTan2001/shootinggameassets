using UnityEngine;
using TMPro;

public class TotalKillsManager : MonoBehaviour
{
    public TextMeshProUGUI killsText; 
    public TextMeshProUGUI killsTextSecondary; 

    private int totalKills = 0;

    void Start()
    {
        UpdateKillsText();
    }

    public void IncreaseKills(int amount)
    {
        totalKills += amount;
        UpdateKillsText();
    }

    private void UpdateKillsText()
    {
        if (killsText != null)
        {
            killsText.text = "" + totalKills;
        }

        if (killsTextSecondary != null)
        {
            killsTextSecondary.text = "Total Kills: " + totalKills;
        }
    }
}

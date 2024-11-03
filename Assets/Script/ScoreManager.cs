using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI messageText; 
    public TextMeshProUGUI totalScoreText;
    private int score = 0; 

    void Start()
    {
        UpdateScoreUI(); 
        UpdateMessageUI(); 
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
        totalScoreText.text = "Total Score: " + score.ToString();
    }

    void UpdateMessageUI()
    {
        if (score < 100)
        {
            messageText.text = "Bad ...";
        }
        else if (score >= 100 && score <= 300)
        {
            messageText.text = "Nice !!";
        }
        else
        {
            messageText.text = "Outstanding !!";
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreUI();
        UpdateMessageUI(); 
    }
}

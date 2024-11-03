using UnityEngine;
using TMPro;

public class TimeRemaining : MonoBehaviour
{
    public float totalTime = 180f; 
    private float remainingTime;
    public TMP_Text timerText;
    public GameObject gameOverImage; 

    private bool isGameOver = false; 

    void Start()
    {
        remainingTime = totalTime;
        UpdateTimerText(); 
    }

    void Update()
    {
        if (!isGameOver)
        {
            remainingTime -= Time.deltaTime;

            UpdateTimerText();

            if (remainingTime <= 0f)
            {
                isGameOver = true;
                GameOver();
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        timerText.text = "Game Over";
        
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
        }
        
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }
}
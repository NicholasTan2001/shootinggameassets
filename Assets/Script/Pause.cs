using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            
            PauseGame();
            
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;

    }

}


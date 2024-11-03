using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public GameObject pauseImage; 

     public AudioSource finishSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;

            finishSound.Play();
            Debug.Log("You are safe right now");

            if (pauseImage != null)
            {
                pauseImage.SetActive(true);
            }
        }
    }
}

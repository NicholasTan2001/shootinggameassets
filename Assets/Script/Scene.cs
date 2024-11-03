using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public string sceneName; 

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName); 
    }
}

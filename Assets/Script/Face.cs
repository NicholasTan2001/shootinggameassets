using UnityEngine;

public class Face : MonoBehaviour
{
    public Transform target; 

    void LateUpdate()
    {
        if (Time.timeScale > 0 && target != null)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }
}

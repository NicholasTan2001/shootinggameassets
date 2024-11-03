using UnityEngine;

public class AimUI : MonoBehaviour
{
    public GameObject objectToHide;
    public GameObject objectToUnhide;

    void Start()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(true); 
        }
        if (objectToUnhide != null)
        {
            objectToUnhide.SetActive(false); 
        }
    }

    public void UnhideObject1()
    {
        if (objectToUnhide != null)
        {
            objectToUnhide.SetActive(true); 
        }
        if (objectToHide != null)
        {
            objectToHide.SetActive(false); 
        }
    }

    public void UnhideObject2()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(true); 
        }
        if (objectToUnhide != null)
        {
            objectToUnhide.SetActive(false); 
        }
    }
}

using UnityEngine;

public class HideMouse : MonoBehaviour
{
    private bool cursorVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorVisible = !cursorVisible;

            Cursor.visible = cursorVisible;

            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}

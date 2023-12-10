using UnityEngine;


public abstract class UIMinigame : MonoBehaviour
{
    [SerializeField] protected InputHandler InputHandler;
    public virtual void Hide()
    {
        if (InputManager.GetButtonDown("Pause"))
        {
            InputHandler.MetaKeyDown -= Hide;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameObject.SetActive(false);
        }
    }

    public virtual void Show()
    {
        print("+");
        InputHandler.MetaKeyDown += Hide;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.SetActive(true);
    }
}

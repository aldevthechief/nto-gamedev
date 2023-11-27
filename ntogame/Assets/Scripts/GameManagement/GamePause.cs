using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private bool Paused;

    public bool _Paused => Paused;

    private void Awake()
    {
        InputHandler.OnKeyDown += Pause;
        Pause(false);
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(true);
            InputHandler.MetaKeyDown += UnPause;
        }
    }

    public void UnPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(false);
            InputHandler.MetaKeyDown -= UnPause;
        }
    }

    public void Pause(bool state)
    {
        Paused = state;

        Time.timeScale = (!Paused).GetHashCode();

        MenuPanel.SetActive(Paused);
        Cursor.visible = Paused;

        if (Paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private bool Paused;

    public bool _Paused => Paused;

    private void Awake()
    {
        Pause(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!Paused);
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

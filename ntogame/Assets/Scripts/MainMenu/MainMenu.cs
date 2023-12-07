using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ContinueButton;
    [SerializeField] private AudioClip Music;

    private void Start()
    {
        FindObjectOfType<Music>().SetMusic(Music);

        ContinueButton.SetActive(SaveHandler._Instance._HasContinue);

        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Continue()
    {
        SaveHandler._Instance.LoadMain();
    }

    public void StartNewGame()
    {
        SaveHandler.DeleteMain();
        SaveHandler.DeleteInstance();
        SceneTransitions.instance.CallSceneTrans(1);
    }

    public void OpenSettings()
    {
        HidePanels();
        SettingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        HidePanels();
        CreditsPanel.SetActive(true);
    }

    public void HidePanels()
    {
        CreditsPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

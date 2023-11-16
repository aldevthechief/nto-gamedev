using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject SettingsPanel;

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
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

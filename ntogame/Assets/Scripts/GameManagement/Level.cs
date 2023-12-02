using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void GoMainMenu()
    {
        // SceneManager.LoadScene(0);
        SceneTransitions.instance.CallSceneTrans(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneTransitions.instance.CallSceneTrans(SceneManager.GetActiveScene().buildIndex);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public virtual string GetLevelInfo() => "";

    public virtual void ReadLevelInfo(string info) { }

    public void GoMainMenu()
    {
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
        SaveHandler.DeleteMain();
        SaveHandler.DeleteInstance();
        SceneTransitions.instance.CallSceneTrans(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMain() => SaveHandler._Instance.LoadMain();
    public void FastSave() => SaveHandler._Instance.FastSave();
    public void FastLoad() => SaveHandler._Instance.FastLoad();

    public void SaveMain() => SaveHandler._Instance.MainSave();
}

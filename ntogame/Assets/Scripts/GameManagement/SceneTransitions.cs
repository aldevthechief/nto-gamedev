using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private float transTime = 0.5f;
    private Animator transAnim;

    public static SceneTransitions instance { get; private set; }

    void Awake () 
    {
        if(instance == null) 
        {
            instance = this;
        } 
        else 
        {
            Destroy(gameObject);  
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transAnim = GameObject.FindGameObjectWithTag("SceneTransitions").GetComponent<Animator>();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void CallSceneTrans(int sceneNum)
    {
        StartCoroutine(LoadScene(sceneNum));
    }

    // код старый, так что есть недоработки
    public IEnumerator LoadScene(int sceneNum)
    {
        transAnim.SetTrigger("end");
        yield return new WaitForSeconds(transTime);
        SceneManager.LoadScene(sceneNum);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour 
{
    [SerializeField] private float MinimalY;

    public float _MinimalY => MinimalY;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}

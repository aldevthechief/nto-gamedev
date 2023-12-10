using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorCatcher : MonoBehaviour
{
    [SerializeField] private Text Text;

    private void OnDestroy()
    {
        Application.logMessageReceived -= Catch;
    }

    private void Awake()
    {
        Application.logMessageReceived += Catch;
    }

    public void Catch(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            try
            {
                Text.text += $"{condition}\n{stackTrace}";
            }
            catch (System.Exception error)
            {
                print(error.Message);
                Text.text = $"{condition}\n{stackTrace}";
            }

            StopAllCoroutines();
            StartCoroutine(Clear());
        }
    }

    public IEnumerator Clear()
    {
        yield return new WaitForSecondsRealtime(60);

        Text.text = "";
    }
}

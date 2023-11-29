using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyNameHolder : MonoBehaviour
{
    [SerializeField] private ButtonInfoHolder ButtonInfoHolder;
    [SerializeField] private RectTransform RectTransform;
    [SerializeField] private GameObject Caret;
    [SerializeField] private Text KeyInfo;
    private string Key = "";
    private bool Listening = false;

    public string _Key => Key;

    private void OnDestroy()
    {
        InputHandler handler = FindObjectOfType<InputHandler>();
        if (Listening && handler != null)
        {
            handler.MetaKeyDown -= GetKey;
        }
    }

    private void OnDisable()
    {
        InputHandler handler = FindObjectOfType<InputHandler>();
        if (Listening && handler != null)
        {
            handler.MetaKeyDown -= GetKey;
        }
    }

    public void SetInfo(ButtonInfoHolder holder, string key)
    {
        ButtonInfoHolder = holder;

        Key = key;
        KeyInfo.text = key;
        StartCoroutine(UpdateSize());
    }

    public void ChangeKey()
    {
        if (!Listening)
        {
            Caret.SetActive(true);
            Listening = true;

            FindObjectOfType<InputHandler>().MetaKeyDown += GetKey;
        }
    }

    public void GetKey()
    {
        Listening = false;
        FindObjectOfType<InputHandler>().MetaKeyDown -= GetKey;

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                Key = keyCode.ToString();
                KeyInfo.text = Key;
                Caret.SetActive(false);

                if (isActiveAndEnabled)
                {
                    StartCoroutine(UpdateSize());
                }
                else
                {
                    RectTransform.sizeDelta = new Vector2(30 + KeyInfo.preferredWidth, 0);

                    ButtonInfoHolder.UpdatePositions();
                }
            }
        }
    }

    private IEnumerator UpdateSize()
    {
        yield return new WaitForEndOfFrame();

        RectTransform.sizeDelta = new Vector2(30 + KeyInfo.preferredWidth, 0);

        ButtonInfoHolder.UpdatePositions();
    }
}
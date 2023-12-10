using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueSystem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private GameObject DialogueWindow;
    [SerializeField] private RectTransform Caret;
    [SerializeField] private RectTransform NameHolder;
    [SerializeField] private Image Sprite;
    [SerializeField] private Text Name;
    [SerializeField] private Text Text;
    [SerializeField] private bool Skipable = true;
    private PhraseInfo[] Phrases = new PhraseInfo[0];
    private string CurrentText = "";
    private int PhraseIndex = -1;
    private int SentenceIndex = -1;

    private Coroutine Writing = null;

    public event SimpleVoid OnDialogueEnd = null;

    public bool _Writing => Writing != null;
    public bool _Skipable { get { return Skipable; } set { Skipable = value; } }

    public void Skip()
    {
        if (!Skipable)
        {
            return;
        }

        if (InputManager.GetButtonDown("SkipDialogue"))
        {
            StopAllCoroutines();
            Writing = null;

            CurrentText = "";
            Text.text = "";
            SentenceIndex = 0;

            PhraseIndex++;

            Phrases = new PhraseInfo[0];

            DialogueWindow.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            InputHandler.MetaKeyDown -= Skip;

            if(OnDialogueEnd != null)
            {
                OnDialogueEnd.Invoke();
            }
        }
        else if (InputManager.GetButtonDown("DialogueNext"))
        {
            Next();
        }
    }

    public void StartDialogue(PhraseInfo[] infos)
    {
        StopAllCoroutines();
        Writing = null;

        InputHandler.MetaKeyDown += Skip;

        Phrases = infos;
        PhraseIndex = 0;
        SentenceIndex = 0; 
        
        CurrentText = "";
        Text.text = "";

        DialogueWindow.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateCharacterInfo();
        Next();
    }

    public void Next()
    {
        if (!Skipable)
        {
            return;
        }

        if (Phrases.Length <= 0)
        {
            return;
        }

        if(Writing != null)
        {
            StopCoroutine(Writing);
            Writing = null;

            StartCoroutine(EndSentence());
            return;
        }

        if (SentenceIndex >= Phrases[PhraseIndex].Sentences.Length)
        {
            CurrentText = "";
            Text.text = "";
            SentenceIndex = 0;

            PhraseIndex++;

            if(PhraseIndex >= Phrases.Length)
            {
                Phrases = new PhraseInfo[0]; 
                
                DialogueWindow.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                InputHandler.MetaKeyDown -= Skip;

                if (OnDialogueEnd != null)
                {
                    OnDialogueEnd.Invoke();
                }
                return;
            }

            UpdateCharacterInfo();
            Next();
        }
        else
        {
            CurrentText += CommandsExecute(Phrases[PhraseIndex].Sentences[SentenceIndex]);
            Writing = StartCoroutine(WriteSentence(Phrases[PhraseIndex].Sentences[SentenceIndex], Phrases[PhraseIndex].Speeds[SentenceIndex]));
            SentenceIndex++;
        }
    }

    private void UpdateCharacterInfo()
    {
        if (Phrases.Length <= 0)
        {
            return;
        }

        Name.text = CommandsExecute(Phrases[PhraseIndex].Name);

        if(Name.text == "")
        {
            NameHolder.gameObject.SetActive(false);
        }
        else
        {
            NameHolder.gameObject.SetActive(true);
            StartCoroutine(UpdateNameSize());
        }

        Sprite sprite = Phrases[PhraseIndex].Sprite;

        Sprite.sprite = sprite;
        if (sprite == null)
        {
            Sprite.gameObject.SetActive(false);
        }
        else
        {
            Sprite.gameObject.SetActive(true);
            Sprite.rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height) / (sprite.texture.height) * Screen.height * 0.75f;
            Sprite.rectTransform.anchoredPosition = new Vector2(-80 - Sprite.rectTransform.sizeDelta.x / 2, Sprite.rectTransform.sizeDelta.y / 2 - 250);
            // Sprite.rectTransform.anchoredPosition = new Vector2(-80 - Sprite.rectTransform.sizeDelta.x/2, 40 + Sprite.rectTransform.sizeDelta.y / 2);
        }
    }

    private string CommandsExecute(string text)
    {
        text = text.Replace("/player", SaveHandler._Instance._PlayerName);

        return text;
    }

    private IEnumerator WriteSentence(string sentence, float speed)
    {
        Caret.gameObject.SetActive(false);

        sentence = CommandsExecute(sentence);

        speed = 1 / speed;
        foreach(char letter in sentence)
        {
            Text.text += letter;

            yield return new WaitForSecondsRealtime(speed);
        }

        StartCoroutine(EndSentence());

        Writing = null;
    }

    private IEnumerator UpdateNameSize()
    {
        yield return new WaitForEndOfFrame();

        NameHolder.sizeDelta = new Vector2(200 + Name.preferredWidth, 50);
        NameHolder.anchoredPosition = new Vector2(40 + NameHolder.sizeDelta.x / 2, 360);
    }

    private IEnumerator EndSentence()
    {
        Text.text = CurrentText;

        yield return new WaitForEndOfFrame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Caret.gameObject.SetActive(true);

        UICharInfo charInfo = Text.cachedTextGenerator.characters[Text.cachedTextGenerator.characterCount - 1];

        if (SentenceIndex >= Phrases[PhraseIndex].Sentences.Length)
        {
            Caret.anchoredPosition = charInfo.cursorPos + new Vector2(charInfo.charWidth + 16, -Text.cachedTextGenerator.lines[Text.cachedTextGenerator.lineCount - 1].height);
            Caret.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Caret.anchoredPosition = charInfo.cursorPos + new Vector2(charInfo.charWidth + 16, -Text.cachedTextGenerator.lines[Text.cachedTextGenerator.lineCount - 1].height / 2);
            Caret.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Next();
    }
}

[System.Serializable]
public struct PhraseInfo
{
    public Sprite Sprite;
    public string Name;
    public string[] Sentences;
    public float[] Speeds;

    public PhraseInfo(Sprite sprite, string name, string[] sentences, float[] speeds)
    {
        Sprite = sprite;
        Name = name;
        Sentences = sentences;
        Speeds = speeds;
    }
}

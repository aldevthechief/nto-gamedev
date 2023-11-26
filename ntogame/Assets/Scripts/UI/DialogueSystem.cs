using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueSystem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject DialogueWindow;
    [SerializeField] private RectTransform NameHolder;
    [SerializeField] private Image Sprite;
    [SerializeField] private Text Name;
    [SerializeField] private Text Text;
    private PhraseInfo[] Phrases = new PhraseInfo[0];
    private string CurrentText = "";
    [SerializeField] private int PhraseIndex = -1;
    [SerializeField] private int SentenceIndex = -1;

    private Coroutine Writing = null;

    public void StartDialogue(PhraseInfo[] infos)
    {
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
        if(Phrases.Length <= 0)
        {
            return;
        }

        if(Writing != null)
        {
            StopCoroutine(Writing);
            Writing = null;
            Text.text = CurrentText;
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
                return;
            }

            UpdateCharacterInfo();
            Next();
        }
        else
        {
            CurrentText += Phrases[PhraseIndex].Sentences[SentenceIndex];
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

        Sprite.sprite = Phrases[PhraseIndex].Sprite;
        Name.text = Phrases[PhraseIndex].Name;

        if(Name.text == "")
        {
            NameHolder.gameObject.SetActive(false);
        }
        else
        {
            NameHolder.gameObject.SetActive(true);
            StartCoroutine(UpdateNameSize());
        }

        if(Sprite.sprite == null)
        {
            Sprite.gameObject.SetActive(false);
        }
        else
        {
            Sprite.gameObject.SetActive(true);
            Sprite.rectTransform.sizeDelta = new Vector2(Sprite.sprite.texture.width, Sprite.sprite.texture.height);
            Sprite.rectTransform.anchoredPosition = new Vector2(-80 - Sprite.rectTransform.sizeDelta.x/2, 40 + Sprite.rectTransform.sizeDelta.y / 2);
        }
    }

    //public void StartDialogue(string characterName)
    //{
    //    DialogueWindow.SetActive(true);

    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;

    //    AddPhraseCount();
    //    CharacterNameText.text = characterName;
    //}

    //public void AddPhraseCount()
    //{
    //    if(phraseNumber < Phrases.Length - 1)
    //    {
    //        phraseNumber++;
    //        DialogueText.text = Phrases[phraseNumber];
    //    }
    //    else
    //    {
    //        DialogueWindow.SetActive(false);

    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;

    //        phraseNumber = -1;
    //    }
    //}

    private IEnumerator WriteSentence(string sentence, float speed)
    {
        speed = 1 / speed;
        foreach(char letter in sentence)
        {
            Text.text += letter;
            yield return new WaitForSecondsRealtime(speed);
        }

        Text.text = CurrentText;

        Writing = null;
    }

    private IEnumerator UpdateNameSize()
    {
        yield return new WaitForEndOfFrame();

        NameHolder.sizeDelta = new Vector2(25 + Name.preferredWidth, 50);
        NameHolder.anchoredPosition = new Vector2(40 + NameHolder.sizeDelta.x / 2, 365);
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

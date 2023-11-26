using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private string[] Phrases;
    [SerializeField] private GameObject DialogueWindow;
    [SerializeField] private Text CharacterNameText;
    [SerializeField] private Text DialogueText;
    private int phraseNumber = -1;

    public void StartDialogue(string characterName)
    {
        DialogueWindow.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AddPhraseCount();
        CharacterNameText.text = characterName;
    }

    public void AddPhraseCount()
    {
        if(phraseNumber < Phrases.Length - 1)
        {
            phraseNumber++;
            DialogueText.text = Phrases[phraseNumber];
        }
        else
        {
            DialogueWindow.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            phraseNumber = -1;
        }
    }
}

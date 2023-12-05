using UnityEngine;

public class LevelDialogue : SimpleDialogue
{
    [SerializeField] private DialogueSystem DialogueSystem;

    public void StartDialogue()
    {
        DialogueSystem.StartDialogue(Phrases);
    }
}
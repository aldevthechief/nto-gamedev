using UnityEngine;

public class StartDialogue : SimpleDialogue
{
    [SerializeField] private DialogueSystem DialogueSystem;

    private void Awake()
    {
        DialogueSystem.StartDialogue(Phrases);
    }
}
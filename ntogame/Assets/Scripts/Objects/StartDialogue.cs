using UnityEngine;

public class StartDialogue : SimpleDialogue
{
    [SerializeField] private DialogueSystem DialogueSystem;

    private void Start()
    {
        DialogueSystem.StartDialogue(Phrases);
    }
}
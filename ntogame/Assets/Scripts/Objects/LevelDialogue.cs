using UnityEngine;

public class LevelDialogue : SimpleDialogue
{
    [SerializeField] private DialogueSystem DialogueSystem;
    [SerializeField] private bool Startable = true;

    public bool _Startable { get { return Startable; } set { Startable = value; } }

    public void StartDialogue()
    {
        if (Startable)
        {
            DialogueSystem.StartDialogue(Phrases);
        }
    }
}
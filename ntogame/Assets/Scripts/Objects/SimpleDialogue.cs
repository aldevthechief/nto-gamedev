using UnityEngine;

public interface IDialogue
{
    public PhraseInfo[] GetPhrases();
}

public class SimpleDialogue : MonoBehaviour, IDialogue
{
    [SerializeField] protected PhraseInfo[] Phrases;

    public PhraseInfo[] GetPhrases() => Phrases;
}
using UnityEngine;

public interface IDialogue
{
    public PhraseInfo[] GetPhrases();
}

public class SimpleDialogue : MonoBehaviour, IDialogue
{
    [SerializeField] private PhraseInfo[] Phrases;

    public PhraseInfo[] GetPhrases() => Phrases;
}
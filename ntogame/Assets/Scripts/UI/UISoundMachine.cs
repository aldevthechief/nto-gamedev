using UnityEngine;

public class UISoundMachine : MonoBehaviour
{
    private static UISoundMachine Instance = null;

    [SerializeField] private GameObject AudioSourcePrefab;
    [SerializeField] private AudioClip[] Other;
    [SerializeField] private AudioClip[] Button;

    public enum UISoundType { Other, Button }

    private void Awake()
    {
        Instance = this;
    }

    private void PlaySound(AudioClip clip, float volume, float pitch)
    {
        AudioSource source = Instantiate(AudioSourcePrefab, null).GetComponent<AudioSource>();
        source.GetComponent<LifeTimer>().LifeTime = clip.length;

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
    }

    public static void PlaySound(UISoundType type, int index) => PlaySound(type, 1, 1, index);

    public static void PlaySound(UISoundType type, float volume, float pitch, int index)
    {
        switch (type)
        {
            case UISoundType.Other:
                Instance.PlaySound(Instance.Other[index], volume, pitch);
                break;
            case UISoundType.Button:
                Instance.PlaySound(Instance.Button[index], volume, pitch);
                break;
        }
    }
}
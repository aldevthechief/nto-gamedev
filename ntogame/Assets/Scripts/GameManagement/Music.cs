using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource Source;

    public bool _IsPlaying
    {
        get { return Source.isPlaying; }
        set 
        {
            if (value)
            {
                Source.UnPause();
            }
            else
            {
                Source.Pause();
            }
        }
    }

    private void Awake()
    {
        if (FindObjectsOfType<Settings>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetMusic(AudioClip music, float volume = 1, float pitch = 1)
    {
        Source.clip = music;
        Source.volume = volume;
        Source.pitch = pitch;
        Source.Play();
    }
}
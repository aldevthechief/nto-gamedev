using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using GameData;

class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private ConfigInfo Config;
    [SerializeField] private ConfigInfo CurrentConfig;

    public delegate void SimpleEvent();
    public event SimpleEvent OnConfigChanged = null;

    public int _XResolution
    {
        get { return Config.XResolution; }
        set 
        { 
            if(value < 400)
            {
                value = 400;
            }
            else if(value > 5000)
            {
                value = 5000;
            }

            Config.XResolution = value; 
        }
    }
    public int _YResolution
    {
        get { return Config.YResolution; }
        set
        {
            if (value < 400)
            {
                value = 400;
            }
            else if (value > 3000)
            {
                value = 3000;
            }

            Config.YResolution = value; 
        }
    }
    public int _ScreenMode
    {
        get { return Config.ScreenMode; }
        set 
        {
            Config.ScreenMode = value; 
        }
    }
    public int _Quality
    {
        get { return Config.Quality; }
        set
        {
            Config.Quality = value;
        }
    }
    public int _FPSCap
    {
        get { return Config.FPSCap; }
        set 
        {
            if(value <= 0)
            {
                value = -1;
            }
            else if(value < 24)
            {
                value = 24;
            }

            Config.FPSCap = value; 
        }
    }
    public float _MusicVolume
    {
        get { return Config.MusicsVolume; }
        set 
        {
            value = Mathf.Clamp01(value);
            Config.MusicsVolume = value;

            Mixer.SetFloat("MusicVolume", value == 0 ? -80 : -30 * (1 - value));
        }
    }
    public float _EffectsVolume
    {
        get { return Config.EffectsVolume; }
        set
        {
            value = Mathf.Clamp01(value);
            Config.EffectsVolume = value;

            Mixer.SetFloat("EffectVolume", value == 0 ? -80 : -30 * (1 - value));
        }
    }

    private void Awake()
    {
        if(FindObjectsOfType<Settings>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        string path = Path.Combine(Application.dataPath, "config.txt");
        if (File.Exists(path))
        {
            Config = JsonUtility.FromJson<ConfigInfo>(File.ReadAllText(path));
        }
        else
        {
            Config = new ConfigInfo();
        }

        ApplyNewConfig();
    }

    private void Start()
    {
        if(Mixer != null)
        {
            Mixer.SetFloat("EffectVolume", CurrentConfig.EffectsVolume == 0 ? -80 : -30 * (1 - CurrentConfig.EffectsVolume));
            Mixer.SetFloat("MusicVolume", CurrentConfig.MusicsVolume == 0 ? -80 : -30 * (1 - CurrentConfig.MusicsVolume));
        }
    }

    public void ApplyNewConfig()
    {
        CurrentConfig = Config.CreateClone();

        Screen.SetResolution(CurrentConfig.XResolution, CurrentConfig.YResolution, GetScreenMode(CurrentConfig.ScreenMode));

        switch (CurrentConfig.Quality)
        {
            case 0:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1;
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 1:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier2;
                QualitySettings.SetQualityLevel(3, true);
                break;
            case 2:
                Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3;
                QualitySettings.SetQualityLevel(5, true);
                break;
        }


        if(CurrentConfig.FPSCap < 1)
        {
            CurrentConfig.FPSCap = -1;
        }
        else if (CurrentConfig.FPSCap < 24)
        {
            CurrentConfig.FPSCap = 24;
        }

        Application.targetFrameRate = CurrentConfig.FPSCap;

        if(Mixer != null)
        {
            Mixer.SetFloat("EffectVolume", CurrentConfig.EffectsVolume == 0 ? -80 : -30 * (1 - CurrentConfig.EffectsVolume));
            Mixer.SetFloat("MusicVolume", CurrentConfig.MusicsVolume == 0 ? -80 : -30 * (1 - CurrentConfig.MusicsVolume));
        }

        File.WriteAllText(Path.Combine(Application.dataPath, "config.txt"), JsonUtility.ToJson(CurrentConfig));

        if(OnConfigChanged != null)
        {
            OnConfigChanged.Invoke();
        }
    }

    private FullScreenMode GetScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                return FullScreenMode.Windowed;
            case 1:
                return FullScreenMode.FullScreenWindow;
            case 2:
                return FullScreenMode.ExclusiveFullScreen;
            default:
                return FullScreenMode.Windowed;
        }
    }
}

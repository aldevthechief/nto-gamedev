using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private InputField XResolution;
    [SerializeField] private InputField YResolution;
    [SerializeField] private InputField FPSCap;
    [SerializeField] private Dropdown ScreenMode;
    [SerializeField] private Slider MusicVolume;
    [SerializeField] private Slider EffectVolume;
    private Settings Settings = null;

    private void Start()
    {
        Settings = FindObjectOfType<Settings>();

        Settings.OnConfigChanged += UpdateValues;

        UpdateValues();
    }

    private void OnDestroy()
    {
        if(Settings != null)
        {
            Settings.OnConfigChanged -= UpdateValues;
        }
    }

    public void UpdateValues()
    {
        XResolution.text = Settings._XResolution.ToString();
        YResolution.text = Settings._YResolution.ToString();

        if (Settings._FPSCap <= 0)
        {
            FPSCap.text = "нет";
        }
        else
        {
            FPSCap.text = Settings._FPSCap.ToString();
        }

        ScreenMode.value = Settings._ScreenMode;
        MusicVolume.value = Settings._MusicVolume;
        EffectVolume.value = Settings._EffectsVolume;
    }

    public void SetXResolution(string info)
    {
        Settings._XResolution = StaticTools.StringToInt(info);
        XResolution.text = Settings._XResolution.ToString();
    }

    public void SetYResolution(string info)
    {
        Settings._YResolution = StaticTools.StringToInt(info);
        YResolution.text = Settings._YResolution.ToString();
    }

    public void SetFPSCap(string info)
    {
        Settings._FPSCap = StaticTools.StringToInt(info);

        if(Settings._FPSCap <= 0)
        {
            FPSCap.text = "нет";
        }
        else
        {
            FPSCap.text = Settings._FPSCap.ToString();
        }
    }

    public void SetScreenMode(int index)
    {
        Settings._ScreenMode = index;
        ScreenMode.value = Settings._ScreenMode;
    }

    public void SetMusicVolume(float value)
    {
        Settings._MusicVolume = value;
        MusicVolume.value = Settings._MusicVolume;
    }

    public void SetEffectVolume(float value)
    {
        Settings._EffectsVolume = value;
        EffectVolume.value = Settings._EffectsVolume;
    }

    public void ApplySettings()
    {
        Settings.ApplyNewConfig();
    }
}

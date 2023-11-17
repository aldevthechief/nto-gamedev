
namespace GameData
{
    public class SaveData
    {
        public int CurrentLevel = 1;
    }

    [System.Serializable]
    public class ConfigInfo
    {
        public int XResolution = 1280;
        public int YResolution = 720;

        public int ScreenMode = 0; //0 - окно, 1 - окно без рамок, 2 - полный экран

        public int FPSCap = -1;

        public float MusicsVolume = 0.5f;
        public float EffectsVolume = 0.5f;

        public ConfigInfo CreateClone()
        {
            ConfigInfo clone = new ConfigInfo();

            clone.XResolution = XResolution;
            clone.YResolution = YResolution;
            clone.ScreenMode = ScreenMode;
            clone.FPSCap = FPSCap;
            clone.MusicsVolume = MusicsVolume;
            clone.EffectsVolume = EffectsVolume;
            
            return clone;
        }
    }
}

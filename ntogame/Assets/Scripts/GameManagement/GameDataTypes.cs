
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

    [System.Serializable]
    public class KeyMapData
    {
        public string PlatformHorizontalAxis = "+Keypad6 +L -Keypad4 -J";
        public string PlatformHeightAxis = "+KeypadPlus +Equals -KeypadMinus -Minus";
        public string PlatformVerticalAxis = "+Keypad8 +I -Keypad2 -K";

        public string Horizontal = "+D +RightArrow -A -LeftArrow"; 
        public string Vertical = "+W +UpArrow -S -DownArrow";

        public string Pause = "Escape";
        public string SkipDialogue = "Escape";
        public string DialogueNext = "F Return Space";
        public string Interact = "F Return";
        public string Jump = "Space";
        public string TurnCamLeft = "Q";
        public string TurnCamRight = "E";
    }
}

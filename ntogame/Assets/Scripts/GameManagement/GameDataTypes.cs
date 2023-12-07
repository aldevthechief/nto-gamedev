
namespace GameData
{
    [System.Serializable]
    public class SaveData
    {
        public int CurrentLevel = 0;

        public int Health = 5;
        public float XPosition = 0;
        public float YPosition = 0;
        public float ZPosition = 0;

        public float YRotation = 0;

        public string LevelInfo = "";

        public SaveData GetClone()
        {
            SaveData clone = new SaveData();
            clone.CurrentLevel = CurrentLevel;
            clone.Health = Health;
            clone.XPosition = XPosition;
            clone.YPosition = YPosition;
            clone.ZPosition = ZPosition;
            clone.YRotation = YRotation;
            clone.LevelInfo = LevelInfo;

            return clone;
        }
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

        public string FastLoad = "F9";
        public string FastSave = "F5";
    }
}

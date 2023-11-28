using UnityEngine;
using System.IO;
using GameData;

public class InputManager : MonoBehaviour
{
    private static InputManager Instance = null;
    public static InputManager _Instance => Instance;


    [SerializeField] private KeyMapData KeyMap;
#if UNITY_EDITOR
    [SerializeField] private bool UpdateKeyMap;
#endif

    public KeyMapData _KeyMap
    {
        get { return KeyMap; }
        set
        {
            KeyMap = value;
            UpdateKeys();
            SaveKeyMap();
        }
    }

    private Axis Horizontal = null;
    private Axis Vertical = null;

    private Button Pause = null;
    private Button SkipDialogue = null;
    private Button DialogueNext = null;
    private Button Interact = null;
    private Button Jump = null;
    private Button TurnCamLeft = null;
    private Button TurnCamRight = null;

    public enum ButtonState {Down, Hold, Up}


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        string path = Path.Combine(Application.dataPath, "keyConfig.txt");
        if (File.Exists(path))
        {
            KeyMap = JsonUtility.FromJson<KeyMapData>(File.ReadAllText(path));
        }
        else
        {
            KeyMap = new KeyMapData();
        }

        UpdateKeys();
    }

    private void OnApplicationQuit()
    {
        SaveKeyMap();
        Instance = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (UpdateKeyMap)
        {
            UpdateKeys();
            SaveKeyMap();

            UpdateKeyMap = false;
        }
    }
#endif

    public void SaveKeyMap() => File.WriteAllText(Path.Combine(Application.dataPath, "keyConfig.txt"), JsonUtility.ToJson(KeyMap));

    private void UpdateKeys()
    {
        Horizontal = new Axis(KeyMap.Horizontal);
        Vertical = new Axis(KeyMap.Vertical);

        Pause = new Button(KeyMap.Pause);
        SkipDialogue = new Button(KeyMap.SkipDialogue);
        DialogueNext = new Button(KeyMap.DialogueNext);
        Interact = new Button(KeyMap.Interact);
        TurnCamLeft = new Button(KeyMap.TurnCamLeft);
        TurnCamRight = new Button(KeyMap.TurnCamRight);
        Jump = new Button(KeyMap.Jump);
    }

    public float GetAxis(string axis)
    {
        switch (axis)
        {
            case "Horizontal":
                return Horizontal.GetValue();
            case "Vertical":
                return Vertical.GetValue();
        }

        Debug.LogError($"аксиса <color=white>{axis}</color> не существует");

        return 0;
    }

    private bool GetButtonState(string button, ButtonState state)
    {
        switch (button)
        {
            case "Pause":
                return Pause.CheckState(state);
            case "SkipDialogue":
                return SkipDialogue.CheckState(state);
            case "DialogueNext":
                return DialogueNext.CheckState(state);
            case "Interact":
                return Interact.CheckState(state);
            case "Jump":
                return Jump.CheckState(state);
            case "TurnCamLeft":
                return TurnCamLeft.CheckState(state);
            case "TurnCamRight":
                return TurnCamRight.CheckState(state);
        }

        Debug.LogError($"кнопка <color=white>{button}</color> не найдена");

        return false;
    }

    public bool GetButtonDown(string button) => GetButtonState(button, ButtonState.Down);

    public bool GetButton(string button) => GetButtonState(button, ButtonState.Hold);

    public bool GetButtonUp(string button) => GetButtonState(button, ButtonState.Up);

    public class Axis
    {
        private KeyCode[] PositiveKeys = new KeyCode[0];
        private KeyCode[] NegativeKeys = new KeyCode[0];

        public Axis(string keys)
        {
            KeyCode[][] parsed = ParseKeys(keys);
            PositiveKeys = parsed[0];
            NegativeKeys = parsed[1];
        }

        public float GetValue()
        {
            float value = 0;

            foreach(KeyCode key in PositiveKeys)
            {
                if (Input.GetKey(key))
                {
                    value += 1;
                    break;
                }
            }

            foreach (KeyCode key in NegativeKeys)
            {
                if (Input.GetKey(key))
                {
                    value -= 1;
                    break;
                }
            }

            return value;
        }

        public static KeyCode[][] ParseKeys(string keys)
        {
            KeyCode[] positiveKeys = new KeyCode[0];
            KeyCode[] negativeKeys = new KeyCode[0];

            bool positive = true;
            string key = "";

            foreach (char letter in keys)
            {
                switch (letter)
                {
                    case '+':
                        positive = true;
                        break;
                    case '-':
                        positive = false;
                        break;
                    case ' ':
                        if (positive)
                        {
                            positiveKeys = StaticTools.ExpandMassive(positiveKeys, GetKeyCode(key));

                            key = "";
                        }
                        else
                        {
                            negativeKeys = StaticTools.ExpandMassive(negativeKeys, GetKeyCode(key));

                            key = "";
                            positive = false;
                        }
                        break;
                    default:
                        key += letter;
                        break;
                }
            }

            if (key.Length > 0)
            {
                if (positive)
                {
                    positiveKeys = StaticTools.ExpandMassive(positiveKeys, GetKeyCode(key));
                }
                else
                {
                    negativeKeys = StaticTools.ExpandMassive(negativeKeys, GetKeyCode(key));
                }
            }

            return new KeyCode[][] { positiveKeys, negativeKeys };
        }
    }

    public class Button
    {
        private KeyCode[] Keys = new KeyCode[0];

        public Button(string keys) => Keys = ParseKeys(keys);

        public bool CheckState(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Down:
                    foreach (KeyCode key in Keys)
                    {
                        if (Input.GetKeyDown(key))
                        {
                            return true;
                        }
                    }
                    break;
                case ButtonState.Hold:
                    foreach (KeyCode key in Keys)
                    {
                        if (Input.GetKey(key))
                        {
                            return true;
                        }
                    }
                    break;
                case ButtonState.Up:
                    foreach (KeyCode key in Keys)
                    {
                        if (Input.GetKeyUp(key))
                        {
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public static KeyCode[] ParseKeys(string keysInfo)
        {
            KeyCode[] keys = new KeyCode[0];
            string key = "";

            foreach (char letter in keysInfo)
            {
                if (letter == ' ')
                {
                    keys = StaticTools.ExpandMassive(keys, GetKeyCode(key));
                    key = "";
                }
                else
                {
                    key += letter;
                }
            }

            if (key.Length > 0)
            {
                keys = StaticTools.ExpandMassive(keys, GetKeyCode(key));
            }

            return keys;
        }
    }

    private static KeyCode GetKeyCode(string key)
    {
        foreach(KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
        {
            if(code.ToString().ToLower() == key.ToLower())
            {
                return code;
            }
        }

        Debug.LogError($"клавиша <color=white>{key}</color> недействительна");

        return KeyCode.None;
    }
}
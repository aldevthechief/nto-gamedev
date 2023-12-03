using UnityEngine;
using GameData;

public class KeyMapper : MonoBehaviour
{
    [SerializeField] private ButtonInfoHolder MoveRight;
    [SerializeField] private ButtonInfoHolder MoveLeft;
    [SerializeField] private ButtonInfoHolder MoveUp;
    [SerializeField] private ButtonInfoHolder MoveDown;
    [SerializeField] private ButtonInfoHolder Jump;
    [SerializeField] private ButtonInfoHolder Interact;

    [SerializeField] private ButtonInfoHolder PlatformMoveRight;
    [SerializeField] private ButtonInfoHolder PlatformMoveLeft;
    [SerializeField] private ButtonInfoHolder PlatformMoveForward;
    [SerializeField] private ButtonInfoHolder PlatformMoveBack;
    [SerializeField] private ButtonInfoHolder PlatformMoveUp;
    [SerializeField] private ButtonInfoHolder PlatformMoveDown;

    [SerializeField] private ButtonInfoHolder Pause;
    [SerializeField] private ButtonInfoHolder SkipDialogue;
    [SerializeField] private ButtonInfoHolder DialogueNext;
    [SerializeField] private ButtonInfoHolder TurnCamLeft;
    [SerializeField] private ButtonInfoHolder TurnCamRight;

    private void Start()
    {
        KeyMapData keyMap = InputManager._Instance._KeyMap;

        {
            KeyCode[][] axes = InputManager.Axis.ParseKeys(keyMap.Horizontal);
            MoveRight.SetKeys(axes[0]);
            MoveLeft.SetKeys(axes[1]);

            axes = InputManager.Axis.ParseKeys(keyMap.Vertical);
            MoveUp.SetKeys(axes[0]);
            MoveDown.SetKeys(axes[1]);

            axes = InputManager.Axis.ParseKeys(keyMap.PlatformHorizontalAxis);
            PlatformMoveRight.SetKeys(axes[0]);
            PlatformMoveLeft.SetKeys(axes[1]);

            axes = InputManager.Axis.ParseKeys(keyMap.PlatformHeightAxis);
            PlatformMoveUp.SetKeys(axes[0]);
            PlatformMoveDown.SetKeys(axes[1]);

            axes = InputManager.Axis.ParseKeys(keyMap.PlatformVerticalAxis);
            PlatformMoveForward.SetKeys(axes[0]);
            PlatformMoveBack.SetKeys(axes[1]);
        }

        Pause.SetKeys(InputManager.Button.ParseKeys(keyMap.Pause));
        SkipDialogue.SetKeys(InputManager.Button.ParseKeys(keyMap.SkipDialogue));
        DialogueNext.SetKeys(InputManager.Button.ParseKeys(keyMap.DialogueNext));
        Interact.SetKeys(InputManager.Button.ParseKeys(keyMap.Interact));
        Jump.SetKeys(InputManager.Button.ParseKeys(keyMap.Jump));
        TurnCamLeft.SetKeys(InputManager.Button.ParseKeys(keyMap.TurnCamLeft));
        TurnCamRight.SetKeys(InputManager.Button.ParseKeys(keyMap.TurnCamRight));
    }

    public void ApplyKeys()
    {
        KeyMapData keyMap = new KeyMapData();
        string keys = "";

        foreach(string key in MoveRight._Keys)
        {
            keys += $"+{key} ";
        }
        foreach (string key in MoveLeft._Keys)
        {
            keys += $"-{key} ";
        }
        keyMap.Horizontal = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in MoveUp._Keys)
        {
            keys += $"+{key} ";
        }
        foreach (string key in MoveDown._Keys)
        {
            keys += $"-{key} ";
        }
        keyMap.Vertical = keys.Remove(keys.Length - 1); 
        
        keys = "";
        foreach (string key in Pause._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.Pause = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in SkipDialogue._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.SkipDialogue = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in DialogueNext._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.DialogueNext = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in Interact._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.Interact = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in Jump._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.Jump = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in TurnCamLeft._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.TurnCamLeft = keys.Remove(keys.Length - 1);

        keys = "";
        foreach (string key in TurnCamRight._Keys)
        {
            keys += $"{key} ";
        }
        keyMap.TurnCamRight = keys.Remove(keys.Length - 1);

        InputManager._Instance._KeyMap = keyMap;
    }
}
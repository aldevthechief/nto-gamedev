using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
    [System.Serializable] private class WorldEvent : UnityEvent { }
    [SerializeField] private WorldEvent OnEnter = new WorldEvent();
    [SerializeField] private WorldEvent OnExit = new WorldEvent();
    [SerializeField] private WorldEvent OnDown = new WorldEvent();
    [SerializeField] private WorldEvent OnUp = new WorldEvent();

    private bool MouseCaptured = false;

    private void OnMouseEnter()
    {
        if (!MouseCaptured)
        {
            MouseCaptured = true;
            OnEnter.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (MouseCaptured)
        {
            MouseCaptured = false;
            OnExit.Invoke();
        }
    }

    private void OnMouseDown()
    {
        OnDown.Invoke();
    }

    private void OnMouseUp()
    {
        OnUp.Invoke();
    }
}

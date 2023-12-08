using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IInteractable 
{
    [SerializeField] private UnityEvent Event;
    [SerializeField] private Outline Outline;

    public void SetOutline(bool enabled) => Outline.enabled = enabled;

    public void Interact()
    {
        Event.Invoke();
    }
}
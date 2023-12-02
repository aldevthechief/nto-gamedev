using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    protected class ButtonEvent : UnityEvent { }

    [SerializeField] protected ButtonEvent OnClick;
    [SerializeField] private float ScaleWhilePressed;

    public virtual void OnPointerClick(PointerEventData eventData) 
    { 
        OnClick.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        UISoundMachine.PlaySound(UISoundMachine.UISoundType.Button, 0.65f, Random.Range(0.84f, 1), 0);

        transform.localScale = Vector3.one * ScaleWhilePressed;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

}

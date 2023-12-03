using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorUnderMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Graphic[] Colorful; // то что окрасится дефолтным цветом
    [SerializeField] private Color HighlightColor;
    [SerializeField] private Color DefaultColor;

    private void OnDisable()
    {
        foreach (Graphic colorful in Colorful)
        {
            colorful.color = DefaultColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISoundMachine.PlaySound(UISoundMachine.UISoundType.Other, 0.2f, Random.Range(0.8f, 1.1f), 0);

        foreach (Graphic colorful in Colorful)
        {
            colorful.color = HighlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Graphic colorful in Colorful)
        {
            colorful.color = DefaultColor;
        }
    }
}

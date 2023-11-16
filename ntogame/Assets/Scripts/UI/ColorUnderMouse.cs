using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorUnderMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Graphic[] Colorful; // то что окрасится дефолтным цветом
    [SerializeField] private Color HighlightColor;
    [SerializeField] private Color DefaultColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
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

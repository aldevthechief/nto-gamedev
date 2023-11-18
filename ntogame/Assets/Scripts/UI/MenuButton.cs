using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuButton : CustomButton, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform[] Parts;
    [SerializeField] private Graphic[] Colorful; 
    [SerializeField] private Color HighlightColor;
    [SerializeField] private Color DefaultColor;
    [SerializeField] private float ImagePartSpeed;

    private void OnDisable()
    {
        foreach (Graphic defaultColorful in Colorful)
        {
            defaultColorful.color = DefaultColor;
        }

        transform.localScale = Vector3.one;

        Parts[0].anchoredPosition = new Vector2(-50, 0);
        Parts[1].anchoredPosition = new Vector2(50, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Graphic defaultColorful in Colorful)
        {
            defaultColorful.color = HighlightColor;
        }

        StopAllCoroutines();
        StartCoroutine(MoveImagePart(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Graphic defaultColorful in Colorful)
        {
            defaultColorful.color = DefaultColor;
        }

        transform.localScale = Vector3.one;

        StopAllCoroutines();
        StartCoroutine(MoveImagePart(false));
    }

    private IEnumerator MoveImagePart(bool closeUp) //true - приблизить к телу, false - наоборот
    {
        if (closeUp)
        {
            while (Parts[0].anchoredPosition.x < 0)
            {
                Parts[0].anchoredPosition += Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;
                Parts[1].anchoredPosition -= Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }

            Parts[0].anchoredPosition = new Vector2(0, 0);
            Parts[1].anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            while (Parts[0].anchoredPosition.x + 50 > 0)
            {
                Parts[0].anchoredPosition -= Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;
                Parts[1].anchoredPosition += Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }

            Parts[0].anchoredPosition = new Vector2(-50, 0);
            Parts[1].anchoredPosition = new Vector2(50, 0);
        }
    }
}

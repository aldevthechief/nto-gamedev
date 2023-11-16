using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuButton : CustomButton, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform ImagePart;
    [SerializeField] private Graphic[] Defaults; // то что окрасится дефолтным цветом
    [SerializeField] private Graphic[] Whites; // то что окрасится белым цветом
    [SerializeField] private Color HighlightColor;
    [SerializeField] private Color DefaultColor;
    [SerializeField] private float ImagePartSpeed;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Graphic defaultColorful in Defaults)
        {
            defaultColorful.color = HighlightColor;
        }
        foreach (Graphic white in Whites)
        {
            white.color = HighlightColor;
        }

        StopAllCoroutines();
        StartCoroutine(MoveImagePart(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Graphic defaultColorful in Defaults)
        {
            defaultColorful.color = DefaultColor;
        }
        foreach (Graphic white in Whites)
        {
            white.color = Color.white;
        }

        transform.localScale = Vector3.one;

        StopAllCoroutines();
        StartCoroutine(MoveImagePart(false));
    }

    private IEnumerator MoveImagePart(bool closeUp) //true - приблизить к телу, false - наоборот
    {
        if (closeUp)
        {
            while (ImagePart.anchoredPosition.x < 0)
            {
                ImagePart.anchoredPosition += Vector2.right * ImagePartSpeed * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            ImagePart.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            while (ImagePart.anchoredPosition.x + 15 > 0)
            {
                ImagePart.anchoredPosition -= Vector2.right * ImagePartSpeed * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            ImagePart.anchoredPosition = new Vector2(-15, 0);
        }
    }
}

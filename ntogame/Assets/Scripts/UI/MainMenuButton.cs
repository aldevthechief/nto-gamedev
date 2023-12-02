using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuButton : CustomButton, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform ImagePart;
    [SerializeField] private Graphic[] Defaults; // то что окрасится дефолтным цветом
    [SerializeField] private Graphic[] Whites; // то что окрасится белым цветом
    [SerializeField] private Color HighlightColor;
    [SerializeField] private Color DefaultColor;
    [SerializeField] private float ImagePartSpeed;

    private void OnDisable()
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

        ImagePart.anchoredPosition = new Vector2(-15, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISoundMachine.PlaySound(UISoundMachine.UISoundType.Other, 0.3f, Random.Range(0.6f, 1), 0);

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
                ImagePart.anchoredPosition += Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }

            ImagePart.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            while (ImagePart.anchoredPosition.x + 15 > 0)
            {
                ImagePart.anchoredPosition -= Vector2.right * ImagePartSpeed * Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }

            ImagePart.anchoredPosition = new Vector2(-15, 0);
        }
    }
}

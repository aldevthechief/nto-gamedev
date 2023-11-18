using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private RectTransform Canvas;
    [SerializeField] private MainMenuBackgroundElement[] Elements;
    [SerializeField] private float Multiplier;

    private void Update()
    {
        Vector2 mouseViewPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Vector2 mousePosition = new Vector2(mouseViewPosition.x * Canvas.sizeDelta.x, mouseViewPosition.y * Canvas.sizeDelta.y);
        foreach(MainMenuBackgroundElement element in Elements)
        {
            element.Move(mousePosition, Multiplier);
        }

        //Vector2 direction = new Vector2(mouseViewPosition.x * -480, mouseViewPosition.y * -270) - Transform.anchoredPosition;

        //float magnitude = direction.magnitude;
        //if(magnitude > 10)
        //{
        //    Transform.anchoredPosition += direction / magnitude * (Time.deltaTime * Speed * Mathf.Clamp01(magnitude * 0.005f));
        //}
    }
}

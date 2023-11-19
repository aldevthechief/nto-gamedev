using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private RectTransform Canvas;
    [SerializeField] private MainMenuBackgroundElement[] Elements;
    [SerializeField] private Vector2 MousePosition;
    [SerializeField] private float MouseFollowSpeed;
    [SerializeField] private float Multiplier;

    private void Update()
    {
        Vector2 mouseViewPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float magnitude = (MousePosition - mouseViewPosition).magnitude;
        if (magnitude > 0.01f)
        {
            MousePosition += MouseFollowSpeed * Time.deltaTime * Mathf.Clamp01(magnitude) * (mouseViewPosition - MousePosition) / magnitude ;
        }

        Vector2 mousePosition = new Vector2(Mathf.Clamp01(MousePosition.x) * Canvas.sizeDelta.x, Mathf.Clamp01(MousePosition.y) * Canvas.sizeDelta.y);
        foreach (MainMenuBackgroundElement element in Elements)
        {
            element.Move(mousePosition, Multiplier);
        }
    }
}

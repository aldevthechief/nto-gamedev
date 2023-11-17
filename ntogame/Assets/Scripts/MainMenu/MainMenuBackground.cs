using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private RectTransform Transform;
    [SerializeField] private float Speed;

    private void Update()
    {
        Vector2 mouseViewPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Vector3.one / 2;
        Vector2 direction = new Vector2(mouseViewPosition.x * -480, mouseViewPosition.y * -270) - Transform.anchoredPosition;

        float magnitude = direction.magnitude;
        if(magnitude > 10)
        {
            Transform.anchoredPosition += direction / magnitude * (Time.deltaTime * Speed * Mathf.Clamp01(magnitude * 0.005f));
        }
    }
}

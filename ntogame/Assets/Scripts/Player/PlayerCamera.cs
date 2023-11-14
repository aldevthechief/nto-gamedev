using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player Player;
    [Header("Îפסוע ןנט y rotation = 45")]
    [SerializeField] private Vector3 StartOffset;
    [SerializeField] private float Speed;
    private Vector3 Offset = Vector3.zero;

    private void Start()
    {
        Offset = StartOffset;
        Player.transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    private void Update()
    {
        Vector3 direction = Player.transform.position - transform.position + Offset;
        if (direction.sqrMagnitude > 0.1f)
        {
            transform.position += Time.deltaTime * Speed * Mathf.Min(direction.magnitude, 3) * direction.normalized;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y + 45, 0);
            UpdateOffset();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y - 45, 0);
            UpdateOffset();
        }
    }

    private void UpdateOffset()
    {
        Player.transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        Vector3 newOffset = Vector3.zero;
        newOffset.y = StartOffset.y;

        float radians = transform.localEulerAngles.y * Mathf.Deg2Rad;

        float op = Mathf.Sin(radians);
        newOffset.x = StartOffset.x * (Mathf.Abs(op) < 0.01f ? 0 : Mathf.Sign(op));

        op = Mathf.Cos(radians);
        newOffset.z = StartOffset.z * (Mathf.Abs(op) < 0.01f ? 0 : Mathf.Sign(op));

        Offset = newOffset;
    }
}

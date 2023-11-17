using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Movement PlayerMovement;

    [SerializeField] private Vector3 StartOffset;
    [SerializeField] private float Speed;
    private Vector3 Offset = Vector3.zero;

    [Header("CameraFovTweaking")]
    private Camera cam;
    private float refValue;
    [SerializeField] private float Smooth;
    [SerializeField] private int[] FovValues;

    private void Start()
    {
        Offset = StartOffset;
        Player.transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, FovValues[(int)PlayerMovement.PlayerState], ref refValue, Smooth);

        Vector3 direction = Player.transform.position - transform.position + Offset;
        if(direction.sqrMagnitude > 0.1f)
        {
            transform.position += Time.deltaTime * Speed * Mathf.Min(direction.magnitude, 3) * direction.normalized;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y + 45, 0);
            UpdateOffset();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y - 45, 0);
            UpdateOffset();
        }
    }

    private void UpdateOffset()
    {
        Player.transform.localRotation = Quaternion.Euler(22.5f, transform.localEulerAngles.y, 0);

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

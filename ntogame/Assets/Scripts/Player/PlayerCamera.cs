using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player Player;
    [SerializeField] private Movement PlayerMovement;

    [Header("Offset and rotation")]
    [SerializeField] private Vector3 StartOffset;
    [SerializeField] private Vector3 StartRotOffset;

    [SerializeField] private float Speed;
    [SerializeField] private float RotationAngle;

    private Vector3 Offset = Vector3.zero;
    private Vector3 rotOffset = Vector3.zero;
    private float refAngle = 0;

    [SerializeField] float RotationalSmooth;

    [Header("CameraFovTweaking")]
    private Camera cam;
    private float refValue;
    [SerializeField] private float ViewSmooth;
    [SerializeField] private int[] FovValues;

    private void Start()
    {
        Offset = StartOffset;
        rotOffset = StartRotOffset;
        Player.transform.localRotation = Quaternion.Euler(22.5f, rotOffset.y, 0);
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, FovValues[(int)PlayerMovement.PlayerState], ref refValue, ViewSmooth);

        float delta = Quaternion.Angle(transform.localRotation, Quaternion.Euler(rotOffset));
        if (delta > 1f)
        {
            float t = Mathf.SmoothDampAngle(delta, 0.0f, ref refAngle, RotationalSmooth);
            t = 1.0f - (t / delta);
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotOffset), t);
        }
        else
        {
            if (Input.GetButtonDown("TurnCamLeft"))
            {
                rotOffset = new Vector3(45, transform.localEulerAngles.y + RotationAngle, 0);
                UpdateOffset();
            }
            else if (Input.GetButtonDown("TurnCamRight"))
            {
                rotOffset = new Vector3(45, transform.localEulerAngles.y - RotationAngle, 0);
                UpdateOffset();
            }
        }
    
        Vector3 direction = Player.transform.position - transform.position + Offset;
        float magnitude = direction.magnitude;
        
        if(magnitude > 0.01f)
        {
            transform.position += Time.deltaTime * Speed * Mathf.Min(magnitude, 3) / magnitude * direction;
        }
    }

    private void UpdateOffset()
    {
        Vector3 newOffset = Vector3.zero;
        newOffset.y = StartOffset.y;

        float radians = rotOffset.y * Mathf.Deg2Rad;

        float op = Mathf.Sin(radians);
        newOffset.x = StartOffset.x * (Mathf.Abs(op) < 0.25f ? 0 : Mathf.Sign(op));

        op = Mathf.Cos(radians);
        newOffset.z = StartOffset.z * (Mathf.Abs(op) < 0.25f ? 0 : Math.Sign(op));

        Offset = newOffset;

        Player.transform.localRotation = Quaternion.Euler(22.5f, rotOffset.y, 0);
    }
}
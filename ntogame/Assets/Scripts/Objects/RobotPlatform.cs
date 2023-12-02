using Unity.Collections;
using UnityEngine;

public class RobotPlatform : Platform
{
    [Header("movement properties")]
    private Rigidbody rb;
    [SerializeField] private float Speed;
    private Vector3 move;
    [SerializeField] Transform PlayerPos;

    [Header("other properties")]
    private MeshRenderer mesh;
    [SerializeField] private Material[] Materials;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Movement>())
        {
            canMove = false;
            mesh.material = Materials[1];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Movement>())
        {
            canMove = true;
            mesh.material = Materials[0];
        }
    }

    private void Update()
    {
        if(!canMove)
        {
            rb.isKinematic = true;
            return;
        }
        else
        {
            rb.isKinematic = false;
        }

        float x = InputManager.GetAxis("PlatformHorizontalAxis");
        float y = InputManager.GetAxis("PlatformHeightAxis");
        float z = InputManager.GetAxis("PlatformVerticalAxis");

        move = Vector3.ClampMagnitude(PlayerPos.right * x + transform.up * y + PlayerPos.forward * z, 1);
    }

    void FixedUpdate()
    {
        if(canMove)
            rb.velocity = move * Speed;
            // rb.MovePosition(rb.position + move * Time.fixedDeltaTime * Speed);
    }
}
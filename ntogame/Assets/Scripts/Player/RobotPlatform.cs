using UnityEngine;

public class RobotPlatform : MonoBehaviour
{
    [SerializeField] private Material[] Materials;
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private Transform Player;
    [SerializeField] private float Speed;
    private bool CanMove = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            CanMove = false;
            Renderer.material = Materials[1];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            CanMove = true;
            Renderer.material = Materials[0];
        }
    }

    private void Update()
    {
        if (!CanMove)
        {
            return;
        }

        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.Keypad6))
        {
            direction += Player.right;
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            direction -= Player.right;
        }

        if (Input.GetKey(KeyCode.Keypad8))
        {
            direction += Player.forward;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            direction -= Player.forward;
        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            direction += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            direction -= Vector3.up;
        }

        transform.position += direction.normalized * Time.deltaTime * Speed;
    }
}
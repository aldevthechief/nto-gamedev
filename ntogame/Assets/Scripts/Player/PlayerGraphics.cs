using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private Player Player;
    [SerializeField] private Transform Sprite;
    [SerializeField] private Animator CameraTiltAnimator;

    void Update()
    {
        CameraTiltAnimator.SetFloat("horizontalValue", InputHandler._InputAllowed ? Input.GetAxis("HorizontalTilt") : 0);
        
        // rotate the player according to the camera direction
        // Vector3 direction = Player._Camera.transform.position - transform.position;
        // float y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // Sprite.rotation = Quaternion.Euler(0, y, 0);
    }
}

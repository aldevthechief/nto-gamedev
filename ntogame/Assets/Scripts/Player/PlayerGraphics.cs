using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Transform Sprite;
    [SerializeField] private Animator Animator;

    private void Update()
    {
        // Vector3 direction = Player._Camera.transform.position - transform.position;
        // float y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // Sprite.rotation = Quaternion.Euler(0, y, 0);
        
    }
}

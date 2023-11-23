using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private LayerMask LayerMask;

    private void Update()
    {
        RaycastHit raycast;
        if(Physics.Raycast(Player.position, Vector3.down, out raycast, 10, LayerMask))
        {
            transform.position = raycast.point;
        }
        else
        {
            transform.position = new Vector3(0, -100, 0);
        }
    }
}

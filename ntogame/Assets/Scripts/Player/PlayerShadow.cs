using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private float Smooth;
    private Vector3 refScale = Vector3.zero;
    private Vector3 normalScale;

    void Start()
    {
        normalScale = transform.localScale;
    }

    private void Update()
    {
        RaycastHit raycast;
        if(Physics.Raycast(Player.position, Vector3.down, out raycast, 10f, LayerMask)) //я сделал 1.5f для плавности, так как при больших значениях при запрыгивании на объекты происходит слишком резкое перемещение тени
        {
            transform.position = raycast.point;
            transform.localScale = Vector3.SmoothDamp(transform.localScale, normalScale, ref refScale, Smooth);
        }
        else
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref refScale, Smooth);
        }
    }
}

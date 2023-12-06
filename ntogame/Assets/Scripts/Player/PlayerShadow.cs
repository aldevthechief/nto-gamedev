using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform ShadowCast;
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
        if(GameManager.IsDead)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref refScale, Smooth);
        }
        else
        {
            RaycastHit raycast;
            if(Physics.Raycast(ShadowCast.position, Vector3.down, out raycast, 9.5f, LayerMask))
            {
                transform.position = raycast.point;
                transform.rotation = Quaternion.FromToRotation(transform.up, raycast.normal) * transform.rotation;
                transform.localScale = Vector3.SmoothDamp(transform.localScale, normalScale, ref refScale, Smooth);
            }
            else
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref refScale, Smooth);
            }
        }
    }
}

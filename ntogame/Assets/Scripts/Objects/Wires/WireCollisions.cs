using System.Collections;
using UnityEngine;

public class WireCollisions : MonoBehaviour
{
    private bool allowCollision = false;
    [SerializeField] private Material EnergyFlowMaterial;

    void OnTriggerEnter(Collider other)
    {
        if(allowCollision)
        {
            PlayerTrigger player = other.gameObject.GetComponent<PlayerTrigger>();
            if(player != null)
                player.WireColl(other.ClosestPoint(transform.position));
        }
    }

    public IEnumerator BakeCollisions()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        lr.sharedMaterial = EnergyFlowMaterial;
 
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        yield return new WaitForSeconds(0.25f);
        allowCollision = true;
    }
}

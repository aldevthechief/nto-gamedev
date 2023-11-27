using UnityEngine;

public class WireCollisions : MonoBehaviour
{
    public void BakeCollisions()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
 
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }
}

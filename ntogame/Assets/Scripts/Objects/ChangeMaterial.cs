using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private Material FirstMaterial;
    [SerializeField] private Material SecondMaterial;
    [SerializeField] private int MaterialIndex;

    public void SetMaterial(bool second)
    {
        if (second)
        {
            Renderer.materials[MaterialIndex] = SecondMaterial;
        }
        else
        {
            Renderer.materials[MaterialIndex] = FirstMaterial;
        }
    }

}

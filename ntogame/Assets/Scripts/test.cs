using UnityEngine;

public class test : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private Material[] Materials;

    public void Indicate(bool state)
    {
        Renderer.material = Materials[StaticTools.BooltoInt(state)];
    }

    public void Interact()
    {
        print(gameObject);
    }
}

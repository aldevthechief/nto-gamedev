using UnityEngine;

public class Platform : MonoBehaviour
{
    public enum MaterialType { None = -1, Beton, Dirt, Water, Metal, Wood}

    [SerializeField] private MaterialType Material;

    public MaterialType _Material => Material;
}

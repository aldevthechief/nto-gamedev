using UnityEngine;

public class MenuPausedEffect : MonoBehaviour
{
    [SerializeField] private Material Material;

    private void Update()
    {
        Material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
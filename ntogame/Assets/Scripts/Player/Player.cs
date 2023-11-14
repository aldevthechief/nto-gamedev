using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCamera Camera;
    [SerializeField] private Level Level;

    public PlayerCamera _Camera => Camera;
    public Level _Level => Level;
}

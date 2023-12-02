using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private Animator CameraTiltAnimator;

    void Update()
    {
        if(!GameManager.InputAllowed)
            return;
            
        CameraTiltAnimator.SetFloat("horizontalValue", InputHandler._InputAllowed ? Input.GetAxis("HorizontalTilt") : 0);
    }
}

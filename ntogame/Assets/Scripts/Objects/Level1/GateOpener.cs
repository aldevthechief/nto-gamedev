using UnityEngine;

namespace Level1
{
    public class GateOpener : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator Gate;
        [SerializeField] private Outline Outline;
        [SerializeField] private bool Interactable;

        public bool _IsOpen => Gate.GetBool("isOpen");
        public bool _Interactable { get { return Interactable; } set{ Interactable = value; } }

        public void SetOutline(bool enabled)
        {
            if (!Interactable)
            {
                Outline.enabled = false;
            }
            else
            {
                Outline.enabled = enabled;
            }
        }

        public void Interact()
        {
            if (Interactable)
            {
                Gate.SetBool("isOpen", !Gate.GetBool("isOpen"));
            }
        }
    }
}
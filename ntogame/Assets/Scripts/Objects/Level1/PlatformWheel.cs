using UnityEngine;

namespace Level1
{
    public class PlatformWheel : MonoBehaviour, IInteractable
    {
        [SerializeField] private Outline Outline;
        [SerializeField] private Animator Animator;
        [SerializeField] private LevelDialogue FirstDialogue;
        [SerializeField] private LevelDialogue SecondDialogue;
        [SerializeField] private bool Broken;

        public void SetOutline(bool enabled)
        {
            Outline.enabled = enabled;
        }

        public void Interact()
        {
            if (Broken)
            {
                SecondDialogue.StartDialogue();
                return;
            }

            Broken = true;
            Animator.SetBool("Downed", false);
            FirstDialogue.StartDialogue();
        }

        public void Down()
        {
            Animator.SetBool("Downed", true);
        }
    }
}

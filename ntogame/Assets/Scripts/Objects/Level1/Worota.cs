using UnityEngine;

namespace Level1
{
    public class Worota : MonoBehaviour, IInteractable
    {
        [SerializeField] private LevelDialogue FirstDialogue;
        [SerializeField] private LevelDialogue SecondDialogue;
        [SerializeField] private GateOpener GateOpener;
        [SerializeField] private Outline Outline;

        public void SetOutline(bool enabled)
        {
            Outline.enabled = enabled;
        }

        public void Interact()
        {
            if (GateOpener._IsOpen)
            {
                SecondDialogue.StartDialogue();
            }
            else
            {
                FirstDialogue.StartDialogue();
            }
        }
    }
}

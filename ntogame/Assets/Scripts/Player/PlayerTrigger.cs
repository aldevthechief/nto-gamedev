using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private IPlayerInteractable[] Interactables = new IPlayerInteractable[0];

    private void OnTriggerEnter(Collider other)
    {
        IPlayerInteractable interactable = other.GetComponent<IPlayerInteractable>();
        if (interactable != null)
        {
            foreach(IPlayerInteractable _interactable in Interactables)
            {
                _interactable.Indicate(false);
            }

            Interactables = StaticTools.ExpandMassive(Interactables, interactable, 0);
            interactable.Indicate(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IPlayerInteractable interactable = other.GetComponent<IPlayerInteractable>();
        if (interactable != null)
        {
            int index = StaticTools.IndexOf(Interactables, interactable);

            if(index < 0)
            {
                return;
            }

            if(index == 0)
            {
                Interactables[0].Indicate(false);
            }

            Interactables = StaticTools.ReduceMassive(Interactables, index);

            if(Interactables.Length > 0)
            {
                Interactables[0].Indicate(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Interactables.Length > 0)
        {
            Interactables[0].Interact();
        }
    }
}

using UnityEngine;
using EZCameraShake;

public class PlayerTrigger : MonoBehaviour
{
    private IPlayerInteractable[] Interactables = new IPlayerInteractable[0];

    [Header("camera shake properties")]
    [SerializeField] float Magnitude = 4f;
    [SerializeField] float Roughness = 4f;
    [SerializeField] float FadeInTime = 0.1f;
    [SerializeField] float FadeOutTime = 0.5f;

    [Header("collision particles")]
    [SerializeField] GameObject KeyParticles;

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

        if(other.CompareTag("Key"))
        {
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, FadeInTime, FadeOutTime);
            Instantiate(KeyParticles, other.transform.position, other.transform.rotation);
            GameManager.KeyCount++;
            Destroy(other.gameObject);
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

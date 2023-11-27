using UnityEngine;
using EZCameraShake;
using UnityEditor.Callbacks;

public class PlayerTrigger : MonoBehaviour
{
    private IPlayerInteractable[] Interactables = new IPlayerInteractable[0];

    [Header("references")]
    [SerializeField] WireBlock WiringSystem;
    [SerializeField] DialogueSystem Dialogue;
    private Movement playerMovement;

    [Header("camera shake properties")]
    [SerializeField] float Magnitude = 4f;
    [SerializeField] float Roughness = 4f;
    [SerializeField] float FadeInTime = 0.1f;
    [SerializeField] float FadeOutTime = 0.5f;

    [Header("collision particles")]
    [SerializeField] GameObject FluidParticles;
    [SerializeField] GameObject ElectricParticles;

    [Header("interaction properties")]
    private bool allowInteraction = false;
    [SerializeField] float PushForce;

    void Start()
    {
        playerMovement = GetComponent<Movement>();
    }

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
            GameObject fp = SetParticlesMaterial(other.gameObject, FluidParticles);
            Instantiate(fp, other.transform.position, other.transform.rotation);
            GameManager.KeyCount++;
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Wire"))
        {
            CameraShaker.Instance.ShakeOnce(Magnitude * 1.5f, Roughness * 1.5f, FadeInTime, FadeOutTime);
            playerMovement.AddForceToThePlayer(-playerMovement.rb.velocity.normalized * PushForce);
            GameObject blood = SetParticlesMaterial(gameObject, FluidParticles);
            Instantiate(blood, transform.position, transform.rotation);
            Instantiate(ElectricParticles, other.ClosestPoint(transform.position), transform.rotation);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("WirePillar"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null)
            {
                outline.enabled = true;
                if(allowInteraction && !WiringSystem.isUsing)
                    WiringSystem.StartWiring(transform, other.transform);
                else if(allowInteraction)
                    WiringSystem.StopWiring(other.transform);

                allowInteraction = false;
            }
        }
        else if (other.CompareTag("NPC"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
                if (allowInteraction)
                    Dialogue.StartDialogue(other.GetComponentInParent<IDialogue>().GetPhrases());
                allowInteraction = false;
            }
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

        if(other.CompareTag("WirePillar"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null)
            {
                outline.enabled = false;
            }
        }

        if(other.CompareTag("NPC"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && Interactables.Length > 0)
        {
            Interactables[0].Interact();
        }

        if(Input.GetButtonDown("Interact"))
            allowInteraction = true;
    }

    GameObject SetParticlesMaterial(GameObject thingCollidedWith, GameObject desiredParticles)
    {
        GameObject resultParticles = desiredParticles;
        ParticleSystemRenderer resultRenderer = resultParticles.GetComponent<ParticleSystemRenderer>();
        MeshRenderer mesh = thingCollidedWith.GetComponent<MeshRenderer>();
        if(mesh != null)
        {
            resultRenderer.sharedMaterial = mesh.material;
            resultRenderer.trailMaterial = mesh.material;
        }
        else
        {
            LineRenderer lr = thingCollidedWith.GetComponent<LineRenderer>();
            if(lr != null)
            {
                resultRenderer.sharedMaterial = lr.material;
                resultRenderer.trailMaterial = lr.material;
            }
        }
        return resultParticles;
    }
}
using UnityEngine;
using EZCameraShake;
using UnityEditor.Callbacks;

public class PlayerTrigger : MonoBehaviour
{
  //  private IPlayerInteractable[] Interactables = new IPlayerInteractable[0]; всеравно не используется

    [Header("references")]
    [SerializeField] WireBlock WiringSystem;
    [SerializeField] DialogueSystem Dialogue;
    [SerializeField] private InputHandler InputHandler;
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

        InputHandler.OnKeyHold += UpdateInteraction;
    }

    public void UpdateInteraction()
    {
        if (InputManager.GetButtonDown("Interact"))
            allowInteraction = true;
    }

    private void OnTriggerEnter(Collider other)
    {
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
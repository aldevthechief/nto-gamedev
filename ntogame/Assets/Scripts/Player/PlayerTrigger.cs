using UnityEngine;
using EZCameraShake;
using UnityEditor.Callbacks;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
  //  private IPlayerInteractable[] Interactables = new IPlayerInteractable[0]; �������� �� ������������

    [Header("references")]
    [SerializeField] WireBlock WiringSystem;
    [SerializeField] DialogueSystem Dialogue;
    [SerializeField] private InputHandler InputHandler;
    [HideInInspector] public Movement playerMovement;

    [SerializeField] private Transform WireGrabPos;

    [Header("camera shake properties")]
    public float Magnitude = 4f;
    public float Roughness = 4f;
    public float FadeInTime = 0.1f;
    public float FadeOutTime = 0.5f;

    [Header("collision particles")]
    public GameObject FluidParticles;
    public GameObject ElectricParticles;

    [Header("interaction properties")]
    private bool allowInteraction = false;
    public float PushForce;

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

    // воркэраунд для колизии обмотанных проводов, так как OnCollisionEnter почему-то не работает :\
    public void WireColl(Vector3 particlesPos)
    {
        CameraShaker.Instance.ShakeOnce(Magnitude * 1.25f, Roughness * 1.25f, FadeInTime, FadeOutTime);
        playerMovement.AddForceToThePlayer(-new Vector3(playerMovement.rb.velocity.normalized.x, 0, playerMovement.rb.velocity.normalized.z) * PushForce);
        GameObject blood = SetParticlesMaterial(gameObject, FluidParticles);
        Instantiate(blood, transform.position, transform.rotation);
        Instantiate(ElectricParticles, particlesPos, transform.rotation);
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

        // if(other.CompareTag("Wire"))
        // {
        //     CameraShaker.Instance.ShakeOnce(Magnitude * 1.25f, Roughness * 1.25f, FadeInTime, FadeOutTime);
        //     playerMovement.AddForceToThePlayer(-new Vector3(playerMovement.rb.velocity.normalized.x, 0, playerMovement.rb.velocity.normalized.z) * PushForce);
        //     GameObject blood = SetParticlesMaterial(gameObject, FluidParticles);
        //     Instantiate(blood, transform.position, transform.rotation);
        //     Instantiate(ElectricParticles, other.ClosestPoint(transform.position), transform.rotation);
        // }
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
                    WiringSystem.StartWiring(WireGrabPos, other.transform);
                else if(allowInteraction && other.transform != WiringSystem.wireStartTransform)
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
        else if(other.CompareTag("WireDispenser"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null)
            {
                outline.enabled = true;
                if(allowInteraction && !WiringSystem.isUsing)
                    WiringSystem.StartWiring(WireGrabPos, other.transform);
                else if(allowInteraction && other.transform != WiringSystem.wireStartTransform)
                    WiringSystem.StopWiring(other.transform);

                allowInteraction = false;
            }

            Animator dispanim = other.GetComponentInParent<Animator>();
            if(dispanim != null)
            {
                dispanim.SetBool("PlayerIsNear", true);
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

        if(other.CompareTag("WireDispenser"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null)
            {
                outline.enabled = false;
            }

            Animator dispanim = other.GetComponentInParent<Animator>();
            if(dispanim != null)
            {
                dispanim.SetBool("PlayerIsNear", false);
            }
        }
    }

    public GameObject SetParticlesMaterial(GameObject thingCollidedWith, GameObject desiredParticles)
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
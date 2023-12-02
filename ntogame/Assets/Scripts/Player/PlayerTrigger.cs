using UnityEngine;
using EZCameraShake;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    [Header("references")]
    [SerializeField] WireBlock WiringSystem;
    [SerializeField] DialogueSystem Dialogue;
    [SerializeField] private InputHandler InputHandler;
    [HideInInspector] public Movement playerMovement;
    [SerializeField] private Transform WireGrabPos;
    [SerializeField] private PlayerCamera CameraMovement;
    [SerializeField] private Animator DeathAnim;
    [SerializeField] private GameObject HealthBar;

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
    private Transform lastPillar;

    void Start()
    {
        playerMovement = GetComponent<Movement>();
        InputHandler.OnKeyHold += UpdateInteraction;
        GameManager.ResetVariables();
    }

    public void UpdateInteraction()
    {
        if (InputManager.GetButtonDown("Interact"))
            allowInteraction = true;
    }

    // воркэраунд для колизии обмотанных проводов, так как OnCollisionEnter почему-то не работает :\
    public void WireColl(Vector3 particlesPos)
    {
        if(GameManager.IsDead)
            return;

        CameraShaker.Instance.ShakeOnce(Magnitude * 1.25f, Roughness * 1.25f, FadeInTime, FadeOutTime);
        GameObject blood = SetParticlesMaterial(gameObject, FluidParticles);
        Instantiate(blood, transform.position, transform.rotation);
        Instantiate(ElectricParticles, particlesPos, transform.rotation);
        GameManager.Health--;

        if(GameManager.Health <= 0)
            StartCoroutine(PlayerDeath());
        else  
            playerMovement.AddForceToThePlayer(-new Vector3(playerMovement.rb.velocity.normalized.x, 0, playerMovement.rb.velocity.normalized.z) * PushForce);
    }

    IEnumerator PlayerDeath()
    {        
        GameManager.IsDead = true;
        DeathAnim.CrossFade("Death", 0.05f, 0);
        Vector3 lastFlight = -new Vector3(playerMovement.rb.velocity.normalized.x, 0, playerMovement.rb.velocity.normalized.z);
        playerMovement.rb.velocity = Vector3.zero;
        playerMovement.AddForceToThePlayer(lastFlight * PushForce / 3);
        CameraMovement.enabled = false;
        GameManager.InputAllowed = false;
        HealthBar.SetActive(false);
        yield return new WaitForSeconds(2f);
        SceneTransitions.instance.CallSceneTrans(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Key"))
        {
            // DeathAnim.enabled = false;
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, FadeInTime, FadeOutTime);
            GameObject fp = SetParticlesMaterial(other.gameObject, FluidParticles);
            Instantiate(fp, other.transform.position, other.transform.rotation);
            GameManager.KeyObtained = true;
            playerMovement.rb.velocity = Vector3.zero;
            GameManager.InputAllowed = false;
            HealthBar.SetActive(false);
            Invoke("LoadNextScene", 1.5f);
            Destroy(other.gameObject);
        }
    }

    void LoadNextScene()
    {
        // change argument when new scene is created
        SceneTransitions.instance.CallSceneTrans(0);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("WirePillar"))
        {
            Outline outline = other.GetComponentInParent<Outline>();
            if(outline != null && !WiringSystem.isUsing && lastPillar == other.transform || outline != null && other.transform != WiringSystem.wireStartTransform && WiringSystem.isUsing)
                outline.enabled = true;

            if(allowInteraction && !WiringSystem.isUsing && lastPillar == other.transform)
                WiringSystem.StartWiring(WireGrabPos, other.transform);
            else if(allowInteraction && other.transform != WiringSystem.wireStartTransform && WiringSystem.isUsing)
            {
                lastPillar = other.transform;
                WiringSystem.StopWiring(lastPillar);
            }

            allowInteraction = false;
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
            if(outline != null && !WiringSystem.isUsing)
            {
                outline.enabled = true;
                if(allowInteraction)
                {
                    DestroyOtherWires();
                    WiringSystem.StartWiring(WireGrabPos, other.transform);
                }
                // else if(allowInteraction && other.transform != WiringSystem.wireStartTransform)
                //     WiringSystem.StopWiring(other.transform);

                allowInteraction = false;
            }

            Animator dispanim = other.GetComponentInParent<Animator>();
            if(dispanim != null)
            {
                dispanim.SetBool("PlayerIsNear", true);
            }
        }
    }

    void DestroyOtherWires()
    {
        GameObject[] wires = GameObject.FindGameObjectsWithTag("Wire");
        foreach(GameObject w in wires)
        {
            Destroy(w);
        }
        
        GameObject[] sceneconnectibles = GameObject.FindGameObjectsWithTag("WirePillar");
        foreach(GameObject x in sceneconnectibles)
        {
            WirePillar wp = x.GetComponent<WirePillar>();
            if(wp != null)
                wp.isConnected = false;
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
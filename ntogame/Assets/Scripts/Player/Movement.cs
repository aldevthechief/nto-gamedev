using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("moving and jumping properties")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private float velocityMagnitude;

    private bool isGrounded;
    public Transform gc;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    public float groundvelmult;
    public float airmult;
    private float velocitymult;
    private Vector3 velocityChange;

    public float jumpheight;
    public float jumpgrace;
    private float? lastgrounded;
    private float? jumppress;

    [Header("current movement state")]
    public MovementState PlayerState;
    public enum MovementState
    {
        grounded,
        air
    }

    private bool isLanding = true;
    private float landedLastTime;

    [Header("animations")]
    [SerializeField] private Animator SpriteAnim;

    [Header("particles")]
    [SerializeField] GameObject PlayerTrail;
    [SerializeField] GameObject JumpParticles;
    [SerializeField] GameObject LandParticles;
    [SerializeField] float TrailTime;

    void Start()
    {
        StartCoroutine(InstantiateTrail());
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(gc.position, groundDistance, groundMask); 

        if(isGrounded)
        {
            PlayerState = MovementState.grounded;
            velocitymult = groundvelmult;
            lastgrounded = Time.time;
        }
        else
        {
            PlayerState = MovementState.air;
            velocitymult = groundvelmult * airmult;
            if(Time.time - landedLastTime > 0.5f)
                isLanding = true;
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        float inputmagnitude = new Vector2(x, z).magnitude;
        velocityMagnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        SpriteAnim.SetBool("isWalking", inputmagnitude > 0);

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 newmove = new Vector3(move.x, rb.velocity.y, move.z);

        CalculateMovementVector(newmove);

        if(Input.GetButtonDown("Jump"))
        {
            jumppress = Time.time;
        }

        if(Time.time - lastgrounded <= jumpgrace && Time.time - jumppress <= jumpgrace)
        {
            SpriteAnim.SetBool("isJumping", true);
            Instantiate(JumpParticles, gc.position, Quaternion.identity);
            rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
            jumppress = null;
            lastgrounded = null;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(velocityChange * velocitymult, ForceMode.Force);
    }

    void CalculateMovementVector(Vector3 dir)
    {
        velocityChange = dir - rb.velocity;
        velocityChange = Vector3.ClampMagnitude(velocityChange, speed);
    }

    IEnumerator InstantiateTrail()
    {
        if(isGrounded && velocityMagnitude > 0.1f)
            Instantiate(PlayerTrail, gc.position, Quaternion.identity);
        yield return new WaitForSeconds(TrailTime);
        StartCoroutine(InstantiateTrail());
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 3 && isLanding) //layermask в этом случае тоже этакий тэг
        {
            print("da");
            SpriteAnim.SetBool("isJumping", false);
            Instantiate(LandParticles, gc.position, Quaternion.identity);
            isLanding = false;
            landedLastTime = Time.time;
        }
    }
}

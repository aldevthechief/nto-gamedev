using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("moving and jumping properties")]
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    //private float x, z; нет причин делать лишние поля у класса, когда эти переменные юзаются в одном блоке

    private bool isGrounded;
    public Transform gc;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    // если groundvelmult = 50, то игрок перемещается с помощью velocity, то есть он действует на физические объекты, но силы на него не действуют, если = 0, то у игрока отсутствует торможение (на него влияют только ускорения, придаваемые силами)
    public float groundvelmult;

    // множитель groundvelmult при нахождении в воздухе
    public float airmult;
    private float velocitymult;
    private Vector3 velocityChange;

    public float jumpheight;

    // coyote time для прыжка
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

    [Header("animations")]
    [SerializeField] private Animator SpriteAnim;

    [Header("particles")]
    [SerializeField] GameObject PlayerTrail;
    [SerializeField] GameObject JumpParticles;
    [SerializeField] float trailtime;

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
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        float inputmagnitude = new Vector2(x, z).magnitude;
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
            SpriteAnim.SetTrigger("isJumping");
            Instantiate(JumpParticles, gc.position, Quaternion.identity);
            rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
            jumppress = null;
            lastgrounded = null;
        }

        if(transform.position.y < player._Level._MinimalY)
        {
            player._Level.Restart();
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
        if(isGrounded)
            Instantiate(PlayerTrail, gc.position, Quaternion.identity);
        yield return new WaitForSeconds(trailtime);
        StartCoroutine(InstantiateTrail());
    }
}

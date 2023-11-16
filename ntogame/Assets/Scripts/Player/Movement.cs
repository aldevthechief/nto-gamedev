using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    // [SerializeField] private Player Player;
    // [SerializeField] private Rigidbody Rigidbody;
    // [SerializeField] private float Speed;
    // [SerializeField] private float JumpForce;
    // private Coroutine GroundCheck = null;
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    // [SerializeField] private float JumpForce;
    // private Coroutine GroundCheck = null;

    //private float x, z; нет причин делать лишние поля у класса, когда эти переменные юзаются в одном блоке

    //private bool isGrounded;
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

    // coyote time для  прыжка
    public float jumpgrace;
    private float? lastgrounded;
    private float? jumppress;

    //void Start() - перенёс данную ответственность на Level.cs
    //{
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked; 
    //}

    void Update()
    {
        //isGrounded = Physics.CheckSphere(gc.position, groundDistance, groundMask); 

        if(Physics.CheckSphere(gc.position, groundDistance, groundMask)) //поле isGrounded используется только в этом месте, не вижу смысла выделять для нее отдельную память
        {
            velocitymult = groundvelmult;
            lastgrounded = Time.time;
        }
        else
        {
            velocitymult = groundvelmult * airmult;
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 newmove = new Vector3(move.x, rb.velocity.y, move.z);

        CalculateMovementVector(newmove);

        if(Input.GetButtonDown("Jump"))
        {
            jumppress = Time.time;
        }

        if(Time.time - lastgrounded <= jumpgrace && Time.time - jumppress <= jumpgrace)
        {
            rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
            jumppress = null;
            lastgrounded = null;
        }

        // Vector3 direction = Vector3.zero;

        // if(Input.GetKey(KeyCode.D))
        // {
        //     direction.x = 1;
        // }
        // else if(Input.GetKey(KeyCode.A))
        // {
        //     direction.x = -1;
        // }

        // if(Input.GetKey(KeyCode.W))
        // {
        //     direction.z = 1;
        // }
        // else if(Input.GetKey(KeyCode.S))
        // {
        //     direction.z = -1;
        // }
        
        // if (Input.GetKey(KeyCode.Space) && GroundCheck == null)
        // {
        //     Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        //     Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

        //     GroundCheck = StartCoroutine(CheckGround());
        // }

        // transform.Translate(Speed * Time.deltaTime * direction.normalized);

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

    // private IEnumerator CheckGround()
    // {
    //     yield return new WaitForSeconds(0.2f);

    //     while(true)
    //     {
    //         foreach (RaycastHit raycast in Physics.RaycastAll(transform.position, Vector3.down, 0.1f))
    //         {
    //             if (raycast.transform.GetComponent<Platform>())
    //             {
    //                 GroundCheck = null;
    //                 yield break;
    //             }
    //         }

    //         yield return new WaitForEndOfFrame();
    //     }
    // }
}

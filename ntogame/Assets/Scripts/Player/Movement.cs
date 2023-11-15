using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private float Speed; 
    [SerializeField] private float JumpForce;
    private Coroutine GroundCheck = null; 

    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        } 

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.z = -1;
        }
        
        if (Input.GetKey(KeyCode.Space) && GroundCheck == null)
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            GroundCheck  = StartCoroutine(CheckGround());
        }

        transform.Translate(Speed * Time.deltaTime * direction.normalized);

        if (transform.position.y < Player._Level._MinimalY)
        {
            Player._Level.Restart();
        }
    }

    private IEnumerator CheckGround() 
    {
        yield return new WaitForSeconds(0.2f);

        while(true)
        {
            foreach (RaycastHit raycast in Physics.RaycastAll(transform.position, Vector3.down, 0.1f))
            {
                if (raycast.transform.GetComponent<Platform>())
                {
                    GroundCheck = null;
                    yield break;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

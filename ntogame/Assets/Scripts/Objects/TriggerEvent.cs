using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnStay;
    [SerializeField] private UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Movement>())
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<Movement>())
        {
            OnStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Movement>())
        {
            OnExit.Invoke();
        }
    }
}

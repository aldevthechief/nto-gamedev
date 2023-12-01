using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnectorRotator : MonoBehaviour
{
    public Vector3 rotationAxis;
    public float rotationSpeed = 1f;

    void Start()
    {
        transform.Rotate(rotationAxis * Random.Range(0, 180));
    }

    private void Update()
    {
        gameObject.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}

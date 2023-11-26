using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WireBlock : MonoBehaviour
{
    [Header("wire start/end position")]
    public Transform wireStartPosition;
    public Transform wireEndPosition;
    private List<Transform> points = new List<Transform>();

    [HideInInspector] public bool isUsing = false;
    // private bool isComplete = false;

    [Header("wire graphics")]
    // [SerializeField] private Color wireColor = Color.red;
    // [SerializeField] private float wireThickness = 1.0f;

    [SerializeField] private GameObject LineRendererObject;
    private LineRenderer wireLineRenderer;

    [Header("other stuff")]
    [SerializeField] LayerMask WrapLayer;
    [SerializeField] Transform WirePointHolder;
    [SerializeField] Transform WireSystemHolder;

    private void Update()
    {
        if(isUsing)
        {
            CalculateWireTrajectory();   
        }
    }

    public void StartWiring(Transform playertransf, Transform pillartransf)
    {
        wireStartPosition = pillartransf;
        wireEndPosition = playertransf;

        GameObject lr = Instantiate(LineRendererObject, Vector3.zero, Quaternion.identity);
        lr.transform.parent = WireSystemHolder;
        wireLineRenderer = lr.GetComponent<LineRenderer>();
        points.Clear();
        isUsing = true;

        points.Add(wireStartPosition);
        points.Add(wireEndPosition);
    }

    public void StopWiring(Transform pillar)
    {
        wireLineRenderer.SetPosition(points.Count - 1, pillar.position);
        isUsing = false;
    }

    private void CalculateWireTrajectory()
    {
        RaycastHit rHit;
        Vector3 direction = points[points.Count - 1].position - points[points.Count - 2].position;

        Debug.DrawRay(points[points.Count - 2].position, direction, Color.green);

        // Creating new points
        if (Physics.Raycast(points[points.Count - 2].position, direction, out rHit, direction.magnitude, WrapLayer))
        {
            GameObject newPoint = new GameObject("WirePoint" + points.Count.ToString());
            newPoint.transform.position = (rHit.point - rHit.collider.transform.position) * 1.1f + rHit.collider.transform.position;
            points.Insert(points.Count - 1, newPoint.transform);
            newPoint.transform.parent = WirePointHolder;
 
            // Debug.Log("Hit" + points.Count);
        }


        // Removing old points
        if (points.Count > 2)
        {
            Debug.DrawLine(points[points.Count - 3].position, points[points.Count - 1].position, Color.red);

            Vector3 old_direction = points[points.Count - 1].position - points[points.Count - 3].position;

            if(!Physics.Raycast(points[points.Count - 3].position, direction, out rHit, direction.magnitude, WrapLayer))
            {
                points.RemoveAt(points.Count - 2);
                // Debug.Log("Removed" + points.Count);
            }
        }

        Debug.DrawLine(points[points.Count - 2].position, points[points.Count - 1].position, Color.yellow);

        if(wireLineRenderer.positionCount != points.Count)
            DrawWire();

        wireLineRenderer.SetPosition(points.Count - 1, wireEndPosition.position);
        wireLineRenderer.SetPosition(0, wireStartPosition.position);
    }

    private void DrawWire()
    {
        wireLineRenderer.positionCount = points.Count;
        for(int i = 0; i < points.Count; i++)
        {
            wireLineRenderer.SetPosition(i, points[i].position);
        }
    }
}
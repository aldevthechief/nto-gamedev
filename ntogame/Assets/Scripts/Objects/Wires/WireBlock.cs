using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WireBlock : MonoBehaviour
{
    [Header("Wire start/end position")]
    [SerializeField] private Transform wireStartPosition;
    [SerializeField] private Transform wireEndPosition;

    // bools for logics
    private bool isUsing = false;
    private bool isComplete = false;

    [Header("Wire graphics")]
    [SerializeField] private Color wireColor = Color.red;
    [SerializeField] private float wireThickness = 1.0f;

    // objects
    private List<Transform> points = new List<Transform>();
    [SerializeField] private LineRenderer wireLineRenderer;

    private void Start()
    {
        if (wireStartPosition == null) 
        {
            wireStartPosition = this.transform;
        }
        StartWiring();
    }

    private void Update()
    {
        if (isUsing)
        {
            CalculateWireTrajectory();   
        }
    }

    public void StartWiring()
    {
        isUsing = true;

        points.Add(wireStartPosition);
        points.Add(wireEndPosition);
    }

    private void CalculateWireTrajectory()
    {
        RaycastHit rHit;
        Vector3 direction = points[points.Count - 1].position - points[points.Count - 2].position;
        int layerMask = 1 << 3;

        // Creating new points
        if (Physics.Raycast(points[points.Count-2].position, direction, out rHit, direction.magnitude, layerMask))
        {
            GameObject newPoint = new GameObject();
            newPoint.transform.position = (rHit.point - rHit.collider.transform.position) * 1.07f + rHit.collider.transform.position;
            points.Insert(points.Count - 1, newPoint.transform);
 
            Debug.Log("Hit" + points.Count);
        }


        // Removing old points
        if (points.Count > 2)
        {
            Debug.DrawLine(points[points.Count - 3].position, points[points.Count - 1].position, Color.red);

            Vector3 old_direction = points[points.Count - 1].position - points[points.Count - 3].position;

            if (!Physics.Raycast(points[points.Count - 3].position, direction, out rHit, direction.magnitude, layerMask))
            {
                points.RemoveAt(points.Count - 2);
                Debug.Log("Removed" + points.Count);
            }
        }
        Debug.DrawLine(points[points.Count - 2].position, points[points.Count - 1].position, Color.yellow);

        if (wireLineRenderer.positionCount != points.Count)
            DrawWire();

        wireLineRenderer.SetPosition(points.Count-1, wireEndPosition.position);
        wireLineRenderer.SetPosition(0, wireStartPosition.position);

    }

    private void DrawWire()
    {
        wireLineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            wireLineRenderer.SetPosition(i, points[i].position);
        }
    }
}
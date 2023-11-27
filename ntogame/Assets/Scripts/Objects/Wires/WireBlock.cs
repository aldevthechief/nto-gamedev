using System.Collections.Generic;
using UnityEngine;

public class WireBlock : MonoBehaviour
{
    [HideInInspector] public bool isUsing = false;

    [Header("wire start/end position")]
    public Transform wireStartTransform;
    public Transform wireEndTransform;

    [Header("wire graphics")]
    [SerializeField] private GameObject LineRendererObject;
    private LineRenderer wireLineRenderer;
    private Vector3 ropeTensionPos;

    [Header("other stuff")]
    [SerializeField] LayerMask WrapLayer;
    [SerializeField] Transform WireSystemHolder;

    public List<Vector3> ropePositions { get; set; } = new List<Vector3>();

    // private void Awake()
    // {
    //     //StartWiring(wireStartTransform, wireEndTransform);
    // }

    private void Update()
    {
        // Debug.Log(wireEndTransform.position);
        if (isUsing)
        {
            UpdateRopePositions();
            LastSegmentGoToPlayerPos();

            DetectCollisionEnter();
            if(ropePositions.Count > 2) 
                DetectCollisionExits();
        }
    }

    public void StartWiring(Transform playertransf, Transform pillartransf)
    {
        wireStartTransform = pillartransf;
        wireEndTransform = playertransf;

        GameObject lr = Instantiate(LineRendererObject, Vector3.zero, Quaternion.identity);
        lr.transform.parent = WireSystemHolder;
        wireLineRenderer = lr.GetComponent<LineRenderer>();
        ropePositions.Clear();
        isUsing = true;

        ropePositions.Add(wireStartTransform.position);
        ropePositions.Add(wireEndTransform.position);
    }

    public void StopWiring(Transform pillar)
    {
        wireLineRenderer.SetPosition(ropePositions.Count - 1, pillar.position);
        wireLineRenderer.gameObject.GetComponent<WireCollisions>().BakeCollisions();
        isUsing = false;
    }

    private void DetectCollisionEnter()
    {
        RaycastHit hit;
        if(Physics.Linecast(wireEndTransform.position, wireLineRenderer.GetPosition(ropePositions.Count - 2), out hit, WrapLayer))
        {
            //ropeTensionPos = hit.point + -0.4f * (hit.point - ropePositions[ropePositions.Count - 2]).normalized;
            ropePositions.Insert(ropePositions.Count - 1, (hit.point - hit.collider.transform.position) * 1.07f + hit.collider.transform.position);
            ropeTensionPos = ropePositions[ropePositions.Count - 2] + -0.4f * (ropePositions[ropePositions.Count - 2] - ropePositions[ropePositions.Count - 3]).normalized;
        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit hit;
        // Debug.DrawLine(wireEndTransform.position, ropeTensionPos, Color.red);
        if (!Physics.Linecast(wireEndTransform.position, ropeTensionPos, out hit, WrapLayer)) // rope.GetPosition(ropePositions.Count - 3)
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);

            if(ropePositions.Count > 2)
            {
                ropeTensionPos = ropePositions[ropePositions.Count - 2] + -0.4f * (ropePositions[ropePositions.Count - 2] - ropePositions[ropePositions.Count - 3]).normalized;
                // Debug.Log("changedTensPos");
            }
        }
    }

    private void UpdateRopePositions()
    {
        wireLineRenderer.positionCount = ropePositions.Count;
        wireLineRenderer.SetPositions(ropePositions.ToArray());
    }

    private void LastSegmentGoToPlayerPos() => wireLineRenderer.SetPosition(wireLineRenderer.positionCount - 1, wireEndTransform.position);
}
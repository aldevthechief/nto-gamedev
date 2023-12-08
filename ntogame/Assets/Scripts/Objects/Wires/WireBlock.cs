using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EZCameraShake;

public class WireBlock : MonoBehaviour
{
    [HideInInspector] public bool isUsing = false;

    [Header("wire start/end position")]
    public Transform wireStartTransform;
    public Transform wireEndTransform;

    [Header("wire graphics")]
    [SerializeField] private GameObject LineRendererObject;
    private LineRenderer wireLineRenderer;
    private Vector3 wireTensionPos;

    [Header("other stuff")]
    [SerializeField] LayerMask WrapLayer;
    [SerializeField] Transform WireSystemHolder;
    [SerializeField] KeyBox Box;

    public List<Vector3> wirePositions { get; set; } = new List<Vector3>();

    [SerializeField] private UnityEngine.Events.UnityEvent OnEnergyFlow;

    [Header("wire length")]
    public float maxWireLength;
    [HideInInspector] public float fullWireLength;

    private List<float> wireLengthStorage { get; set; } = new List<float>(); //Stores the length of rope from 0 to n-1
    private float wireLengthStorageSumm; //Summ of wireLengthStorage
    private float dynamicWireLength; //Stores the length of rope from n-1 to n (from last wrap point to player)

    private bool isDestroying = false; //When true, it makes wireLineRenderer width smaller (Check TryDestroyWireLineRenderer for more info)

    // private void Awake()
    // {
    //     //StartWiring(wireStartTransform, wireEndTransform);
    // }

    public void UpdateLines()
    {
        if (isUsing)
        {
            UpdateRopePositions();
            LastSegmentGoToPlayerPos();

            DetectCollisionEnter();
        }
    }

    private void Update()
    {
        // Debug.Log(wireEndTransform.position);
        if (isUsing)
        {
            UpdateRopePositions();
            LastSegmentGoToPlayerPos();

            DetectCollisionEnter();
            if(wirePositions.Count > 2) 
                DetectCollisionExits();

            CalculateFullWireLength();
        }

        TryDestroyWireLineRenderer();
    // Debug.Log(fullWireLength);
    }

    public void StartWiring(Transform playertransf, Transform pillartransf)
    {
        wireStartTransform = pillartransf;
        wireEndTransform = playertransf;

        GameObject lr = Instantiate(LineRendererObject, Vector3.zero, Quaternion.identity);
        lr.transform.parent = WireSystemHolder;
        wireLineRenderer = lr.GetComponent<LineRenderer>();
        wirePositions.Clear();
        isUsing = true;

        wirePositions.Add(wireStartTransform.position);
        wirePositions.Add(wireEndTransform.position);

        wireLengthStorage.Add((wireEndTransform.position - wireStartTransform.position).magnitude);
    }

    public void StopWiring(Transform pillar)
    {
        wireLineRenderer.SetPosition(wireLineRenderer.positionCount - 1, pillar.position);
        StartCoroutine(wireLineRenderer.gameObject.GetComponent<WireCollisions>().BakeCollisions());

        WirePillar endWirePillar = pillar.gameObject.GetComponentInParent<WirePillar>();
        WirePillar startWirePillar = wireStartTransform.gameObject.GetComponentInParent<WirePillar>();

        if(endWirePillar != null && startWirePillar != null)
        {
            endWirePillar.Connect(startWirePillar, true);
            startWirePillar.Connect(endWirePillar, false);
        }

        CheckIfEnergyIsFlowing();

        isUsing = false;
    }

    void CheckIfEnergyIsFlowing()
    {
        WirePillar[] sceneconnectibles = GameObject.FindObjectsOfType<WirePillar>();
        bool allconnected = true;
        foreach(WirePillar x in sceneconnectibles)
        {
            if(!x.isConnected)
            {
                allconnected = false;
                break;
            }
        }

        if (allconnected)
        {
            OnEnergyFlow.Invoke();
            Box.OpenTheBox();
        }
    }

    private void DetectCollisionEnter()
    {
        RaycastHit hit;
        if(Physics.Linecast(wireEndTransform.position, wireLineRenderer.GetPosition(wirePositions.Count - 2), out hit, WrapLayer))
        {
            //wirePositions.Insert(wirePositions.Count - 1, (hit.point - hit.collider.transform.position) * 1.07f + hit.collider.transform.position);
            wirePositions.Insert(wirePositions.Count - 1, hit.point + (Vector3.Cross(hit.normal, Vector3.down) * 0.2f));

            wireTensionPos = wirePositions[wirePositions.Count - 2] + -0.4f * (wirePositions[wirePositions.Count - 2] - wirePositions[wirePositions.Count - 3]).normalized;

            //Length calculations
            wireLengthStorage.Add((wirePositions[wirePositions.Count - 2] - wirePositions[wirePositions.Count - 3]).magnitude);
            wireLengthStorageSumm = wireLengthStorage.Sum();
        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit hit;
        // Debug.DrawLine(wireEndTransform.position, ropeTensionPos, Color.red);
        if (!Physics.Linecast(wireEndTransform.position, wireTensionPos, out hit, WrapLayer))
        {
            wirePositions.RemoveAt(wirePositions.Count - 2);

            //Length calculations
            wireLengthStorage.RemoveAt(wirePositions.Count - 1);
            wireLengthStorageSumm = wireLengthStorage.Sum();
            //End of length calculations

            if(wirePositions.Count > 2)
            {
                wireTensionPos = wirePositions[wirePositions.Count - 2] + -0.4f * (wirePositions[wirePositions.Count - 2] - wirePositions[wirePositions.Count - 3]).normalized;
            }
        }
    }

    private void TearWireApart() //<-- Cleans all data and destroys the wire
    {
        CameraShaker.Instance.ShakeOnce(5, 3, 0.1f, 1);
        isUsing = false;
        isDestroying = true;
        wirePositions.Clear();
        wireLengthStorage.Clear();
        wireLengthStorageSumm = 0f;
        dynamicWireLength = 0f;
        fullWireLength = 0f;
        wireTensionPos = Vector3.zero;
    }

    private void TryDestroyWireLineRenderer() //<-- You should probably refactor this!
    {
        if (isDestroying)
        {
            wireLineRenderer.widthMultiplier -= 3f * Time.deltaTime;

            if (wireLineRenderer.widthMultiplier <= 0.02f)
            {
                isDestroying = false;
                Destroy(wireLineRenderer.gameObject);
            }
        }
    }

    private void CalculateFullWireLength()
    {
        dynamicWireLength = (wireEndTransform.position - wirePositions[wirePositions.Count - 2]).magnitude;
        fullWireLength = wireLengthStorageSumm + dynamicWireLength;

        if (fullWireLength > maxWireLength) 
            TearWireApart();
    }

    private void UpdateRopePositions()
    {
        wireLineRenderer.positionCount = wirePositions.Count;
        wireLineRenderer.SetPositions(wirePositions.ToArray());
    }

    private void LastSegmentGoToPlayerPos() => wireLineRenderer.SetPosition(wireLineRenderer.positionCount - 1, wireEndTransform.position);
}
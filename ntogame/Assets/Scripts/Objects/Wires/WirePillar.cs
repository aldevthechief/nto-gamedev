using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePillar : MonoBehaviour
{
    [SerializeField] private WirePillar ConnecterFrom;
    [SerializeField] private WirePillar ConnectedTo;
    [SerializeField] private bool IsDispencer; //раздает, т е соединение в него не требуется
    [SerializeField] private bool IsEnding; //замыкает, т е соединение от него не требуется

    public WirePillar[] _Connection => new WirePillar[] { ConnecterFrom, ConnectedTo };

    public bool isConnected
    {
        get
        {
            if (IsDispencer)
            {
                return ConnectedTo != null;
            }
            else if (IsEnding)
            {
                return ConnecterFrom != null;
            }
            else
            {
                return ConnectedTo != null && ConnecterFrom != null;
            }
        }
    }

    public void Disconnect()
    {
        ConnectedTo = null;
        ConnecterFrom = null;
    }

    public void Disconnect(WirePillar pillar)
    {
        if(ConnectedTo == pillar)
        {
            ConnectedTo = null;
        }
        if (ConnecterFrom == pillar)
        {
            ConnecterFrom = null;
        }
    }

    public void Connect(WirePillar pillar, bool from)
    {
        if (from)
        {
            ConnecterFrom = pillar;
        }
        else
        {
            ConnectedTo = pillar;
        }
    }
}

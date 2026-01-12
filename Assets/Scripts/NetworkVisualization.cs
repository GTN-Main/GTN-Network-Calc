using System.Collections.Generic;
using UnityEngine;

public class NetworkVisualization : MonoBehaviour
{
    Network _network;
    NetworkVisualization _parentNetworkV;
    List<NetDataV> _netDataVs = new List<NetDataV>();
    List<NetworkVisualization> _subNetVs = new List<NetworkVisualization>();

    public NetworkVisualization Init(Network network, NetworkVisualization parentNetworkV)
    {
        _network = network;
        _parentNetworkV = parentNetworkV;
        return this;
    }

    public void AddNetDataV(NetDataV netDataV)
    {
        _netDataVs.Add(netDataV);
    }

    public void RemoveNetDataV(NetDataV netDataV)
    {
        _netDataVs.Remove(netDataV);
    }

    public void AddSubNetV(NetworkVisualization subNetV)
    {
        _subNetVs.Add(subNetV);
    }

    public void RemoveSubNetV(NetworkVisualization subNetV)
    {
        _subNetVs.Remove(subNetV);
    }

    public Network GetNetwork()
    {
        return _network;
    }
}

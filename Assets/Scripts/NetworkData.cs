using System.Collections.Generic;

[System.Serializable]
public class NetworkData
{
    List<Network> _netBases = new List<Network>();

    public NetworkData()
    {
    }

    public List<Network> GetNetworkBases()
    {
        return _netBases;
    }

    public void AddNetworkBase(Network netBase)
    {
        _netBases.Add(netBase);
    }

    public void RemoveNetworkBase(Network netBase)
    {
        _netBases.Remove(netBase);
    }
}

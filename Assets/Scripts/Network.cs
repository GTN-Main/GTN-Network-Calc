using System.Collections.Generic;

public class Network
{
    string _name;
    IP _ip;
    Mask _mask;
    Network _parentNetwork;
    List<Tag> _tags = new List<Tag>();
    List<Network> _subNets = new List<Network>();

    public Network(string name, IP ip, Mask mask, Network parentNetwork)
    {
        this._name = name;
        this._ip = ip;
        this._mask = mask;
        this._parentNetwork = parentNetwork;
    }

    public void ChangeIP(IP newIP)
    {
        _ip = newIP;
    }

    public void ChangeMask(Mask newMask)
    {
        _mask = newMask;
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new System.Exception("Network name cannot be null or empty.");
        if (newName == "")
            throw new System.Exception("Network name cannot be an empty string.");

        _name = newName;
    }

    public void AddTag(Tag tag)
    {
        _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        _tags.Remove(tag);
    }

    public void AddSubNet(Network subNet)
    {
        _subNets.Add(subNet);
    }

    public void RemoveSubNet(Network subNet)
    {
        _subNets.Remove(subNet);
    }

    public string GetName()
    {
        return _name;
    }

    public IP GetIP()
    {
        return _ip;
    }

    public Mask GetMask()
    {
        return _mask;
    }

    public Network GetParentNetwork()
    {
        return _parentNetwork;
    }

    public List<Tag> GetTags()
    {
        return _tags;
    }

    public List<Network> GetSubNets()
    {
        return _subNets;
    }

    public IP GetBroadcastIP()
    {
        byte[] ipBytes = _ip.GetIPAsBytes();
        byte[] maskBytes = _mask.GetMaskAsBytes();
        byte[] broadcastBytes = new byte[4];

        for (int i = 0; i < 4; i++)
        {
            broadcastBytes[i] = (byte)(ipBytes[i] | (~maskBytes[i]));
        }

        return new IP(broadcastBytes);
    }
}
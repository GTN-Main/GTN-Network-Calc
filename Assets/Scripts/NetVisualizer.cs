using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class NetVisualizer : MonoBehaviour
{
    static GameObject _networkVisualizerPrefab;
    static GameObject _netDataVPrefab;

    void Start()
    {
        Test();
    }

    [ContextMenu("Test Visualize Network")]
    public void Test()
    {
        NetworkData networkData = new NetworkData();
        NetHolder.SetNetworkData(networkData);

        Network network1 = new Network(
                "Network Root 1",
                new IP(new byte[] { 192, 168, 1, 0 }),

                new Mask(new byte[] { 255, 255, 255, 0 }),

                null
        );

        network1.AddTag(new Tag("Office", network1, network1.GetIP().GetNextIP(), network1.GetMask()));
        network1.AddTag(new Tag("SecondFloor", network1, network1.GetIP().GetNextIP().GetNextIP(), network1.GetMask()));

        Network network2 = new Network(
                "Network Root 2",
                new IP(new byte[] { 10, 0, 10, 0 }),

                new Mask(new byte[] { 255, 255, 0, 0 }),

                null
        );

        Network network3 = new Network(
                "JAWs Office",
                new IP(new byte[] { 10, 0, 10, 30 }),

                new Mask(new byte[] { 255, 255, 255, 0 }),

                null
        );

        Network network4 = new Network(
                "JAWs Second Office",
                new IP(new byte[] { 10, 0, 10, 32 }),

                new Mask("/26"),

                null
        );

        network2.AddTag(new Tag("FirstOffice", network1, network1.GetIP().GetNextIP(), network1.GetMask()));
        network2.AddSubNet(network3);
        network3.AddSubNet(network4);

        networkData.AddNetworkBase(
            network1
        );

        networkData.AddNetworkBase(
            network2
        );

        VisualizeNetwork();
    }

    public static void VisualizeNetwork()
    {
        if (_networkVisualizerPrefab == null)
        {
            _networkVisualizerPrefab = Resources.Load<GameObject>("Prefabs/NetworkVisualization");
            if (_networkVisualizerPrefab != null)
                Debug.Log("Loaded NetworkVisualization prefab.");
            else
                Debug.LogError("Failed to load NetworkVisualization prefab.");
        }

        if (_netDataVPrefab == null)
        {
            _netDataVPrefab = Resources.Load<GameObject>("Prefabs/NetDataV");
            if (_netDataVPrefab != null)
                Debug.Log("Loaded NetDataV prefab.");
            else
                Debug.LogError("Failed to load NetDataV prefab.");
        }

        NetworkData networkData = NetHolder.GetNetworkData();
        List<Network> networks = networkData.GetNetworkBases();

        foreach (Network network in networks)
        {
            VisualizeNetworkRecursive(network, 0);
        }
    }

    static void VisualizeNetworkRecursive(Network network, int depth, NetworkVisualization parentNetworkV = null)
    {
        List<NetDataV> netDataVs = new List<NetDataV>();
        Mask mask = network.GetMask();
        IP net_ip = network.GetIP();
        IP broadcast_ip = network.GetBroadcastIP();
        IP first_usable_ip = net_ip.GetNextIP();
        IP last_usable_ip = broadcast_ip.GetPreviousIP();
        int hostsCount = mask.GetNumberOfUsableHosts();

        NetworkVisualization networkV = SpawnNetworkVisualizer(network, parentNetworkV);

        string indent = "";
        for (int i = 0; i < depth; i++)
        {
            indent += "|         ";
        }

        netDataVs.Add(new NetDataV().Init(networkV, indent+$"----DepthOpen({depth})----"));
        netDataVs.Add(new NetDataV().Init(networkV, net_ip, mask, indent+"Name: " + network.GetName()));
        netDataVs.Add(new NetDataV().Init(networkV, net_ip, mask, indent+"Network IP: "));
        netDataVs.Add(new NetDataV().Init(networkV, first_usable_ip, mask, indent+"First Usable IP: "));
        netDataVs.Add(new NetDataV().Init(networkV, last_usable_ip, mask, indent+"Last Usable IP: "));
        netDataVs.Add(new NetDataV().Init(networkV, broadcast_ip, mask, indent+"Broadcast IP: "));
        netDataVs.Add(new NetDataV().Init(networkV, indent+"Number of Usable Hosts: " + hostsCount));
        netDataVs.Add(new NetDataV().Init(networkV, indent+"Tags:"));
        
        foreach (Tag tag in network.GetTags())
        {
            netDataVs.Add(new NetDataV().Init(networkV, tag.ip, tag.mask, indent+"|         " + "Tag-- " + tag.Text));
        }

netDataVs.Add(new NetDataV().Init(networkV, indent+"Subnets:"));

        foreach (NetDataV netDataV in netDataVs)
        {
            SpawnNetDataV(networkV, netDataV);
        }
        netDataVs.Clear();

        foreach (Network subNet in network.GetSubNets())
        {
            VisualizeNetworkRecursive(subNet, depth + 1, networkV);
        }

        netDataVs.Add(new NetDataV().Init(networkV, indent+$"----DepthClose({depth})----"));
        SpawnNetDataV(networkV, netDataVs[0]);
    }

    static NetworkVisualization SpawnNetworkVisualizer(Network network, NetworkVisualization parentNetworkV = null)
    {
        GameObject networkVObj = GameObject.Instantiate(_networkVisualizerPrefab);
        NetworkVisualization networkV = networkVObj.GetComponent<NetworkVisualization>().Init(network, parentNetworkV);
        networkVObj.name = "NetworkVisualizer_" + network.GetName();
        if (parentNetworkV != null)
        {
            networkVObj.transform.parent = parentNetworkV.transform;
        }
        else
        {
            networkVObj.transform.parent = NetHolder.instance.rootVTransform;
        }
        networkVObj.transform.SetAsLastSibling();
        networkVObj.transform.localScale = Vector3.one;

        return networkV;
    }

    static NetDataV SpawnNetDataV(NetworkVisualization parentNetworkV, NetDataV _netDataV)
    {
        GameObject netDataVObj = GameObject.Instantiate(_netDataVPrefab);
        NetDataV netDataV = netDataVObj.GetComponent<NetDataV>().Init(_netDataV);
        netDataVObj.name = "NetDataV_" + netDataV.GetText();
        netDataVObj.transform.parent = parentNetworkV.transform;
        netDataVObj.transform.SetAsLastSibling();
        netDataVObj.transform.localScale = Vector3.one;

        netDataV.SetText();

        parentNetworkV.AddNetDataV(netDataV);

        return netDataV;
    }
}
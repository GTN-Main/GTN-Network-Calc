using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetDataV : Checker
{
    [SerializeField] TMP_Text TMPText;
    public enum EDataType
    {
        DESCRIPTION,
        IPDESCRIPTION,
        COUNT,
        NONE
    }

    public string _text { get; private set; }
    public Tag _tag { get; private set; }
    public IP _IP { get; private set; }
    public Mask _mask { get; private set; }
    public NetworkVisualization _parentNetworkV { get; private set; }
    EDataType _dataType;

    public void SetText()
    {
        TMPText.text = GetText();
    }

    public EDataType GetDataType()
    {
        return _dataType;
    }

    public NetDataV Init(NetDataV netDataV)
    {
        _dataType = netDataV.GetDataType();
        _text = netDataV._text;
        _IP = netDataV._IP;
        _mask = netDataV._mask;
        _tag = netDataV._tag;
        _parentNetworkV = netDataV._parentNetworkV;
        return this;
    }

    public NetDataV Init(NetworkVisualization parentNetworkV, string text)
    {
        _dataType = EDataType.DESCRIPTION;
        _netDataV = this;
        _parentNetworkV = parentNetworkV;
        _text = text;
        return this;
    }

    public NetDataV Init(NetworkVisualization parentNetworkV, IP ip, Mask mask, string text)
    {
        _dataType = EDataType.IPDESCRIPTION;
        _netDataV = this;
        _parentNetworkV = parentNetworkV;
        _IP = ip;
        _mask = mask;
        _tag = new Tag(text, parentNetworkV.GetNetwork(), ip, mask);
        _text = text;
        return this;
    }

    public NetDataV Init(NetworkVisualization parentNetworkV, Tag tag)
    {
        _dataType = EDataType.IPDESCRIPTION;
        _netDataV = this;
        _tag = tag;
        _parentNetworkV = parentNetworkV;
        _IP = tag.ip;
        _mask = tag.mask;
        _text = tag.Text;
        return this;
    }

    public NetDataV Init(NetworkVisualization parentNetworkV)
    {
        _dataType = EDataType.NONE;
        _netDataV = this;
        _parentNetworkV = parentNetworkV;
        return this;
    }

    public NetDataV Init(NetworkVisualization parentNetworkV, int count)
    {
        _dataType = EDataType.COUNT;
        _netDataV = this;
        _parentNetworkV = parentNetworkV;
        _text = count.ToString();
        return this;
    }

    public string GetText()
    {
        switch (_dataType)
        {
            case EDataType.DESCRIPTION:
                return _text;
            case EDataType.IPDESCRIPTION:
                return $"{_text} {_IP}{_mask.GetMaskAsStringSlashNotation()} ({_mask.GetMaskAsStringDottedNotation()})";
            case EDataType.COUNT:
                return _text;
            case EDataType.NONE:
                return "";
            default:
                return "";
        }
    }
}

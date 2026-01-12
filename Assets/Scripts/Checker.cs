using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Checker : MonoBehaviour, IPointerClickHandler
{
    protected NetDataV _netDataV;
    bool _isChecked = false;
    public void Init(NetDataV netDataV)
    {
        _netDataV = netDataV;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NetDataV.EDataType dataType = _netDataV.GetDataType();
        // Replace _netDataV in NetHolder 
    }

    public bool IsChecked()
    {
        return _isChecked;
    }

    public void Check()
    {
        _isChecked = true;
    }

    public void UnCheck()
    {
        _isChecked = false;
    }
}

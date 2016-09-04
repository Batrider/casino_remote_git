using UnityEngine;
using System.Collections;

public class MenuItemCenterCallback : MonoBehaviour {
    public UICenterOnChild uc;
    void Start () {
        uc.onCenterIndex += CenterIndex;
    }
    void Enable()
    {
        uc.onCenterIndex += CenterIndex;
    }
    void CenterIndex(int index)
    {
        Debug.Log("Index:"+index);
    }

}

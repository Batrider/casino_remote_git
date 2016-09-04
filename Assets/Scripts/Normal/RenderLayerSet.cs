using UnityEngine;
using System.Collections;

public class RenderLayerSet : MonoBehaviour {

    public string SortLayerName = "progress";
    public int sortOrder = 0;
    void Start()
    {
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in mrs)
        {
            mr.sortingLayerName = SortLayerName;
            mr.sortingOrder = sortOrder;
        }
    }
}

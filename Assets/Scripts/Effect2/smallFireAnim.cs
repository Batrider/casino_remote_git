using UnityEngine;
using System.Collections;

public class smallFireAnim : MonoBehaviour {

    public Object fireAnimPrefabs;

    public void fireShow(GameObject go)
    {

        GameObject fire = Instantiate(fireAnimPrefabs) as GameObject;
        fire.transform.parent = go.transform;
        fire.transform.localScale = Vector3.one;
        fire.transform.localEulerAngles = Vector3.zero;
        fire.transform.localPosition = Vector3.zero;

    }
}

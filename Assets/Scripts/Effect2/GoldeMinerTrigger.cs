using UnityEngine;
using System.Collections;

public class GoldeMinerTrigger : MonoBehaviour {

    bool beSelected = false;

    void OnTriggerEnter(Collider col)
    {
        if(col.name!= "bg")
        {
        Debug.Log(col.name);
        transform.parent = col.transform;
        GetComponentInParent<CarController>().StartCoroutine("CloseTools");
        }
    }
}

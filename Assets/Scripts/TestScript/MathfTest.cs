using UnityEngine;
using System.Collections;

public class MathfTest : MonoBehaviour {
    public float current;
    public float target;
    public Transform obj1;
    public Transform obj2;
    Quaternion q ;
    void Update() {
        q.SetLookRotation(obj1.localPosition, obj2.localPosition);
        transform.rotation = q;

    }

}

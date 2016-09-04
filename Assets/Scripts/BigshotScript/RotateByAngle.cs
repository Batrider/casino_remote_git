using UnityEngine;
using System.Collections;

public class RotateByAngle : MonoBehaviour {
    public Vector3 angle = new Vector3(0, 0, 7.7f);
    public float deltaTime = 0.3f;
    float time = 0;
    void Update () {
        time += Time.deltaTime;
        if (time > deltaTime)
        {
            time = 0;
            transform.localEulerAngles+=angle;
        }
    }
}

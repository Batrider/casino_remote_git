using UnityEngine;
using System.Collections;

public class RudderRun : MonoBehaviour {
    public float time;

    public void rudderRun(float angle)
    {
        TweenRotation tr = gameObject.GetComponent<TweenRotation>();
        tr.from =new Vector3(0,0,transform.localEulerAngles.z);
        Debug.Log(transform.localEulerAngles.z);
        tr.to =new Vector3(0,0,angle);
        tr.duration = time;
        tr.enabled = true;
        tr.ResetToBeginning();
    }
}

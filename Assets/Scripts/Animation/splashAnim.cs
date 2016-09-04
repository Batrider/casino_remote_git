using UnityEngine;
using System.Collections;

public class splashAnim : MonoBehaviour {
    private float timeCount;
	public void RotateStart()
    {
        timeCount = 5f;
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate()
    {
        while (timeCount>0)
        {
            transform.Rotate(new Vector3(0,0,-144*Time.deltaTime));
            timeCount-=Time.deltaTime;
            yield return null;
        }
        transform.localEulerAngles = new Vector3(0, 0, 64.7f);
    }
}

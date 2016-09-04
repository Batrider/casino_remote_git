using UnityEngine;
using System.Collections;

public class LightChange : MonoBehaviour {
    public GameObject[] objs;
	// Use this for initialization
	void Start () {
        StartCoroutine(AlaphShowObj());
	}
    IEnumerator AlaphShowObj()
    {
        for (int i = 0; i<objs.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            TweenAlpha.Begin(objs[i],0.5f,1);
        }
    }
}

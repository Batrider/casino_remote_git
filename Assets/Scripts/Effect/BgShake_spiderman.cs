using UnityEngine;
using System.Collections;

public class BgShake_spiderman : MonoBehaviour {
	public Vector3 offset;
	int count = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		count++;
		gameObject.transform.localPosition = gameObject.transform.localPosition+offset;
		if(count>5)
		{
			count = 0;
			offset = -1*offset;
		}
		if(Time.timeSinceLevelLoad>1.5f)
		{
			GetComponent<BgShake_spiderman>().enabled = false;
		}
	}
}

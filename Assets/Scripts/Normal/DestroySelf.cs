using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {
	public float exitTimes;
	void Update () {
		if(Time.timeSinceLevelLoad>exitTimes)
		{
			Destroy(gameObject);
		}
	
	}
}

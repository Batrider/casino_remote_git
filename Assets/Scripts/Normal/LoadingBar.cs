using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour {
	float timeCount = 0;
	void Update()
	{
		timeCount += Time.deltaTime;
		if(timeCount>0.1f)
		{
			timeCount = 0;
			transform.localEulerAngles = transform.localEulerAngles + new Vector3(0,0,-30);
		}
	}
}

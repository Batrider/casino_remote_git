using UnityEngine;
using System.Collections;

public class Playeffect : MonoBehaviour {
	private float cameraValue;
	private bool isMaxSize = true;
	public float CameraMinSize = 0.77f;
	void Start()
	{
		cameraValue = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize;
	}
    /*
	void Update()
	{
		if(isMaxSize&&cameraValue<0.995f)
		{
			cameraValue = Mathf.Lerp(cameraValue,1,8*Time.deltaTime);
			GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = cameraValue;

		}
		else if(!isMaxSize&&cameraValue>(CameraMinSize+0.005))
		{
			cameraValue = Mathf.Lerp(cameraValue, CameraMinSize, 12*Time.deltaTime);
			GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = cameraValue;
		}
	}
 */   
	public void ChangeTheSizeOfCamera()
	{
		isMaxSize = !isMaxSize;
	}

}

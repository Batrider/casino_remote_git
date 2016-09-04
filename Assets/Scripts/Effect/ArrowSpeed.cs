using UnityEngine;
using System.Collections;

public class ArrowSpeed : MonoBehaviour {
	// Use this for initialization
	private float targetAngle;
	public void ChangeTheArrowAngle()
	{
		Time.timeScale = (Time.timeScale)%3+1;
		StartCoroutine(yieAminute());
	}
	IEnumerator yieAminute()
	{
		yield return new WaitForSeconds(0.01f);
		targetAngle = -(Time.timeScale-1)*100f;
		while(transform.localEulerAngles.z - targetAngle>5)
		{
			yield return new WaitForSeconds(Time.deltaTime);
			transform.localEulerAngles =new Vector3(0,0,Mathf.LerpAngle(transform.localEulerAngles.z,targetAngle,0.5f*Time.deltaTime));
		}
	}


}

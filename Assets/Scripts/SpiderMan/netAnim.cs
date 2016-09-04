using UnityEngine;
using System.Collections;

public class netAnim : MonoBehaviour {
	public float speedX;
	public float speedY;
	private UIPanel uipanel;
	// Use this for initialization
	void Start () {
		uipanel = GetComponent<UIPanel>();
	
	}
	
	// Update is called once per frame
	void Update () {
		speedX +=Time.deltaTime*speedX;
		speedY +=Time.deltaTime*speedY;
		uipanel.baseClipRegion = new Vector4(0,0,speedX,speedY);
		if(speedX>210)
		{
			GetComponent<netAnim>().enabled = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {
	private float durtime = 0;
	public UI2DSprite u2s;
	// Use this for initialization
	void Start () {
		u2s = GetComponent<UI2DSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		durtime+=Time.deltaTime;
	}
}

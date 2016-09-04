using UnityEngine;
using System.Collections;

public class AutoScaleCameraSize : MonoBehaviour {
    public float height;
    public float width;
    private float screenHeight;
    private float screenWidth;
	// Use this for initialization
	void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        float sizeFactor = screenWidth / screenHeight;
        float sizeFactor2 = height / width;
        transform.localScale = new Vector3(1, sizeFactor2 / sizeFactor, 1);

	}
	
	// Update is called once per frame
	void Update () {

	
	}
}

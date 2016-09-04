using UnityEngine;
using System.Collections;

public class ChangeUISprite : MonoBehaviour {
    UISprite us;
	// Use this for initialization
    float timeCount;
    int indexOfSprite = 1;
	void Start () {
        us = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {
        timeCount += Time.fixedDeltaTime;
        if (timeCount > 1f)
        {
            timeCount = 0;
            us.spriteName = (indexOfSprite % 2).ToString();
            us.depth = indexOfSprite % 2;
            indexOfSprite++;
        }
	}
}

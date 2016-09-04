using UnityEngine;
using System.Collections;

public class RandomCard : MonoBehaviour {
	private GameObject parentOBJ;
	private bool turned;
	// Use this for initialization
	void Start () {
		parentOBJ = transform.parent.gameObject;
		turned = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(parentOBJ.transform.localEulerAngles.y<180)
		{
			parentOBJ.transform.Rotate(new Vector3(0,180*Time.deltaTime,0));
		}
		else
		{
//			parentOBJ.transform.localEulerAngles = Vector3.zero;
			GetComponent<RandomCard>().enabled = false;
		}
		if(parentOBJ.transform.localEulerAngles.y>90&&turned)
		{
			turned = false;
			GetComponent<UISprite>().spriteName = "card"+Random.Range(1,5);
		}
	}
}


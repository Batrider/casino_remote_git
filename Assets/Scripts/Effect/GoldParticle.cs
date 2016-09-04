using UnityEngine;
using System.Collections;

public class GoldParticle : MonoBehaviour {
	// Use this for initialization
	float ran_dir_x;
	float ran_dir_y;
	void Start () {
		ran_dir_x = Random.Range(-80,80);
		ran_dir_y = Random.Range(0,100);
		gameObject.rigidbody.AddForce(new Vector2(ran_dir_x,ran_dir_y));
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(new Vector3(5f*ran_dir_x*Time.deltaTime,5f*ran_dir_y*Time.deltaTime,0));
		if(gameObject.transform.localPosition.y<-400)
			Destroy(gameObject);
	}
}

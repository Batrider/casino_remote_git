using UnityEngine;
using System.Collections;

public class CaidaiEffect : MonoBehaviour {
	float ran_dir_x;
	float ran_dir_y;
    float exitTime;
	void Start () {
		ran_dir_x = Random.Range(-20,20);
		ran_dir_y = Random.Range(0,20);
		gameObject.rigidbody.AddForce(new Vector2(ran_dir_x,ran_dir_y));
	}
	
	// Update is called once per frame
	void Update () {
        exitTime += Time.deltaTime;
		gameObject.transform.Rotate(new Vector3(5f*ran_dir_x*Time.deltaTime,5f*ran_dir_y*Time.deltaTime,0));
        if(exitTime>8)
			Destroy(gameObject);
	}
}

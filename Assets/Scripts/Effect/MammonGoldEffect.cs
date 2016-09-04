using UnityEngine;
using System.Collections;

public class MammonGoldEffect: MonoBehaviour {
	// Update is called once per frame
	void Start()
	{
		GetComponent<AudioSource>().Play();
	}
	void Update () {
		transform.localPosition -= new Vector3(250*Time.deltaTime,0,0);
		if(transform.localPosition.x<-180)
		{
			if(GameObject.FindWithTag("Credits"))
			{
				Animator am = GameObject.FindWithTag("Credits").GetComponent<Animator>();
				am.ResetTrigger("play");
				am.SetTrigger("play");
			}
			Destroy(gameObject);
		}
	}
}

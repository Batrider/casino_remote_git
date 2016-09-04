using UnityEngine;
using System.Collections;

public class GoldFlyAE : MonoBehaviour {
	float speedX = 80;
	float amG = 10;
	float timeG = 0;
    public AudioClip goldClip;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles += new Vector3 (0,0,-Time.deltaTime*360);
		if(transform.localPosition.x<-460)
			transform.localPosition += new Vector3(speedX*Time.deltaTime,0,0);
		else
		{
			timeG += Time.deltaTime;
			transform.localPosition += new Vector3(speedX*Time.deltaTime,-0.5f*amG*timeG*timeG,0);
		}
		if(transform.localPosition.y<-230)
		{
			if(GameObject.FindWithTag("Credits"))
			{
				Animator am = GameObject.FindWithTag("Credits").GetComponent<Animator>();
                GameObject.FindWithTag("Credits").GetComponent<AudioSource>().PlayOneShot(goldClip);
				am.Play("CreditsAnim");
			}
			Destroy(gameObject);
		}
	}
}

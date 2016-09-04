using UnityEngine;
using System.Collections;

public class GoldFly : MonoBehaviour {
	int step = 1;
	float t1;
	float t2;
	float t3;
	float vy;
	public float vx = -4;
	float a = -10;
	float sx ;
    public AudioClip ac;
	// Use this for initialization
	void Start () {
		t1 = 0;
		t2 = 0;

//		rigidbody2D.AddForce(new Vector2(-5,5));
	}
	void FixedUpdate()
	{
		if(step == 1)
		{
			//物理公式
			float sy = 0.5f*a*t1*t1;
			sx = vx*t1;
			transform.localPosition = transform.localPosition + new Vector3(sx,sy,0);
			t1+=Time.fixedDeltaTime;
        //    t1+=RealTime.deltaTime;
			if(transform.localPosition.y<-190)
			{
                GetComponent<AudioSource>().PlayOneShot(ac);
				step = 2;
				vy = a*t1;
			}
		}
		else if(step == 2)
		{
			float sy = 0.5f*a*t2*t2;

			transform.localPosition = transform.localPosition + new Vector3(sx,-0.26f*vy*t1+5f*sy,0);
     //       t2+=RealTime.deltaTime;
            t2+=Time.fixedDeltaTime;
			if(transform.localPosition.y<-210)
			{
                GetComponent<AudioSource>().PlayOneShot(ac);
				step =3;
//				vy = a*(t2-t1);
			}
		}
		else if(step == 3)
		{
			float sy = 0.5f*a*t3*t3;

			transform.localPosition = transform.localPosition + new Vector3(sx,-0.22f*vy*t1+10f*sy,0);
         //   t3+=RealTime.deltaTime;
           t3+=Time.fixedDeltaTime;
		}
		transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y+1000*Time.fixedDeltaTime,0);

		if(transform.localPosition.y<-300)
		{
			Animator am = GameObject.FindWithTag("Credits").GetComponent<Animator>();
			GameObject.FindWithTag("Credits").GetComponent<AudioSource>().Play();
	//		am.SetTrigger("add");
			Destroy(gameObject);
		}
	}
	//金币特效
	
//	public void GoldFlyEffect()
//	{
//		Object gold =  Resources.Load("ChinaStyle/Gold");
//		for(int i = 0;i<20;i++)
//		{
//			GameObject goldPre = Instantiate(gold) as GameObject;
//			goldPre.transform.localPosition = GameObject.FindWithTag("goldInstatiatePosition").transform.localPosition + new Vector3(Random.Range(2f,5f),Random.Range(0f,3f),0);
//			goldPre.transform.localScale = Vector3.one;
//			goldPre.transform.localEulerAngles = Vector3.zero;
//
//			
//		}
//	}
}

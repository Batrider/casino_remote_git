using UnityEngine;
using System.Collections;

public class LionShow : MonoBehaviour {
    Vector3 vPoint = new Vector3(55.52f,-0.65f,0);
    Vector3 vPoint2 = new Vector3(47.99f,-0.65f,0);
    public AudioClip walkClip;
    public AudioClip shoutClip;
    public float exitTime = 0;
    private AudioSource ac;
	void Start (){
        GetComponent<Animator>().SetTrigger("MINORJACKPOT");
        GetComponent<Animator>().SetTrigger("WIN");
        ac = GetComponent<AudioSource>();
        GameObject.FindGameObjectWithTag("door").GetComponent<Animator>().SetTrigger("open");
        StartCoroutine(SoundSet());
	}
	
	// Update is called once per frame
	void Update () {
        exitTime += Time.deltaTime;
        if (exitTime < 10f)
        {
            transform.position = Vector3.MoveTowards(transform.position, vPoint, 0.95f * Time.deltaTime);
            if (Vector3.Distance(transform.position, vPoint) < 5f && Vector3.Distance(transform.position, vPoint) > 0.05f)
            {
                transform.localEulerAngles -= new Vector3(0, 8 * Time.deltaTime, 0);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, vPoint2, 1f * Time.deltaTime);

        }
        if (exitTime > 16f)
        {
            GameObject.FindGameObjectWithTag("door").GetComponent<Animator>().SetTrigger("close");
            Destroy(gameObject);
        }

	}
    IEnumerator SoundSet()
    {
        while (true)
        {
            if (exitTime > 7.2f)
            {
                ac.clip = shoutClip;
                if (!ac.isPlaying)
                {
                    ac.loop = false;
                    ac.PlayOneShot(shoutClip);
                    break;
                }
            }
            yield return new WaitForSeconds(0.02f);
            
        }
        while (true)
        {
            if(exitTime<7f||exitTime>10f)
            {
                ac.clip = walkClip;
                if (!ac.isPlaying)
                {
                    ac.Play();
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}

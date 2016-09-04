using UnityEngine;
using System.Collections;
//中MEGAJACKPOT的动画及特效
public class DragonFly : MonoBehaviour {
    private Vector3 point1;
    private Vector3 point2;
    public GameObject dragonFire;
    public float speed = 2;
    public AudioClip fireShoot;
    public AudioClip flyClip;
        // Use this for initialization
	void Start () {
        GetComponent<Animator>().SetTrigger("MEGAJACKPOT");
        GetComponent<AudioSource>().clip = flyClip;
        GetComponent<Animator>().speed = 1.5f;
        point1 = new Vector3(27.35f, 0.7f, -4.78f);
        point2 = new Vector3(26.047f, 0.9f, -5f);
        StartCoroutine(updateLoop());
	}
    IEnumerator updateLoop()
    {
        while (true)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, point1, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, point1) < 0.005f)
            {
                transform.position = point1;
                FireShoot();
                break;
                // gameObject.GetComponent<DragonFly>().enabled = false;
            }
        }
    }
    void FireShoot()
    {
        GetComponent<Animator>().SetTrigger("WIN");
        StartCoroutine(InsFire());
        StartCoroutine(PlaySound());
    }
    IEnumerator InsFire()
    {
        yield return new WaitForSeconds(1f);
        dragonFire.SetActive(true);
        StartCoroutine(HouseEffect());
        yield return new WaitForSeconds(2.3f);
        dragonFire.SetActive(false);
    }
    IEnumerator HouseEffect()
    {
        GameObject house = GameObject.Find("UI Root/BG/OuZhouXiaoZhen/Fangzi1");
        MeshRenderer hm= house.GetComponent<MeshRenderer>();
        Color c = hm.material.color;
        while (true)
        {
            c.g = Mathf.Lerp(c.g,0.3f,Time.deltaTime);
            c.b = Mathf.Lerp(c.b,0.3f,Time.deltaTime);
            hm.material.color = c;
            if(c.g<0.31f)
            {
                StartCoroutine(ResumeColor());
                StartCoroutine(EndEffect());
                break;
            }
            yield return null;
        }
    }
    //恢复房屋颜色
    IEnumerator ResumeColor()
    {
        GameObject house = GameObject.Find("UI Root/BG/OuZhouXiaoZhen/Fangzi1");
        MeshRenderer hm= house.GetComponent<MeshRenderer>();
        Color c = hm.material.color;
        while (true)
        {
            c.g = Mathf.Lerp(c.g,1f,0.8f*Time.deltaTime);
            c.b = Mathf.Lerp(c.b,1f,0.8f*Time.deltaTime);
            hm.material.color = c;
            if(c.g>0.99)
            {
                break;
            }
            yield return null;
        }
    }
    //特效结束
    IEnumerator EndEffect()
    {
        GetComponent<AudioSource>().clip = flyClip;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.2f);
        GetComponent<Animator>().speed = 2f;
        while (true)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, point2, 0.3f*speed * Time.deltaTime);
            transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles,Vector3.zero, 10f*speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, point2) < 0.005f)
            {
                Destroy(gameObject);
            }
        }
    }
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().clip = fireShoot;
        GetComponent<AudioSource>().PlayOneShot(fireShoot);
        
    }

}

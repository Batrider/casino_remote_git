using UnityEngine;
using System.Collections;

public class NewBoss : MonoBehaviour {
    Vector3 vPoint1 = new Vector3(26.844f,0.609f,-5f);
    Vector3 vPoint2 = new Vector3(26.844f,1.29f,-5f);
	// Use this for initialization
    void Start () {
        GetComponent<Animator>().SetTrigger("MINIJACKPOT");
        StartCoroutine(updateLoop());
    }
    IEnumerator updateLoop()
    {
        while (true)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, vPoint1, 2 * Time.deltaTime);
            if (Vector3.Distance(transform.position, vPoint1) < 0.005f)
            {
                Attack();
                break;
            }
        }
    }
    void Attack()
    {
        GetComponent<Animator>().SetTrigger("WIN");
        StartCoroutine(updateLoop2());
        
    }
    IEnumerator updateLoop2()
    {
        yield return new WaitForSeconds(4f);
        while (true)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, vPoint2, 2 * Time.deltaTime);
            if (Vector3.Distance(transform.position, vPoint2) < 0.005f)
            {
                break;
            }
        }
    }
}

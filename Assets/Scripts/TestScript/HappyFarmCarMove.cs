using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HappyFarmCarMove : MonoBehaviour {
    private int tag = 1;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    [ContextMenu("start")]
    void startrun()
    {
        StartCoroutine(CarStart(tag));
    }
    public IEnumerator CarStart(int number)
    {
        while(number>0)
        {
            number--;
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Debug.Log("tag:"+tag);
            if(tag == 16)tag = 2;
            if(tag == 2||tag == 4||tag == 10|| tag ==15)
            {
                transform.localScale = new Vector3(-transform.localScale.x,1,1);
            }

            GetComponent<iTweenEvent>().pathName1 = "path"+tag;
            GetComponent<iTweenEvent>().Play();
            tag++;
            yield return new WaitForSeconds(2f);
        }
    }

}

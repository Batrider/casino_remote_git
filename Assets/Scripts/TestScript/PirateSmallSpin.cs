using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PirateSmallSpin : MonoBehaviour {
    public Object labelPrefabs;
    public int tag = 1;
    public AudioClip audioWave;
    public AudioClip audioReward;
    public void paly(int number)
    {
        StartCoroutine(ShipStart(number));
    }
    IEnumerator ShipStart(int number)
    {
        while(number>0)
        {
            number--;
            audio.PlayOneShot(audioWave);
 //           string pathName = GetComponent<iTweenEvent>().pathName1;
 //           int tag =int.Parse(pathName.Substring(4));
            Debug.Log("tag:"+tag);
            if(tag == 15)tag = 1;
            GetComponent<iTweenEvent>().pathName1 = "path"+tag;
            GetComponent<iTweenEvent>().Play();
            tag++;
            yield return new WaitForSeconds(2f);
        }
    }
    public void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(labelPrefabs) as GameObject;
        labelObj.transform.parent = transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = GameObject.FindGameObjectWithTag("SgCamera").GetComponentInChildren<Sg_Pirate>().smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(-73,120,0);
        tp.to = new Vector3(-73,175,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();

        audio.PlayOneShot(audioReward);
        
    }

}

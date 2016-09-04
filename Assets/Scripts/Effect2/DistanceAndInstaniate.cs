using UnityEngine;
using System.Collections;

public class DistanceAndInstaniate : MonoBehaviour {
    public Object AddLabel;
    public Object caiDai;
    public GameObject lantern;
     public float time = 6;
    void Start()
    {
        StartCoroutine(waitToplay());
    }
    IEnumerator waitToplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            //间隔5秒，且距离小于1才能触发动画
            if(Vector3.Distance(transform.parent.localPosition,lantern.transform.localPosition)<20&&time>5)
            {
                time = 0;
                StartCoroutine(CaidaiIns());
                AddMethod();

            }
            time+=0.02f;
        }
    }
    IEnumerator CaidaiIns()
    {
        GetComponent<UISpriteAnimation>().enabled = true;
        GetComponent<UISpriteAnimation>().ResetToBeginning();
        yield return null;
        for(int i = 0;i<40;i++)
        {
            yield return null;
            GameObject  gold = Instantiate(caiDai) as GameObject;
            gold.transform.parent = transform.parent;
            gold.transform.localScale = new Vector3(100,100,100);
            gold.transform.eulerAngles = Vector3.zero;
            gold.transform.localPosition = new Vector3(Random.Range(-300,300),Random.Range(-200,200),0);
            
            gold.GetComponent<UISprite>().spriteName = "caidai_"+Random.Range(1,14);
            gold.GetComponent<UISprite>().MakePixelPerfect();
        }
        yield return new WaitForSeconds(10);
        GetComponent<UISprite>().spriteName = "ball01";
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(AddLabel) as GameObject;
        labelObj.transform.parent =this.transform.parent;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = GameObject.FindGameObjectWithTag("SgCamera").GetComponentInChildren<Sg_ChinaStyle>().smallGameTimesLabel.bitmapFont;
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,-200,0);
        tp.to = new Vector3(0,100,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void AddMethod()
    {
        SmallGame sgScript = GetComponentInParent<SmallGame>();
        AddMoney(sgScript.money_small);
        sgScript.smallPrizeLabel.text = (double.Parse(sgScript.smallPrizeLabel.text)+sgScript.money_small).ToString();
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Bigshot : SmallGame {
//  public GameObject helpObj;
    public Object addMoneyLabel;
    public GameObject[] people;
    public GameObject playbtn;
    int index1;
    int index2;
    bool isAnimation = false;
    GameObject peo1;
    GameObject peo2;
	void Start()
	{
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        peo1 = new GameObject("temp");
        peo2 = new GameObject("temp2");
	}
	void Update()
	{
		//收到小游戏的返回消息
		if(reciveMsgOfSmallGame)
		{
			reciveMsgOfSmallGame = false;
			StartCoroutine(SmallGameEffect());
		}
	}
	public void ShowTheSmallGameResult(int index1,int index2,double prize)
	{
		Debug.Log("This is the result:");
        this.index1 = index1-1;
        Debug.Log("index1:" + index1);
        this.index2 = index2-1;
        Debug.Log("index2:" + index2);
		money_small = prize;
		reciveMsgOfSmallGame = true;
	}
	//进入小游戏
	IEnumerator SmallGameEffect()
	{
        StartCoroutine(PeopleSelcetAnimation());
        yield return new WaitForSeconds(3.8f);
        if (int.Parse(smallGameTimesLabel.text) > 0)
        {
            //可用次数降低，金额增加
            AddMoney(money_small);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            
            
            //选定女郎
            peo1.SetActive(false);
            peo2.SetActive(false);
            people[index1].SetActive(true);
            people[index2].SetActive(true);
            aso.PlayOneShot(bigPrizeClip);
            yield return new WaitForSeconds(2f);
        }
        if (int.Parse(smallGameTimesLabel.text) == 0)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        } else
        {
            yield return new WaitForSeconds(0.5f);
            people[index1].SetActive(false);
            people[index2].SetActive(false);
            SmallGame.canSelect = true;
            playbtn.GetComponent<BoxCollider>().enabled = true;
            playbtn.GetComponent<UIButton>().state = UIButtonColor.State.Normal;
        }
    }
	public override void ConcretMethod ()
	{
        PlayerPrefs.SetInt("BigshotFlag", 1);
	}
    public void SelectBtnMessage()
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            playbtn.GetComponent<BoxCollider>().enabled = false;
            playbtn.GetComponent<UIButton>().state = UIButtonColor.State.Disabled;
            networkConnect.GAME_Roulette12();
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    IEnumerator PeopleSelcetAnimation()
    {
        int indexTemp1;
        int indexTemp2;
        float timeCount = 0.04f;
        while(timeCount<0.18f)
        {
            timeCount = timeCount*1.05f;
            indexTemp1 = Random.Range(0, 6);
            indexTemp2 = Random.Range(6, 12);
            peo1 = people [indexTemp1];
            peo2 = people [indexTemp2];
            peo1.SetActive(true);
            peo2.SetActive(true);
            yield return new WaitForSeconds(timeCount);
            peo1.SetActive(false);
            peo2.SetActive(false);
         }
         aso.PlayOneShot(tapClip);
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =this.transform;
        labelObj.transform.localScale =2*Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,300,0);
        tp.to = new Vector3(0,450,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void OnEnable()
    {
        NetworkConnect.sg_roulette12_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_roulette12_Callback -= ShowTheSmallGameResult;
    }

}

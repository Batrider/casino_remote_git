using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_IceWorld : SmallGame {

    public Object iceCake;
    public Object tool;
    public Object bigPrize;
    public Object smallPrize;
    public Object addMoneyLabel;

    public Transform iceCakeParent;
	public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public AudioClip iceBreak;


    private int curCareIndex = 0;
    private GameObject curIceCake;
	void Start()
	{
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        smallGameTipsManager.finger.SetActive(false);
        StartCoroutine(waitForCloseTip());
    }
    IEnumerator waitForCloseTip()
    {
        while (smallGameTipsManager.tipBox.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(InstaniateIceCake());
        yield return new WaitForSeconds(1f);
        smallGameTipsManager.finger.SetActive(true);
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
	public void ShowTheSmallGameResult(int index,int tag,double prize)
	{
		Debug.Log("This is the result:");
		index_small = index;
		tag_small = tag;
		money_small = prize;
		Debug.Log("index:"+index+";tag:"+tag+";prize:"+prize);
		reciveMsgOfSmallGame = true;
	}
	//进入小游戏
	IEnumerator SmallGameEffect()
	{
		if(int.Parse(smallGameTimesLabel.text)>0)
		{
			//可用次数降低，金额增加
			smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            //砸冰工具
            GameObject tools = Instantiate(tool) as GameObject;
            tools.transform.parent = curIceCake.transform;
            tools.transform.localScale = Vector3.one;
            tools.transform.localEulerAngles = Vector3.zero;
            tools.transform.localPosition = new Vector3(215,-65,0);
            aso.PlayOneShot(tapClip);
            yield return new WaitForSeconds(0.5f);
            aso.PlayOneShot(iceBreak);
            curIceCake.GetComponentInChildren<Animator>().SetTrigger("break");
            yield return new WaitForSeconds(0.5f);
            curIceCake.GetComponent<UISprite>().enabled = false;
            curIceCake.GetComponentInChildren<UIPanel>().alpha = 0;
			if(tag_small ==1)
			{
                aso.PlayOneShot(smallPrizeClip);

                GameObject small = Instantiate(smallPrize) as GameObject;
                small.transform.parent = curIceCake.transform;
                small.transform.localScale = Vector3.one;
                small.transform.localEulerAngles = Vector3.zero;
                small.transform.localPosition = new Vector3(0,0,0);
			}
			//大奖
			else if(tag_small ==2)
			{
                aso.PlayOneShot(bigPrizeClip);

                GameObject small = Instantiate(bigPrize) as GameObject;
                small.transform.parent = curIceCake.transform;
                small.transform.localScale = Vector3.one;
                small.transform.localEulerAngles = Vector3.zero;
                small.transform.localPosition = tools.transform.localPosition;
			}
            AddMoney(money_small);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                
		}
        
		if(int.Parse(smallGameTimesLabel.text) ==0)
		{
			yield return new WaitForSeconds(2f);
            Destroy(curIceCake);
            
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
		}
        else
        {
            yield return new WaitForSeconds(2f);
            Destroy(curIceCake);
            
            SmallGame.canSelect = true;
        }
	}
	public override void ConcretMethod ()
	{
	}
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            curIceCake = obj;
            networkConnect.GAME_SMALL_5TurnOver(++curCareIndex);
        }
        smallGameTipsManager.CloseFingerTip();
    }
    IEnumerator InstaniateIceCake()
    {
        Vector3 startPos = new Vector3(-430,392,0);

        for(int i = 0;i<3;i++)
        {
            for(int j = 0;j<5;j++)
            {
                GameObject ic = Instantiate(iceCake) as GameObject;
                ic.name = (j + i*5).ToString();
                ic.transform.parent = iceCakeParent;
                ic.transform.localScale = Vector3.one;
                ic.transform.localEulerAngles = Vector3.zero;
                ic.transform.localPosition = startPos + new Vector3(230*j,i*280,0);

                EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
                ed.parameters[0] = new EventDelegate.Parameter(ic);
                EventDelegate.Add(ic.GetComponent<UIButton>().onClick,ed);

                yield return null;
            }
        }        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curIceCake.transform;
        labelObj.transform.localScale =Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(100,0,0);
        tp.to = new Vector3(100,150,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void OnEnable()
    {
        NetworkConnect.sg_5Turnover_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_5Turnover_Callback -= ShowTheSmallGameResult;        
    }
}

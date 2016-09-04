using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_AncientEgyptian : SmallGame {
    public GameObject[] gosLeft;
    public GameObject[] gosRight;
    public GameObject playbtn;
    public Object addMoneyLabel;
    public Object animObj;
    public Object bigWin;
    public Object middleWin;
    public Object smallWin;
    public Object fire;
    private string prizeType1;
    private string prizeType2;
    private GameObject twoGameObject;
    private bool reciveMsgOfSmallGame1 = false;
    private bool reciveMsgOfSmallGame2 = false;
    //服务器端消息回发接口
    void Start()
    {
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();      
    }

    public void ShowSG_L_Result(string str)
    {
        Debug.Log(str);
        prizeType1 = str;
        reciveMsgOfSmallGame1 = true;
        
    }
    public void ShowSG_R_Result(string str,double prize)
    {
        Debug.Log(str);
        prizeType2 = str;
        money_small = prize;
        reciveMsgOfSmallGame2 = true;
    }
    void Update()
    {
        if(reciveMsgOfSmallGame1)
        {
            reciveMsgOfSmallGame1 = false;
            LeftDiskEffect();
            GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_EGYPTIAN(5019);
        }
        //收到小游戏的返回消息
        if(reciveMsgOfSmallGame2)
        {
            reciveMsgOfSmallGame2 = false;
            //开始动画
            StartCoroutine(RightDiskEffect());
        }
    }
    //左
    public void LeftDiskEffect()
    {
        // small prize
        if(prizeType1=="65")
        {
            aso.PlayOneShot(smallPrizeClip);
            int index = Random.Range(1,5);
            gosLeft[index].GetComponent<TweenPosition>().enabled = false;
            gosLeft[index].GetComponent<AESgLerpToTarget>().startMove();
        }
        else
        {
            aso.PlayOneShot(bigPrizeClip);
            gosLeft[0].GetComponent<TweenPosition>().enabled = false;
            gosLeft[0].GetComponent<AESgLerpToTarget>().startMove();
        }
    }
    //右特效显示及中奖金额增加效果
    IEnumerator RightDiskEffect()
    {
        smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
        smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
        smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
        
        
        if(prizeType2=="65")
        {
            aso.PlayOneShot(smallPrizeClip);
            int index = Random.Range(1,5);
            gosRight[index].GetComponent<TweenPosition>().enabled = false;
            gosRight[index].GetComponent<AESgLerpToTarget>().startMove();
        }
        else
        {
            aso.PlayOneShot(bigPrizeClip);
            gosRight[0].GetComponent<TweenPosition>().enabled = false;
            gosRight[0].GetComponent<AESgLerpToTarget>().startMove();
        }
        yield return new WaitForSeconds(1.5f);
        //生成动画
        GameObject animobj = Instantiate(animObj) as GameObject;
        animobj.transform.parent = transform;
        animobj.transform.localPosition = new Vector3(1806,-136,-53);
        animobj.transform.localEulerAngles = Vector3.zero;
        animobj.transform.localScale = new Vector3(80,80,80);
        yield return new WaitForSeconds(2f);
        //弹出中奖动画
        //…………
        if(prizeType1=="65"&&prizeType2=="65")
        {
            //大奖
            GameObject bigW = Instantiate(bigWin) as GameObject;
            bigW.transform.parent = transform;
        }
        else if(prizeType1!="65"&&prizeType2!="65")
        {
            //小奖
            GameObject smallW = Instantiate(smallWin) as GameObject;
            smallW.transform.parent = transform;
        }
        else 
        {
            //中等奖励
            GameObject midW = Instantiate(middleWin) as GameObject;
            midW.transform.parent = transform;
        }
        GameObject firee = Instantiate(fire) as GameObject;
        firee.transform.parent = transform;
        firee.transform.localEulerAngles = Vector3.zero;
        firee.transform.localPosition = new Vector3(0,-58,0);
        firee.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        AddMoney(money_small);
        smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+money_small).ToString();
        if(int.Parse(smallGameTimesLabel.text) ==0)
        {
            //游戏结束
            PlayerPrefs.SetInt("ancientEgyptianFlag", 1);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        }
        else
        {
            //可以继续下一轮
            StartCoroutine(Next_Hand());
            
        }
    }
    //恢复游戏默认状态
    IEnumerator Next_Hand()
    {
        yield return new WaitForSeconds(1.5f);
        
        playbtn.GetComponent<UIButton>().isEnabled = true;
        canSelect = true;
    }
    public void SelectBtnMessage()
    {
        SmallGameTipsManager.timer = 0;
        if(canSelect)
        {
            
            playbtn.GetComponent<UIButton>().isEnabled = false;
            canSelect = false;
            networkConnect.GAME_SMALL_EGYPTIAN(5018);
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =this.transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,-50,0);
        tp.to = new Vector3(0,250,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void OnEnable()
    {
        NetworkConnect.sg_Egyptian_L_Callback += ShowSG_L_Result;
        NetworkConnect.sg_Egyptian_R_Callback += ShowSG_R_Result;
    }
    void OnDisable()
    {
        NetworkConnect.sg_Egyptian_L_Callback -= ShowSG_L_Result;
        NetworkConnect.sg_Egyptian_R_Callback -= ShowSG_R_Result;
    }

    /*
    [HideInInspector]
	public GameObject targetObj;
    [HideInInspector]
    public GameObject targetObj2;
    
	private string prizeType;
	private bool reciveMsgOfSmallGame1 = false;
	private bool reciveMsgOfSmallGame2 = false;
	public GameObject playBtn;
	
	public GameObject chooseBtn1;
	public GameObject chooseBtn2;
    public GameObject helpObj;
	void Start()
	{
		//小游戏请求返回的绑定函数
		GameObject.Find("/GameManager").GetComponent<NetworkConnect>().sg_Egyptian_L_Callback += ShowSG_L_Result;
		GameObject.Find("/GameManager").GetComponent<NetworkConnect>().sg_Egyptian_R_Callback += ShowSG_R_Result;
        if (PlayerPrefs.GetInt("ancientEgyptianFlag") == 0)
            helpObj.SetActive(true);
    }
	void Update()
	{
		if(reciveMsgOfSmallGame1)
		{
			reciveMsgOfSmallGame1 = false;
			LeftDiskEffect(targetObj);
            //请求第二个轮盘消息
            transform.GetComponent<CZSelect>().SelectBtnMessage(targetObj2);
		}
		//收到小游戏的返回消息
		if(reciveMsgOfSmallGame2)
		{
			reciveMsgOfSmallGame2 = false;

			RightDiskEffect();
		}
	}
    //请求第一个轮盘消息
    public void AskForMessage()
    {
        transform.GetComponent<CZSelect>().SelectBtnMessage(targetObj);
    }

	//服务器端消息回发接口
	public void ShowSG_L_Result(string str)
	{
		Debug.Log(str);
		prizeType = str;
		reciveMsgOfSmallGame1 = true;

	}
	public void ShowSG_R_Result(string str,double prize)
	{
		Debug.Log(str);
		prizeType = str;
		money_small = prize;
		reciveMsgOfSmallGame2 = true;
	}
	//这一轮游戏结束，显示这一轮游戏的获奖金额

	//左转盘虫子按钮特效显示
	public void LeftDiskEffect(GameObject tarObj)
	{
		// small prize
		if(prizeType=="65")
		{
			aso.PlayOneShot(smallPrizeClip);
            tarObj.GetComponent<UISprite>().spriteName = "red";
		}
		else
		{
			aso.PlayOneShot(bigPrizeClip);
            tarObj.GetComponent<UISprite>().spriteName = "green";
		}
	}
	//右转盘按钮特效显示及中奖金额增加效果
	public void RightDiskEffect()
	{
		LeftDiskEffect(targetObj2);
		//预留特效代码
		smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+money_small).ToString();
		if(int.Parse(smallGameTimesLabel.text) ==0)
		{
			//游戏结束
            PlayerPrefs.SetInt("ancientEgyptianFlag", 1);
			StartCoroutine(Small_Over());
		}
		else
		{
			//可以继续下一轮
			StartCoroutine(Next_Hand());
		}
	}
	//恢复游戏默认状态
	IEnumerator Next_Hand()
	{
		yield return new WaitForSeconds(1.5f);
		playBtn.GetComponent<BoxCollider>().enabled = true;
        if (PlayerPrefs.GetInt("ancientEgyptianFlag") == 0)
            helpObj.SetActive(true);
		UISprite[] uss1 = chooseBtn1.GetComponentsInChildren<UISprite>();
		foreach(UISprite us in uss1)
		{
			if(us.spriteName == "red"||us.spriteName =="green")
				us.spriteName = "yellow";
		}
		UISprite[] uss2 = chooseBtn2.GetComponentsInChildren<UISprite>();
		foreach(UISprite us in uss2)
		{
			if(us.spriteName == "red"||us.spriteName =="green")
				us.spriteName = "yellow";
		}
	}
	public override void ConcretMethod ()
	{
		base.ConcretMethod ();
		StartCoroutine(Next_Hand());
	}
	public void HitPlay(GameObject play)
	{
        helpObj.SetActive(false);
		play.GetComponent<AudioSource>().Play();
		playBtn.GetComponent<BoxCollider>().enabled = false;
		smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
	}
 */   
}

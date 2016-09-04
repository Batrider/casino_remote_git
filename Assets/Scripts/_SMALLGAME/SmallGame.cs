using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallGame : MonoBehaviour {
	[HideInInspector]
	public GameObject[] numsObj;
	//小游戏奖金，与小游戏剩余次数
	public UILabel smallPrizeLabel;
    protected double smallTotalWin;
	public UILabel smallGameTimesLabel;
	//本轮游戏的金钱、标识、索引
    [HideInInspector]
	public double money_small;
	protected int tag_small;
	protected bool reciveMsgOfSmallGame = false;
    private bool reciveAutoSmallPrize = false;
    //选择按钮的序号
	public static int index_small;
	public static bool canSelect = true;
    public static int valueOfTime;
	//小游戏声音对象
	public AudioSource aso;
	public AudioClip tapClip;
	public AudioClip bigPrizeClip;
	public AudioClip smallPrizeClip;
	
	public Dictionary<int,GameObject> btn_smalls = new Dictionary<int, GameObject>();
	public Dictionary<int,GameObject> num_smalls = new Dictionary<int, GameObject>();

    protected NetworkConnect networkConnect;
    protected SmallGameTipsManager smallGameTipsManager;

    public void InitSmallGame()
    {
        SmallGameTipsManager.timer = 1000;
        smallTotalWin = 0;
        SmallGame.canSelect = true;
        smallGameTimesLabel.text = valueOfTime.ToString();
        networkConnect = GameObject.Find("/GameManager").GetComponent<NetworkConnect>();
        smallGameTipsManager = GameObject.FindGameObjectWithTag("SgHelp").GetComponent<SmallGameTipsManager>();
        smallGameTipsManager.CheckTip();

    }

    //自动游戏中的小游戏模式
    public IEnumerator AutoSmallGame()
    {
        canSelect = false;
        smallGameTimesLabel.text = valueOfTime.ToString();
        NetworkConnect.smallTotalPrize += GetSmallTotalPrize;
        GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALLPrizeRequire();
        while (!reciveAutoSmallPrize)
        {
            yield return null;
        }
        StartCoroutine(Small_Over(smallTotalWin));
    }
    public void GetSmallTotalPrize(double smallPrize)
    {
        this.smallTotalWin = smallPrize;
        reciveAutoSmallPrize = true;
        NetworkConnect.smallTotalPrize -= GetSmallTotalPrize;
    }

    public void SmallGame_Num(double num)
	{
        GameObject endAnimation = Instantiate(Resources.Load("SmallGameEndAmination")) as GameObject;
        endAnimation.transform.parent =this.transform;
        endAnimation.transform.localPosition = Vector3.zero;
        endAnimation.transform.localScale = Vector3.one;
        endAnimation.transform.localEulerAngles = Vector3.zero;
         
        endAnimation.GetComponentInChildren<PrizeCount>().StartAdd( num);
        /* 显示钱的旧方法
        GameObject OffsetObj =small_GameAmin.transform.FindChild("add").gameObject;
        OffsetObj.GetComponent<UISprite>().width = 80;
        OffsetObj.GetComponent<UISprite>().height = 77;
        OffsetObj.transform.localPosition = new Vector3(-40*num.Length,0,0);
        Vector3 offsetChar = Vector3.zero;
		Debug.Log("Instantiate nums"+"!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		foreach(char n in num)
		{
            offsetChar += new Vector3(80,0,0);

			GameObject numIocn = Instantiate(Resources.Load("__Normal/nums")) as GameObject;
			numIocn.name = n.ToString();
			if(n=='.')
			{
				numIocn.GetComponent<UISprite>().spriteName ="e";
			}
			else
			{
				numIocn.GetComponent<UISprite>().spriteName = n.ToString();
			}
            numIocn.transform.parent = OffsetObj.transform;
			numIocn.transform.localScale = Vector3.one;
			numIocn.transform.eulerAngles = Vector3.zero;
            numIocn.transform.localPosition = offsetChar;
            num_smalls.Add(num_smalls.Count,numIocn);
		}
  */      
	}
	public IEnumerator Small_Over(double winMoney)
	{
        SmallGameTipsManager.timer = 1000;
        yield return new WaitForSeconds(0.5f);
		//显示小游戏结束画面
        double deltaMoneyAdd =0.05*winMoney;
        /*
        for(double money = 0;money<winMoney;)
        {
            SmallGame_Num(money.ToString("f1"));
            money+=deltaMoneyAdd;
            yield return new WaitForSeconds(0.05f);
            foreach(KeyValuePair<int,GameObject> num in num_smalls)
            {
                Destroy(num.Value);
            }
        }
        */
        SmallGame_Num(winMoney);
        yield return new WaitForSeconds(4f);
		//删除数字
		foreach(KeyValuePair<int,GameObject> num in num_smalls)
		{
			Destroy(num.Value);
		}
		num_smalls.Clear();
        //将赢取的金钱总额加入账户里面
        GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkConnect>().GAME_Balance_Refresh();
        GameObject.FindWithTag("GameManager").GetComponent<manager>().AddMoney(winMoney,1);
		//返回游戏
		GameObject.FindWithTag("SgCamera").GetComponent<SmallGameChange>().ReturnToGame();
        ConcretMethod();
        yield return new WaitForSeconds(2f);
		canSelect = true;
    }
    public virtual void ConcretMethod()
	{
		Debug.Log("put subclass method here.");
	}









}

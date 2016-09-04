using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Holidaybikini : SmallGame {
    public GameObject[] fiveLantern = new GameObject[5];
    private GameObject curUmbrella;
    public Object addMoneyLabel;
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    private Vector3[] startPoint = new Vector3[5];
    private Color[] starColor = new Color[5];
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
        if (int.Parse(smallGameTimesLabel.text) > 0)
        {
            //可用次数降低，金额增加
            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();

            //小奖
            if (tag_small == 1)
            {
                aso.clip = smallPrizeClip;
                aso.Play();
            }
            //大奖
            else if (tag_small == 2)
            {
                aso.clip = bigPrizeClip;
                aso.Play();
            }
            curUmbrella.transform.FindChild("shadow").GetComponent<UISprite>().enabled = false;
            TweenPosition.Begin(curUmbrella.transform.FindChild("Umbrella").gameObject, 0.5f, curUmbrella.transform.FindChild("Umbrella").localPosition + new Vector3(2000, 500));
            TweenScale.Begin(curUmbrella.transform.FindChild("Umbrella").gameObject, 0.5f, 0.3f*Vector3.one);

            curUmbrella.transform.FindChild("girl").GetComponent<UISprite>().enabled = true;

            AddMoney(money_small);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
            //等待灯笼上升
            yield return new WaitForSeconds(0.5f);
        }
		if(int.Parse(smallGameTimesLabel.text) ==0)
		{
			yield return new WaitForSeconds(2f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
		}
        else
        {
            SmallGame.canSelect = true;
        }
	}
	public override void ConcretMethod ()
	{
	}
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent = curUmbrella.transform.FindChild("girl").transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;

        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;


        labelObj.GetComponent<UILabel>().text = "+" + moneyCount.ToString();

        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0, 150, 0);
        tp.to = new Vector3(0, 350, 0);
        tp.enabled = true;
        tp.PlayForward();

        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            curUmbrella = obj;
            curUmbrella.transform.FindChild("shadow").GetComponent<BoxCollider>().enabled = false;

            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
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

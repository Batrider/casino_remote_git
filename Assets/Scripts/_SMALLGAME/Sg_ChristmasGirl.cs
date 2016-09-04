using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_ChristmasGirl : SmallGame {
    public GameObject[] fiveBox = new GameObject[5];
    public Object addMoneyLabel;
    public GameObject curBox;
    public Object giftPrefab;
    private double winMoney;

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
        if (reciveMsgOfSmallGame)
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
            GameObject gift = Instantiate(giftPrefab) as GameObject;
            gift.transform.parent = curBox.transform.parent;
            gift.transform.localPosition = curBox.transform.localPosition + new Vector3(0, 400, 0);
            gift.transform.localEulerAngles = Vector3.zero;
            gift.transform.localScale = Vector3.zero;
            gift.transform.GetComponent<UISprite>().spriteName = "B" + Random.Range(1, 6);
            gift.transform.GetComponent<UISprite>().MakePixelPerfect();

            yield return new WaitForSeconds(1f);
        }
        AddMoney(money_small);
        smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+ money_small).ToString();
        if (int.Parse(smallGameTimesLabel.text) ==0)
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
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            obj.GetComponent<Animator>().SetTrigger("open");
            SmallGame.canSelect = false;
            curBox = obj;
            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent = curBox.transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;

        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;


        labelObj.GetComponent<UILabel>().text = "+" + moneyCount.ToString();

        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0, 50, 0);
        tp.to = new Vector3(0, 300, 0);
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

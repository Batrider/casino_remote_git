using UnityEngine;
using System.Collections;

public class Sg_Pirate : SmallGame {
    public GameObject rudder;
    public GameObject ship;
    public UIButton spinButton;
//	public GameObject helpObj;
    private int diceNum;
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
	public void ShowTheSmallGameResult(int diceNum,int offsetMap,double prize)
	{
        this.diceNum = diceNum;
        money_small = prize;
		reciveMsgOfSmallGame = true;
	}
    IEnumerator  SmallGameEffect()
    {
        //TimeLabel -1;
        smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
        smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
        smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
        
        rudder.GetComponent<RudderRun>().rudderRun(1080+(diceNum-1)*60);
        //每次转轮花5秒时间
        GetComponent<AudioSource>().PlayOneShot(tapClip);
        yield return new WaitForSeconds(5f);
        //船开动转动的diceNum格数
        ship.GetComponent<PirateSmallSpin>().paly(diceNum);
        yield return new WaitForSeconds(diceNum*2f+1);
        //跳出中奖动画
        ship.GetComponent<PirateSmallSpin>().AddMoney(money_small);
        smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
        if(int.Parse(smallGameTimesLabel.text)>0)
        {
            SmallGame.canSelect = true;
            spinButton.enabled = true;
            spinButton.state =  UIButtonColor.State.Normal;
            
            
        }
        else//结束游戏
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
    }

    public override void ConcretMethod ()
    {
        PlayerPrefs.SetInt("PirateTag", 1);
    }
    public void SpinBtnMessage()
    {
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            spinButton.enabled = false;
            spinButton.state = UIButtonColor.State.Disabled;
            networkConnect.GAME_SMALL_Monopoly();
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void OnEnable()
    {
        NetworkConnect.monopoly_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.monopoly_Callback -= ShowTheSmallGameResult;
    }
}

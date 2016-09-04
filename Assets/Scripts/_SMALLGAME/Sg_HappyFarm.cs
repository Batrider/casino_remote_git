using UnityEngine;
using System.Collections;

public class Sg_HappyFarm : SmallGame {
    public GameObject rudder;
    public GameObject car;
    public GameObject pointer;
    public UIButton spinButton;
    public Object labelPrefabs;
    public int diceNum;
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
        
        yield return StartCoroutine(PointRun());
        aso.PlayOneShot(tapClip);
        //船开动转动的diceNum格数
        car.GetComponent<HappyFarmCarMove>().StartCoroutine("CarStart",diceNum);
        yield return new WaitForSeconds(diceNum*2);
        //跳出中奖动画
        AddMoney(money_small);
        aso.PlayOneShot(bigPrizeClip);

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
            aso.PlayOneShot(tapClip);
            networkConnect.GAME_SMALL_Monopoly();
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    public void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(labelPrefabs) as GameObject;
        labelObj.transform.parent = car.transform.parent;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = car.transform.localPosition+new Vector3(-5,100,0);
        tp.to = car.transform.localPosition+new Vector3(-5,160,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
        
    }
    IEnumerator PointRun()//1:0;2:-60;3:-120
    {
        float targetAngle =(7-diceNum)*60;
        if(diceNum<2)
        {
            targetAngle = 0;
        }
        RotateByAngle rba = pointer.GetComponent<RotateByAngle>();
        while(true)
        {
            yield return null;
            rba.angle = new Vector3(0,0,Mathf.Lerp(rba.angle.z,-10,Time.deltaTime));
            if(Mathf.Abs(rba.angle.z+10)<.2f)
            {
                rba.angle = new Vector3(0,0,-10);
                break;
            }
        }
        yield return new WaitForSeconds(1f);
        while(true)
        {
            yield return null;
            if(pointer.transform.localEulerAngles.z>targetAngle)
            {
                break;
            }
        }
        while(true)
        {
            yield return null;
            rba.angle =new Vector3(0,0,-Mathf.Abs(pointer.transform.localEulerAngles.z-targetAngle)/18);
            if(Mathf.Abs(pointer.transform.localEulerAngles.z-targetAngle)<5)
            {
                rba.angle = Vector3.zero;
                break;
            }
        }
        
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

using UnityEngine;
using System.Collections;

public class Sg_Aladdin : SmallGame {
    public Object addMoneyLabel;
    public GameObject lamp1;
    public GameObject lamp2;
    public Object smokeEffect;
    public Object smokeEffectFirst;
    
    private GameObject lamp;
    private bool reciveMsgOfSmallGame0 = false;
    private int timesCount;
    private double winMoney;
    bool isTipShow = false;
    
    void Start()
    {
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        StartCoroutine(waitForCloseTip());
        networkConnect.GAME_SMALL_GuessTheSize();
    }
    //监测
    IEnumerator waitForCloseTip()
    {
        while(smallGameTipsManager.tipBox.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SmokeEffectFirst();
        
    }
    void Update()
    {
        if(reciveMsgOfSmallGame0)
        {
            //显示金额
            reciveMsgOfSmallGame0 = false;
            SetTheBaseMoney();
        }
        if(reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            StartCoroutine(ShowTheSmallGameResult3());
        }
    }
    public void ShowTheSmallGameResult1(double basePrize)
    {
        money_small = basePrize;
        reciveMsgOfSmallGame0 = true;
    }
    public void SetTheBaseMoney()
    {
        winMoney += money_small;
        smallPrizeLabel.text = money_small.ToString();
    }

    public void ShowTheSmallGameResult2(int tag)
    {
        tag_small = tag;
        reciveMsgOfSmallGame = true;
    }
    IEnumerator ShowTheSmallGameResult3()
    {
        //tag = 0失败 tag = 1 成功
        //失败账户加强进去 下一把
        
        if(tag_small == 0)
        {
            lamp.transform.FindChild("smoke").GetComponent<UI2DSpriteAnimation>().enabled = true;
            lamp.transform.FindChild("smoke").GetComponent<UI2DSpriteAnimation>().ResetToBeginning();

            
            TweenColor.Begin(lamp,.3f,Color.gray);
//            manager.user_Account += double.Parse(smallPrizeLabel.text);

            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            if(int.Parse(smallGameTimesLabel.text)>0)
                SetTheBaseMoney();
            
            timesCount = 0;
            aso.PlayOneShot(smallPrizeClip);
            yield return new WaitForSeconds(0.2f);
            lamp.transform.FindChild("smoke/failed").GetComponent<UILabel>().alpha = .65f;

        }else if(tag_small == 1)
        {

            //执行动画
            lamp.transform.FindChild("god").GetComponent<UI2DSpriteAnimation>().enabled = true;
            lamp.transform.FindChild("god").GetComponent<UI2DSpriteAnimation>().ResetToBeginning();
            //
            AddMoney(2*double.Parse(smallPrizeLabel.text));

            winMoney += double.Parse(smallPrizeLabel.text);
            
            smallPrizeLabel.text =(2*double.Parse(smallPrizeLabel.text)).ToString();
            aso.PlayOneShot(bigPrizeClip);
            timesCount++;
            
            //超过三次； - 1;
            if(timesCount==3)
            {
                yield return new WaitForSeconds(2f);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                SetTheBaseMoney();
                timesCount = 0;
            }
        }
        
        yield return new WaitForSeconds(1f);
        TweenColor.Begin(lamp,.3f,Color.white);
        lamp.transform.FindChild("smoke/failed").GetComponent<UILabel>().alpha = 0;
        
        lamp1.GetComponent<UISprite>().alpha = 0f;
        lamp2.GetComponent<UISprite>().alpha = 0f;
        lamp1.GetComponent<UISprite>().spriteName = "d"+(timesCount+1).ToString();
        lamp2.GetComponent<UISprite>().spriteName = "d"+(timesCount+1).ToString();

        if(timesCount>0)
            SmokeEffect();
        else if(int.Parse(smallGameTimesLabel.text)>0)
            SmokeEffectFirst();
            

        yield return new WaitForSeconds(1.5f);
        if(int.Parse(smallGameTimesLabel.text)>0)
        {
            SmallGame.canSelect = true;
        }
        else
        {
            StartCoroutine(Small_Over(winMoney));
        }
    }


    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            lamp = obj;
            SmallGame.canSelect = false;
            networkConnect.GAME_SMALL_GessTheSize2(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =lamp.transform.parent;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(-50,-300,0);
        tp.to = new Vector3(-50,0,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void SmokeEffect()
    {
        GameObject lamp1Particle = Instantiate(smokeEffect) as GameObject;
        lamp1Particle.transform.parent = lamp1.transform;
        lamp1Particle.transform.localScale = Vector3.one;
        lamp1Particle.transform.localEulerAngles = Vector3.zero;
        lamp1Particle.transform.localPosition = new Vector3(-27,-54,0);
        
        GameObject lamp2Particle = Instantiate(smokeEffect) as GameObject;
        lamp2Particle.transform.parent = lamp2.transform;
        lamp2Particle.transform.localScale = Vector3.one;
        lamp2Particle.transform.localEulerAngles = Vector3.zero;
        lamp2Particle.transform.localPosition = new Vector3(-51,-54,0);

        TweenAlpha.Begin(lamp1,0.5f,1);
        TweenAlpha.Begin(lamp2,0.5f,1);
    }
    void SmokeEffectFirst()
    {
        GameObject lamp1Particle = Instantiate(smokeEffectFirst) as GameObject;
        lamp1Particle.transform.parent = lamp1.transform.parent;
        lamp1Particle.transform.localScale = Vector3.one;
        lamp1Particle.transform.localEulerAngles = Vector3.zero;
        lamp1Particle.transform.localPosition = new Vector3(0,-700,0);

        TweenAlpha.Begin(lamp1,0.2f,1);
        TweenAlpha.Begin(lamp2,0.2f,1);
    }

    void OnEnable()
    {
        NetworkConnect.guessTheSize_Callback1+=ShowTheSmallGameResult1;        
        NetworkConnect.guessTheSize_Callback2+=ShowTheSmallGameResult2;
    }
    void OnDisable()
    {
        NetworkConnect.guessTheSize_Callback1-=ShowTheSmallGameResult1;        
        NetworkConnect.guessTheSize_Callback2-=ShowTheSmallGameResult2;
    }
}

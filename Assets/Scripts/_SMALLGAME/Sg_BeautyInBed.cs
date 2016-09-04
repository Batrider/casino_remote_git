using UnityEngine;
using System.Collections;

public class Sg_BeautyInBed : SmallGame {

    public Object addMoneyLabel;
    public GameObject leftScreen;
    public GameObject rightScreen;
    public GameObject leftGirl;
    public GameObject rightGirl;
    
    private GameObject curScreen;
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
            TweenScale tp = curScreen.GetComponent<TweenScale>();
            tp.enabled = true;
            tp.delay = 0;
            tp.PlayReverse();
            rightGirl.GetComponent<UISprite>().enabled = false;
            leftGirl.GetComponent<UISprite>().enabled = false;

            yield return new WaitForSeconds(1.5f);

            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            if(int.Parse(smallGameTimesLabel.text)>0)
                SetTheBaseMoney();
            
            timesCount = 0;
            aso.PlayOneShot(smallPrizeClip);
            tp.PlayForward();
        }
        else if(tag_small == 1)
        {

            TweenScale tp = curScreen.GetComponent<TweenScale>();
            tp.enabled = true;
            tp.delay = 0;
            tp.PlayReverse();
            if (curScreen == leftScreen)
            {
                leftGirl.GetComponent<UISprite>().enabled = true;
                leftGirl.GetComponent<UISprite>().name = "LP" + Random.Range(1, 3);
                rightGirl.GetComponent<UISprite>().enabled = false;
            }
            else
            {
                leftGirl.GetComponent<UISprite>().enabled = false;
                rightGirl.GetComponent<UISprite>().enabled = true;
                rightGirl.GetComponent<UISprite>().name = "RP" + Random.Range(1, 3);
            }
            yield return new WaitForSeconds(1.5f);

            AddMoney(2*double.Parse(smallPrizeLabel.text));
            winMoney += double.Parse(smallPrizeLabel.text);
            
            smallPrizeLabel.text =(2*double.Parse(smallPrizeLabel.text)).ToString();
            aso.PlayOneShot(bigPrizeClip);
            timesCount++;

            tp.PlayForward();

            //超过三次； - 1;
            if(timesCount==3)
            {
                yield return new WaitForSeconds(2f);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                SetTheBaseMoney();
                timesCount = 0;
            }
        }
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
            curScreen = obj;
            SmallGame.canSelect = false;
            networkConnect.GAME_SMALL_GessTheSize2(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curScreen.transform.parent;
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

using UnityEngine;
using System.Collections;

public class Sg_GreenJungle : SmallGame {
    public Object addMoneyLabel;
    public GameObject[] holes;
    public GameObject[] nuts;
    public GameObject[] stones;

    public Vector3[] targetPosition;
    public GameObject squirrel;
    private GameObject hole;
    private bool reciveMsgOfSmallGame0 = false;
    private int timesCount;
    int curIndex;
    private double winMoney;
    
    void Start()
    {
        //如果是自动小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        timesCount = 0;
        networkConnect.GAME_SMALL_GuessTheSize();

        for(int i = 0 ;i<holes.Length;i++)
        {
            holes[i].SetActive(false);
        }
        holes[0].SetActive(true);
        holes[1].SetActive(true);
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
            yield return StartCoroutine(MoveToTargetPos(timesCount+1,false));
            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            if(int.Parse(smallGameTimesLabel.text)>0)
                SetTheBaseMoney();
            
            timesCount = 0;
            yield return new WaitForSeconds(1f);
            if(int.Parse(smallGameTimesLabel.text)>0)
                squirrel.transform.localPosition = targetPosition[0];
            

        }else if(tag_small == 1)
        {
            yield return StartCoroutine(MoveToTargetPos(timesCount+1,true));
            
            AddMoney(2*double.Parse(smallPrizeLabel.text));
            winMoney += double.Parse(smallPrizeLabel.text);
            
            smallPrizeLabel.text =(2*double.Parse(smallPrizeLabel.text)).ToString();
            timesCount++;
            //超过三次； - 1;
            if(timesCount==3)
            {
                yield return new WaitForSeconds(2f);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                SetTheBaseMoney();
                timesCount = 0;
                if(int.Parse(smallGameTimesLabel.text)>0)
                    squirrel.transform.localPosition = targetPosition[0];
            }
        }
        yield return new WaitForSeconds(1f);
        if(int.Parse(smallGameTimesLabel.text)>0)
        {
            SmallGame.canSelect = true;
        }
        else
        {
            StartCoroutine(Small_Over(winMoney));
        }
    }
    IEnumerator FailMethod()
    {
        for(int i = 0 ;i<holes.Length;i++)
        {
            holes[i].GetComponent<BoxCollider>().enabled = true;
            holes[i].SetActive(false);
        }
        if(hole!=null)
            hole.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        if(int.Parse(smallGameTimesLabel.text)>0)
        {
            holes[0].SetActive(true);
            holes[1].SetActive(true);
        }
        
    }
    IEnumerator SucMethod(int curLevel)
    {
        holes[(curLevel-1)*2+1].GetComponent<BoxCollider>().enabled = false;
        holes[(curLevel-1)*2].GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        holes[curLevel*2+1].SetActive(true);
        holes[curLevel*2].SetActive(true);
    }
    IEnumerator MoveToTargetPos(int curLevel,bool success)
    {
        UISpriteAnimation suUA = squirrel.GetComponent<UISpriteAnimation>();

        suUA.enabled = true;
        suUA.ResetToBeginning();
        suUA.loop = true;
        suUA.namePrefix = "cemian";
        Vector3 startPos = squirrel.transform.localPosition;
        float timeCount = 0; 
        while(Vector3.Distance(squirrel.transform.localPosition,targetPosition[int.Parse(hole.name)])>5f)
        {
            timeCount += Time.deltaTime;
            yield return null;
            squirrel.transform.localPosition = Vector3.MoveTowards(startPos,targetPosition[int.Parse(hole.name)],150*timeCount);
        }
        if(success)
        {
            suUA.loop = false;            
            suUA.namePrefix = "xiao";
            suUA.ResetToBeginning();
            nuts[curIndex].SetActive(true);
            nuts[curIndex].GetComponent<ParticleSystem>().Emit(5);
            aso.PlayOneShot(bigPrizeClip);
            yield return new WaitForSeconds(1f);
            nuts[curIndex].SetActive(false);
            
            StartCoroutine(SucMethod(curLevel));
            
        }else
        {
            suUA.loop = false;            
            suUA.namePrefix = "ku";
            suUA.ResetToBeginning();
            stones[curIndex].SetActive(true);
            stones[curIndex].GetComponent<ParticleSystem>().Emit(5);
            aso.PlayOneShot(smallPrizeClip);
            yield return new WaitForSeconds(1f);
            stones[curIndex].SetActive(false);
            
            StartCoroutine(FailMethod());
        }
        suUA.name = "S2";

    }

    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            hole = obj;
            curIndex = (int.Parse(obj.name) -1);
            SmallGame.canSelect = false;
            networkConnect.GAME_SMALL_GessTheSize2(int.Parse(obj.name)%2);
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =hole.transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2.5f;
        tp.from = new Vector3(-50,-50,0);
        tp.to = new Vector3(-50,0,50);
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

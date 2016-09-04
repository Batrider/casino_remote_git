using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Astrospace : SmallGame {
    public Object addMoneyLabel;
    GameObject curObject;
    public GameObject[] fiveShip = new GameObject[5];
    public GameObject[] timeShip = new GameObject[5];
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
        for(int i = 0;i<timeShip.Length;i++)
            timeShip[i].SetActive(int.Parse(smallGameTimesLabel.text)>i);
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
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
            AddMoney(money_small);
            for(int i = 0;i<timeShip.Length;i++)
                timeShip[i].SetActive(int.Parse(smallGameTimesLabel.text)>i);
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
        }
        yield return new WaitForSeconds(2f);
        if (int.Parse(smallGameTimesLabel.text) ==0)
        {
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
            SmallGame.canSelect = false;
            StartCoroutine(FireMethod(obj));
            curObject = obj;
            curObject.GetComponent<BoxCollider>().enabled = false;
            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();

    }
    IEnumerator FireMethod(GameObject obj)
    {
        UISprite us = obj.transform.FindChild("GameObject").GetComponent<UISprite>();
        while(us.fillAmount<1)
        {
            yield return null;
            us.fillAmount += 0.03f;
        }
        us.fillAmount = 0;
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.GetComponent<TweenPosition>().enabled = false;
        UISpriteAnimation usa = obj.GetComponent<UISpriteAnimation>();
        UISprite us2 = obj.GetComponent<UISprite>();
        us2.spriteName = "explode01";
        usa.enabled = true;
        usa.ResetToBeginning();
        us2.width = 600;
        us2.height = 600;
        yield return null;
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curObject.transform;
        labelObj.transform.localScale =Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,50,0);
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
        NetworkConnect.sg_5Turnover_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_5Turnover_Callback -= ShowTheSmallGameResult;        
    }
}

using UnityEngine;
using System.Collections;

public class Sg_Sugar : SmallGame {
    public Object addMoneyLabel;
    public Object sugarObj;
    private GameObject curSugarBox;
    int tagX;
    void Start()
    {
        //如果是自动游戏的话
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
        tagX = tag;
        money_small = prize;
        reciveMsgOfSmallGame = true;
    }
    IEnumerator SmallGameEffect()
    {
        if(int.Parse(smallGameTimesLabel.text)>0)
        {
            GetComponent<AudioSource>().PlayOneShot(tapClip);
            //可用次数降低，金额增加
            smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            
            if(tagX>1)
            {
                StartCoroutine(InstaniateSugar(20));
                GetComponent<AudioSource>().PlayOneShot(bigPrizeClip);
            }
            else
                StartCoroutine(InstaniateSugar(12));

            yield return new WaitForSeconds(2f);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
            AddMoney(money_small);
            if(int.Parse(smallGameTimesLabel.text)>0)
                SmallGame.canSelect = true;
        }
        if(int.Parse(smallGameTimesLabel.text)==0)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        }
    }
    IEnumerator InstaniateSugar(int num)
    {
        yield return null;
        for(int i = 0;i <num;i++)
        {
            GameObject sugar = Instantiate(sugarObj) as GameObject;
            sugar.GetComponent<UISprite>().spriteName = "sugar"+((i%7)+1);
            sugar.transform.SetParent(curSugarBox.transform);
            sugar.transform.localEulerAngles = Vector3.zero;
            sugar.transform.localScale = Vector3.one;
            sugar.transform.localPosition = new Vector3(Random.Range(-71,71),450,0);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public override void ConcretMethod ()
    {
        PlayerPrefs.SetInt("HoroscopeFlag", 1);
    }
    
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            curSugarBox = obj;
            obj.GetComponent<BoxCollider>().enabled = false;
            SmallGame.canSelect = false;
            GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        GameObject.FindGameObjectWithTag("SgHelp").GetComponent<SmallGameTipsManager>().CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curSugarBox.transform;
        labelObj.transform.localScale =1.5f*Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(-20,20,0);
        tp.to = new Vector3(-20,180,0);
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

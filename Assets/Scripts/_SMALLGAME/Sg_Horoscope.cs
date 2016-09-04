using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Sg_Horoscope : SmallGame {
    public Object addMoneyLabel;
    public GameObject[] ho12Object;
    public GameObject playBtn;
    int index1;
    int index2;
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
    public void ShowTheSmallGameResult(int index1,int index2,double prize)
    {
        Debug.Log("This is the result:");
        this.index1 = index1-1;
        Debug.Log("index1:" + index1);
        this.index2 = index2-1;
        Debug.Log("index2:" + index2);
        money_small = prize;
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
            

            for(int i =0;i<ho12Object.Length;i++)
            {
                TweenAlpha hoAl = TweenAlpha.Begin(ho12Object[i],0.5f,1);
                hoAl.style = UITweener.Style.PingPong;
                yield return new WaitForSeconds(0.1f);
            }
            for(int i =0;i<ho12Object.Length;i++)
            {
                if(i!=index1&&i!=index2)
                {
                    TweenAlpha hoAl = TweenAlpha.Begin(ho12Object[i],0.3f,0);
                    hoAl.style = UITweener.Style.Once;
                    yield return new WaitForSeconds(0.1f);
                    Debug.Log("i:"+i);
                }
            }
            /*
            //星座浮现
            TweenAlpha ta1 = TweenAlpha.Begin(ho12Object[index1],0.5f,1);
            ta1.style = UITweener.Style.PingPong;
            GetComponent<AudioSource>().PlayOneShot(bigPrizeClip);

            TweenAlpha ta2 = TweenAlpha.Begin(ho12Object[index2],0.5f,1);
            ta2.style = UITweener.Style.PingPong;
            */
            GetComponent<AudioSource>().volume = 0.1f;
            aso.PlayOneShot(tapClip);
            yield return new WaitForSeconds(3f);
            AddMoney(money_small);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
            yield return new WaitForSeconds(1.5f);
            TweenAlpha.Begin(ho12Object[index1],0.2f,0);
            TweenAlpha.Begin(ho12Object[index2],0.2f,0);
        }
        if (int.Parse(smallGameTimesLabel.text) == 0)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        } else
        {
            playBtn.GetComponent<BoxCollider>().enabled = true;
            playBtn.GetComponent<UIButton>().state =  UIButtonColor.State.Normal;
            SmallGame.canSelect = true;
        }
    }
    public override void ConcretMethod ()
    {
        PlayerPrefs.SetInt("HoroscopeFlag", 1);
    }
    
    public void SelectBtnMessage()
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            playBtn.GetComponent<BoxCollider>().enabled = false;
            playBtn.GetComponent<UIButton>().state =  UIButtonColor.State.Disabled;
            networkConnect.GAME_Roulette12();
        }
        smallGameTipsManager.CloseFingerTip();
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =this.transform;
        labelObj.transform.localScale =1.5f*Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,300,0);
        tp.to = new Vector3(0,450,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void OnEnable()
    {
        NetworkConnect.sg_roulette12_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_roulette12_Callback -= ShowTheSmallGameResult;
    }
}

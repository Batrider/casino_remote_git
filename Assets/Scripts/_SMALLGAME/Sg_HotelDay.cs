using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_HotelDay : SmallGame
{
    public Object addMoneyLabel;
    public Object girlPrefabs;
    private GameObject curGirl;
    public GameObject[] parentBtns = new GameObject[5];
    private GameObject[] girls = new GameObject[5];
     
    int count = 0;
//    public GameObject helpObj;
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
    }
    //监测
    IEnumerator waitForCloseTip()
    {
        while(smallGameTipsManager.tipBox.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        InstaniateSycees();

    }
    void Update()
    {
        //收到小游戏的返回消息
        if (reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            StartCoroutine(SmallGameEffect(SmallGame.index_small));

        }
    }
    public void ShowTheSmallGameResult(int tag, double prize)
    {
        tag_small = tag;
        money_small = prize;
        reciveMsgOfSmallGame = true;
    }
    //进入小游戏
    IEnumerator SmallGameEffect(int index)
    {
        if (int.Parse(smallGameTimesLabel.text) > 0)
        {
            aso.PlayOneShot(tapClip);
            //中奖
            if (tag_small == 1)
            {
                aso.PlayOneShot(bigPrizeClip);
                TweenAlpha.Begin(curGirl.transform.parent.FindChild("deng/light").gameObject, 0.3f, 1);
                curGirl.GetComponent<UISprite>().spriteName = curGirl.GetComponent<UISprite>().spriteName.Substring(0, 3) + "1";
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();                
                AddMoney(money_small);
                yield return new WaitForSeconds(2f);
                count++;
            }
			//nothing
			else if (tag_small == 2)
            {
                
                //可用次数降低，金额增加
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();                
                
                aso.PlayOneShot(smallPrizeClip);
                GameObject amin_Broken = curGirl;
                TweenAlpha.Begin(curGirl, 0.3f, 0);
                yield return new WaitForSeconds(2f);

                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    count = 0;
                    for (int i = 0; i< girls.Length; i++)
                    {
                        Destroy(girls[i]);
                    }
                    InstaniateSycees();
                }
            }
            if (count >= 5)
            {

                count = 0;
                //可用次数降低，金额增加
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                
                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    for (int i = 0; i< girls.Length; i++)
                    {
                        Destroy(girls[i]);
                    }
                    InstaniateSycees();
                }
            }


        }
        if (int.Parse(smallGameTimesLabel.text) == 0)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        } else
        {
            SmallGame.canSelect = true;
        }
    }
    public override void ConcretMethod()
    {
        PlayerPrefs.SetInt("MammonFlag", 1);
    }

    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
        {
            curGirl = obj;
            SmallGame.canSelect = false;
            obj.GetComponent<BoxCollider>().enabled = false;
            Sg_Mammom.index_small = int.Parse(obj.name);
            networkConnect.GAME_SMALL_Mammom(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent = curGirl.transform;
        labelObj.transform.localScale = 1.5f * Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        labelObj.GetComponent<UILabel>().fontSize = 23;

        labelObj.GetComponent<UILabel>().text = "+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0, 20, 0);
        tp.to = new Vector3(0, 200, 0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void InstaniateSycees()
    {

        for (int i = 0; i<girls.Length; i++)
        {
            GameObject syc = Instantiate(girlPrefabs) as GameObject;
            syc.name = (i + 1).ToString();
            syc.transform.parent = parentBtns[i].transform;
            syc.transform.localScale = Vector3.one;
            syc.transform.localEulerAngles = Vector3.zero;
            syc.transform.localPosition = new Vector3(0, 20, 0);
            syc.transform.GetComponent<UISprite>().spriteName = "P"+(i+1)+"_3";

            UIEventListener.Get(syc).onClick = SelectBtnMessage;
            parentBtns[i].transform.FindChild("deng/light").GetComponent<UISprite>().alpha = 0;
            girls[i] = syc;
        }
    }
    void OnEnable()
    {
        NetworkConnect.sg_Mammom_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_Mammom_Callback -= ShowTheSmallGameResult;
    }


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_SexyBody : SmallGame
{
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public Object addMoneyLabel;
        
    private GameObject curTreasure;
    public GameObject[] treasures = new GameObject[9];
    public Object failObject;
    private int currentValue = 0;
    private int maxValue;
        
    private bool waitTag = false;
    void Start()
    {
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        currentValue = 0;
        tempTime = valueOfTime;
        networkConnect.GAME_SMALL_SpiderMan();
    }
    void Update()
    {
        if (reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            if (tempTime > 0)
                networkConnect.GAME_SMALL_SpiderMan();
        }
    }
    public void ShowTheSmallGameResult(int tag, double prize)
    {
        Vector2 data = new Vector2();
        data.x = tag;
        data.y = (float)prize;
        money_small = prize;
        Debug.Log(data);
        preLoadData.Add(preLoadData.Count, data);
        maxValue = preLoadData.Count;//0start
        if (tag == 0)
        {
            tempTime--;
            Debug.Log("tempTime--:"+tempTime);
        }
            
        reciveMsgOfSmallGame = true;
            
    }
        
    public void SelectBtnMessage(GameObject treasure)
    {
        SmallGameTipsManager.timer = 0;
		if (SmallGame.canSelect&&tempTime<=0)//已全部请求完消息
        {
            SmallGame.canSelect = false;
            curTreasure = treasure;
            treasure.GetComponent<BoxCollider>().enabled = false;
            
            StartCoroutine(SmallGameEffect());
        }
        smallGameTipsManager.CloseFingerTip();
            
    }
    //进入小游戏
    IEnumerator SmallGameEffect()
    {
        Debug.Log("currentValue:" + currentValue + ";maxValue:" + maxValue);
        if (currentValue < maxValue)
        {
            //中奖特效
            Vector2 value;
            preLoadData.TryGetValue(currentValue, out value);
            currentValue++;
            waitTag = false;
//            yield return new WaitForSeconds(2f);
            if (value.x > 0)
            {
                money_small = value.y;
                smallTotalWin += money_small;

                curTreasure.GetComponent<UISprite>().enabled = false;

                aso.PlayOneShot(bigPrizeClip);
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                    
            } else
            {
                money_small = value.y;
                smallTotalWin += money_small;

                curTreasure.GetComponent<UISprite>().enabled = false;
                GameObject plank = Instantiate(failObject) as GameObject;
                plank.transform.parent = curTreasure.transform;
                plank.transform.localScale = Vector3.one;
                plank.transform.localEulerAngles = Vector3.zero;
                plank.transform.localPosition = Vector3.zero;

                aso.PlayOneShot(smallPrizeClip);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                    
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                yield return new WaitForSeconds(2f);
                Destroy(plank);
                PlaceBagOfMoney();
            }
                
            if (currentValue == maxValue)
            {
                StartCoroutine(Small_Over(smallTotalWin));
                preLoadData.Clear();
            } else
            {
                SmallGame.canSelect = true;
                    
            }
        }
    }
    void PlaceBagOfMoney()
    {
        for (int i = 0; i<9; i++)
        {
            treasures [i].GetComponent<BoxCollider>().enabled = true;
            treasures [i].GetComponent<UISprite>().enabled = true;
                
        }
    }
        
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent = curTreasure.transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
            
 //       labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
            
            
        labelObj.GetComponent<UILabel>().text = "+" + moneyCount.ToString();
            
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0, 75, 0);
        tp.to = new Vector3(0, 225, 0);
        tp.enabled = true;
        tp.PlayForward();
            
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void OnEnable()
    {
        NetworkConnect.sg_NineThunder_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_NineThunder_Callback -= ShowTheSmallGameResult;
    }

}

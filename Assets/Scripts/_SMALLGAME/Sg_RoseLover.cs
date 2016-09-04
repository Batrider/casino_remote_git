using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_RoseLover : SmallGame {
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public Object addMoneyLabel;
    
    private GameObject curTreasure;
    private GameObject[] treasures = new GameObject[9];
    public Object[] sucObject;
    public Object failObject;
    public Object flowerObject;
    public GameObject btnParent;
    private int currentValue = 0;
    private int maxValue;
    public Vector3 startPos;
    public int dis_X;
    public int dis_Y;

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
        PlaceBagOfMoney();
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
            tempTime--;
        
        reciveMsgOfSmallGame = true;
        
    }
    
    public void SelectBtnMessage(GameObject treasure)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
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
                
                curTreasure.GetComponent<UISpriteAnimation>().enabled = true;
                curTreasure.GetComponent<UISpriteAnimation>().ResetToBeginning();

                GameObject go = Instantiate(sucObject[Random.Range(0,3)]) as GameObject;
                go.transform.parent = curTreasure.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localPosition = new Vector3(34,-12,0);
                yield return new WaitForSeconds(0.5f);
                go.GetComponent<UISpriteAnimation>().enabled = true;
                go.GetComponent<UISpriteAnimation>().ResetToBeginning();


                
                aso.PlayOneShot(bigPrizeClip);
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                
            } else
            {
                money_small = value.y;
                smallTotalWin += money_small;
                
                curTreasure.GetComponent<UISpriteAnimation>().enabled = true;
                curTreasure.GetComponent<UISpriteAnimation>().ResetToBeginning();
                
                GameObject plank = Instantiate(failObject) as GameObject;
                plank.transform.parent = curTreasure.transform;
                plank.transform.localScale = Vector3.one;
                plank.transform.localEulerAngles = Vector3.zero;
                plank.transform.localPosition = new Vector3(34,-12,0);

                yield return new WaitForSeconds(0.5f);
                plank.GetComponent<UISpriteAnimation>().enabled = true;
                plank.GetComponent<UISpriteAnimation>().ResetToBeginning();

                aso.PlayOneShot(smallPrizeClip);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                yield return new WaitForSeconds(0.5f);
                Destroy(plank);
                if (currentValue != maxValue)
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
            Destroy(treasures[i]);
        }
        for(int i = 0;i<3;i++)
        {
            for(int j = 0;j<3;j++)
            {
                GameObject go = Instantiate(flowerObject) as GameObject;
                go.transform.parent = btnParent.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localPosition = new Vector3(startPos.x+i*dis_X,startPos.y+j*dis_Y,0);

                EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
                ed.parameters[0] = new EventDelegate.Parameter(go);
                EventDelegate.Add(go.GetComponent<UIButton>().onClick,ed);

                treasures[3*i+j] = go;
            }
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

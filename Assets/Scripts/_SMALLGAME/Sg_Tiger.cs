using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Tiger : SmallGame
{
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public Object addMoneyLabel;
    public Object bagMoneyPrefabs;

    public Object[] bigPrizePrefab;
    public Object smallPrizePrefab;
    public Object swordPrefab;
    public Object zhuahenPrefab;

    public Transform btnParent;
    private GameObject curTreasure;
    private GameObject[] treasures = new GameObject[9];
    public Vector3[] treasuresPos = new Vector3[9];
    private int currentValue = 0;
    private int maxValue;
    void Start()
    {
        //如果是自动游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        base.InitSmallGame();
        currentValue = 0;
        tempTime = valueOfTime;
        networkConnect.GAME_SMALL_SpiderMan();
        PlaceBagOfMoney();
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

        GameObject sword = Instantiate(swordPrefab) as GameObject;
        sword.transform.parent = curTreasure.transform;
        sword.transform.localEulerAngles = Vector3.zero;
        sword.transform.localScale = Vector3.one;
        sword.transform.localPosition = new Vector3(160,-50,0);
        
        if (currentValue < maxValue)
        {
            //中奖特效
            Vector2 value;
            preLoadData.TryGetValue(currentValue, out value);
            currentValue++;
            yield return new WaitForSeconds(2f);
            if (value.x > 0)
            {
                money_small = value.y;
                smallTotalWin += money_small;
                //钻石
                GameObject diamond = Instantiate(bigPrizePrefab[Random.Range(0,bigPrizePrefab.Length)]) as GameObject;
                diamond.transform.parent = curTreasure.transform;
                diamond.transform.localEulerAngles = Vector3.zero;
                diamond.transform.localScale = Vector3.one;
                diamond.transform.localPosition = Vector3.zero;
                TweenPosition.Begin(diamond,0.5f,new Vector3(-170,0,0));

                aso.PlayOneShot(bigPrizeClip);
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                
            }
            else
            {
                money_small = value.y;
                smallTotalWin += money_small;
                
                GameObject bomb = Instantiate(smallPrizePrefab) as GameObject;
                bomb.transform.parent =curTreasure.transform;
                bomb.transform.localEulerAngles = Vector3.zero;
                bomb.transform.localScale = Vector3.one;
                bomb.transform.localPosition = Vector3.zero;
                TweenPosition.Begin(bomb,0.3f,new Vector3(0,95,0));

                GameObject zhuahen = Instantiate(zhuahenPrefab) as GameObject;
                zhuahen.transform.parent =btnParent.transform;
                zhuahen.transform.localEulerAngles = Vector3.zero;
                zhuahen.transform.localScale = Vector3.one;
                zhuahen.transform.localPosition = new Vector3(0,-250,0);

                aso.PlayOneShot(smallPrizeClip);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");

                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                yield return new WaitForSeconds(2f);
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
        for(int i = 0;i<9;i++)
        {
            if(treasures[i]!=null)
            {
                Destroy(treasures[i]);
            }

            GameObject treasure = Instantiate(bagMoneyPrefabs) as GameObject;
            treasure.transform.parent = btnParent.transform;
            treasure.transform.localEulerAngles = Vector3.zero;
            treasure.transform.localScale = Vector3.one;
            treasure.transform.localPosition = treasuresPos[i];


            EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
            ed.parameters[0] = new EventDelegate.Parameter(treasure);
            EventDelegate.Add(treasure.GetComponent<UIButton>().onClick,ed);

            treasures[i] = treasure;
        }
    }

    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curTreasure.transform;
        labelObj.transform.localScale =Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(0,75,0);
        tp.to = new Vector3(0,225,0);
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

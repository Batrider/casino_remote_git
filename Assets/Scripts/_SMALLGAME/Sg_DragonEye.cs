using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_DragonEye : SmallGame
{
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public GameObject[] treasure;
    public GameObject fireEffect;
    public Object addMoneyLabel;
    public GameObject dragron;
    public Transform btnParent;
    private GameObject curTreasure;
    private int currentValue = 0;
    private int maxValue;
    private GameObject[] footBallPlayer = new GameObject[9];


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
            tempTime--;
        
        reciveMsgOfSmallGame = true;

    }

    public void SelectBtnMessage(GameObject treasure)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            treasure.GetComponent<BoxCollider>().enabled = false;
            curTreasure = treasure;
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
            UISpriteAnimation ua = curTreasure.GetComponent<UISpriteAnimation>();
            if (value.x > 0)
            {
                money_small = value.y;
                smallTotalWin += money_small;
                ua.enabled = true;
                ua.namePrefix ="B_";
                ua.ResetToBeginning();
                aso.PlayOneShot(bigPrizeClip);

                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                
            }
            else
            {
                money_small = value.y;
                smallTotalWin += money_small;
                
                ua.enabled = true;
                ua.namePrefix ="konglongdan";
                ua.ResetToBeginning();

                UISpriteAnimation uaDragon = dragron.GetComponent<UISpriteAnimation>();
                uaDragon.enabled = true;
                uaDragon.ResetToBeginning();
                fireEffect.SetActive(true);
                aso.PlayOneShot(smallPrizeClip);
                yield return new WaitForSeconds(0.2f);
                ParticleSystem[] fireParticles = fireEffect.GetComponentsInChildren<ParticleSystem>();
                for(int i = 0;i<fireParticles.Length;i++)
                {
                    fireParticles[i].enableEmission = true;
                }
                yield return new WaitForSeconds(2f);
                dragron.GetComponent<UISprite>().spriteName = "long0001";
                uaDragon.enabled = false;
               
                ua.namePrefix ="suile";
                ua.ResetToBeginning();
                for(int i = 0;i<fireParticles.Length;i++)
                {
                    fireParticles[i].enableEmission = false;
                }
                yield return new WaitForSeconds(0.5f);
                ua.enabled = false;
                fireEffect.SetActive(false);


                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                if(int.Parse(smallGameTimesLabel.text)>0)
                    TreasureReplace();
                
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");

                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
            }

            if (currentValue == maxValue)
            {
                StartCoroutine(Small_Over(smallTotalWin));
                preLoadData.Clear();
            } else
            {
                yield return new WaitForSeconds(2f);
                SmallGame.canSelect = true;
            }
        }
    }
    void TreasureReplace()
    {
        for(int i = 0;i<treasure.Length;i++)
        {
            treasure[i].GetComponent<UISprite>().spriteName = "B_1";
            treasure[i].GetComponent<BoxCollider>().enabled = true;
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
        tp.from = new Vector3(100,0,0);
        tp.to = new Vector3(100,150,0);
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

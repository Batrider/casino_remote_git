using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Vampire : SmallGame
{
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public GameObject[] btnOBJs;
    public Object coffinPrefab;
    public Object addMoneyLabel;
    public Object failEffect;

    public Transform btnParent;
    private GameObject curCoffin ;
    private int currentValue = 0;
    private int maxValue;
    private GameObject[] conffins = new GameObject[9];


    private bool waitTag = false;
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
        StartCoroutine(waitForCloseTip());

    }
    IEnumerator waitForCloseTip()
    {
        while (smallGameTipsManager.tipBox.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(PlaceConffin());

    }
    void Update()
    {
        if (reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            if (tempTime > 0)
                GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_SpiderMan();
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

    public void SelectBtnMessage(GameObject coffin)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            curCoffin = coffin;
            StartCoroutine(SmallGameEffect());
        }
        GameObject.FindGameObjectWithTag("SgHelp").GetComponent<SmallGameTipsManager>().CloseFingerTip();
        
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
            if (value.x > 0)
            {
                money_small = value.y;
                smallTotalWin += money_small;
                curCoffin.GetComponent<CoffinController>().OpenTheClock(true,currentValue);
                yield return new WaitForSeconds(1.5f);
                aso.PlayOneShot(tapClip);
                yield return new WaitForSeconds(0.5f);
                aso.PlayOneShot(bigPrizeClip);

                AddMoney(money_small);
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                yield return new WaitForSeconds(2f);
                
            }
            else
            {
                money_small = value.y;
                smallTotalWin += money_small;
                //GetComponent<AudioSource>().volume = 0.12f;
                curCoffin.GetComponent<CoffinController>().OpenTheClock(false,currentValue);
                
                yield return new WaitForSeconds(1.5f);
                aso.PlayOneShot(tapClip);
                yield return new WaitForSeconds(0.5f);
                aso.PlayOneShot(smallPrizeClip);

                GameObject eff = Instantiate(failEffect) as GameObject;
                eff.transform.parent = btnParent;
                eff.transform.localScale = Vector3.one;
                eff.transform.localEulerAngles = Vector3.zero;
                eff.transform.localPosition = new Vector3(0,0,0);

                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");

                AddMoney(money_small);
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                yield return new WaitForSeconds(2f);
                if(int.Parse(smallGameTimesLabel.text)>0)
                    StartCoroutine(PlaceConffin());
                
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
    IEnumerator PlaceConffin()
    {
        Vector3 playerPosition = new Vector3(-1200, -50, 0);
        Vector3 playerPosition2 = new Vector3(-1200, -550, 0);
        
        for (int i = 0; i<9; i++)
        {


            if (conffins [i] != null)
            {
                Destroy(conffins[i]);
            }
            yield return new WaitForSeconds(0.1f);
            GameObject coffin = Instantiate(coffinPrefab) as GameObject;
            coffin.name = (i+1).ToString();
            coffin.transform.parent = btnParent;
            coffin.transform.localScale = Vector3.one;
            coffin.transform.localEulerAngles = Vector3.zero;
            if(i < 5)
            {
                coffin.transform.localPosition = playerPosition;
                TweenPosition.Begin(coffin,1f,new Vector3(72-218*i,coffin.transform.localPosition.y,0));
            }else
            {
                coffin.transform.localPosition = playerPosition2;
                TweenPosition.Begin(coffin,1f,new Vector3(-146-218*(i-5),coffin.transform.localPosition.y,0));
                
            }

            EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
            ed.parameters[0] = new EventDelegate.Parameter(coffin);
            EventDelegate.Add(coffin.GetComponent<UIButton>().onClick,ed);
            conffins [i] = coffin;
        }
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =curCoffin.transform;
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

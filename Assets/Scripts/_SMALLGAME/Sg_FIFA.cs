using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_FIFA : SmallGame
{
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public GameObject[] btnOBJs;
    public Object Footballplayer;
    public Object addMoneyLabel;
    public Object effect;
    public GameObject targetSoccer;
    public Transform btnParent;
    private GameObject curPlayer;
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
        PlacePeople();
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

    public void SelectBtnMessage(GameObject player)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            curPlayer = player;
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
            Animator amSoccer = targetSoccer.GetComponent<Animator>();
            waitTag = false;
            if (value.x > 0)
            {
                money_small = value.y;
                smallTotalWin += money_small;
                StartCoroutine(Play("success"));
                while(!waitTag)
                {
                    yield return null;
                }
                aso.PlayOneShot(bigPrizeClip);
                
                GameObject eff = Instantiate(effect) as GameObject;
                eff.GetComponent<UISprite>().spriteName = "GOAL";
                eff.transform.parent = btnParent;
                eff.transform.localScale = Vector3.one;
                eff.transform.localEulerAngles = Vector3.zero;
                eff.transform.localPosition = new Vector3(0,-250,0);

                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();
                AddMoney(money_small);
                
            }
            else
            {
                money_small = value.y;
                smallTotalWin += money_small;
                
                //GetComponent<AudioSource>().volume = 0.12f;
                StartCoroutine(Play("fail"));
                while(!waitTag)
                {
                    yield return null;
                }
                aso.PlayOneShot(smallPrizeClip);

                GameObject eff = Instantiate(effect) as GameObject;
                eff.GetComponent<UISprite>().spriteName = "FAILD";
                eff.transform.parent = btnParent;
                eff.transform.localScale = Vector3.one;
                eff.transform.localEulerAngles = Vector3.zero;
                eff.transform.localPosition = new Vector3(0,-250,0);

                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
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
    IEnumerator Play(string trigger)
    {
        UISpriteAnimation uas = curPlayer.GetComponent<UISpriteAnimation>();
        uas.enabled = true;
        uas.ResetToBeginning();

        Destroy(curPlayer.transform.FindChild("tag").gameObject);

        Vector3 targetPosition = targetSoccer.transform.localPosition;
        Vector3 startPosition = curPlayer.transform.localPosition;
        while(Vector3.Distance(curPlayer.transform.localPosition,targetPosition)>5f)
        {
            Debug.Log(Vector3.Distance(curPlayer.transform.localPosition,targetPosition));
            curPlayer.transform.localPosition = Vector3.MoveTowards(curPlayer.transform.localPosition,targetPosition,200*Time.deltaTime);
            yield return null;
        }
        aso.PlayOneShot(tapClip);
        yield return new WaitForSeconds(0.3f);
        uas.namePrefix = "ti";
        uas.loop = false;
        uas.ResetToBeginning();
        targetSoccer.GetComponent<Animator>().SetTrigger(trigger);
        yield return new WaitForSeconds(1);
        Destroy(curPlayer);
        if(string.Equals(trigger,"fail"))
            PlacePeople();
        waitTag = true;
    }
    void PlacePeople()
    {
        Vector3 playerPosition = new Vector3(320, -100, 0);
        for (int i = 0; i<3; i++)
        {
            for (int j = 0; j<3; j++)
            {
                if (footBallPlayer [3*i+j] != null)
                {
                    Destroy(footBallPlayer[3*i+j]);
                }
                GameObject player = Instantiate(Footballplayer) as GameObject;
                player.name = (3*i+j).ToString();
                player.transform.FindChild("tag/num").GetComponent<UILabel>().text = (3*i+j+1).ToString();
                player.transform.parent = btnParent;
                player.transform.localPosition = playerPosition + new Vector3(250*i,-250*j,0);
                player.transform.localScale = Vector3.one;
                player.transform.localEulerAngles = Vector3.zero;

                EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
                ed.parameters[0] = new EventDelegate.Parameter(player);
                EventDelegate.Add(player.GetComponent<UIButton>().onClick,ed);

                footBallPlayer [3*i+j] = player;

                Debug.Log("=============================Instaniate======================");
            }
        }
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =transform;
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

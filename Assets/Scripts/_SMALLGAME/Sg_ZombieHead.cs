using UnityEngine;
using System.Collections;

public class Sg_ZombieHead : SmallGame {
    public GameObject[] zombie1;
    public GameObject[] zombie2;
    public GameObject[] zombie3;
    public GameObject[] zombie4;
    public GameObject[] zombie5;
    public GameObject[] anim_smalls = new GameObject[5];
    public GameObject airPlane; 
    public UIButton[] btns;
    private int currentIndex;
    private GameObject currentQuan;

    public AirplaneController airPlaneController;
    //    public GameObject helpObj;
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
        Debug.Log("This is the result:");
        tag_small = tag;
        money_small = prize;
        Debug.Log("tag:"+tag+";prize:"+prize);
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


            //中奖
            if (tag_small == 1)
            {
                airPlane.GetComponent<AirplaneController>().AirplaneStartRun(currentIndex > 2 ? 498 : 390, true, currentIndex);
            }
            //nothing
            else if (tag_small == 2)
            {
                airPlane.GetComponent<AirplaneController>().AirplaneStartRun(currentIndex > 2 ? 498 : 390, false, currentIndex);
            }

            if (int.Parse(smallGameTimesLabel.text) == 0)
            {
                SmallGameTipsManager.timer = 1000;
            }
            while (!AirplaneController.move)
            {
                yield return null;
            }
            if (int.Parse(smallGameTimesLabel.text) == 0)
            {
                StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
            }
            else
            {
                canSelect = true;
                foreach (UIButton ub in btns)
                {
                    ub.enabled = true;
                    ub.state = UIButtonColor.State.Normal;
                }
            }
        }
    }
    public override void ConcretMethod ()
    {
        PlayerPrefs.SetInt("ZombieHead", 1);
    }
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        
        if(SmallGame.canSelect)
        {
            currentQuan = obj;
            SmallGame.canSelect = false;
            currentIndex = int.Parse(obj.name);
            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
            foreach(UIButton ub in btns)
            {
                ub.enabled = false;
            }
            obj.GetComponent<UISpriteAnimation>().enabled = false;
            obj.GetComponent<UISprite>().spriteName = "gun10001";
            TweenScale ts = TweenScale.Begin(obj,2f,.3f*Vector3.one);
            ts.style = UITweener.Style.PingPong;
            airPlaneController.aimPos = currentQuan.transform.position;
        }
        smallGameTipsManager.CloseFingerTip();
        
    }

    public void ShoeZombieEffect()
    {
        
        currentQuan.SetActive(false);
        switch(currentIndex)
        {
            case 1:
                foreach(GameObject zombie in zombie1)
                {
                    TweenColor.Begin(zombie,1f,Color.black);
                }
                break;
            case 2:
                foreach(GameObject zombie in zombie2)
                {
                    TweenColor.Begin(zombie,1f,Color.black);
                }
                break;
                
            case 3:
                foreach(GameObject zombie in zombie3)
                {
                    TweenColor.Begin(zombie,1f,Color.black);
                }
                break;
                
            case 4:
                foreach(GameObject zombie in zombie4)
                {
                    TweenColor.Begin(zombie,1f,Color.black);
                }
                break;
                
            case 5:
                foreach(GameObject zombie in zombie5)
                {
                    TweenColor.Begin(zombie,1f,Color.black);
                }
                break;
                aso.PlayOneShot(bigPrizeClip);
        }
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

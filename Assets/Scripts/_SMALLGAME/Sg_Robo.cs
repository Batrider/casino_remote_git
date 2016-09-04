using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Robo : SmallGame
{
    public Object addMoneyLabel;
    public Object powerGunPrefab;
    public GameObject gunParent;
    public GameObject energyBoxParent;
    public GameObject waring;
    public GameObject robot;
    private GameObject curGun;
    public Transform[] robotPoint = new Transform[5];
    int count = 0;
    bool isFirstSet = false;
    bool isTipShow = false;
    SmallGameTipsManager stm;

    public List<GameObject> energyBox = new List<GameObject>();
    GameObject[] fiveGuns = new GameObject[5];
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
        InstaniateGuns();

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
            while(Vector3.Distance(curGun.transform.position,robotPoint[count].position+new Vector3(0.3f,-0.05f,0))>0.005f)
            {
                yield return null;
                curGun.transform.position = Vector3.MoveTowards(curGun.transform.position,robotPoint[count].position +new Vector3(0.3f,-0.05f,0),0.005f);
            }
            curGun.transform.FindChild("dianliu").GetComponent<UISprite>().enabled = true;
            aso.PlayOneShot(tapClip);
            robot.GetComponent<Animator>().SetTrigger("charging");
            yield return new WaitForSeconds(1f);
            //中奖
            if (tag_small == 1)
            {
                aso.PlayOneShot(bigPrizeClip);


                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();                
                AddMoney(money_small);
                Destroy(curGun);
                
                yield return new WaitForSeconds(0.5f);
                count++;
                InstaniateEnergyBox(count);
            }
			//nothing
			else if (tag_small == 2)
            {

                //可用次数降低，金额增加
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                
                smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text) + money_small).ToString();                

                TweenAlpha.Begin(waring,0.5f,1);
                aso.PlayOneShot(smallPrizeClip);
                Destroy(curGun);
                yield return new WaitForSeconds(2f);
                TweenAlpha.Begin(waring,0.1f,0);
                
                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    count = 0;
                    InstaniateGuns();
                }
                InstaniateEnergyBox(0);
                
            }
            if (count >= 5)
            {
                robot.GetComponent<Animator>().SetTrigger("fly");
                
                count = 0;
                
                //可用次数降低，金额增加
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                
                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    InstaniateGuns();
                }
                InstaniateEnergyBox(0);
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
            curGun = obj;
            SmallGame.canSelect = false;
            obj.GetComponent<BoxCollider>().enabled = false;
            index_small = int.Parse(obj.name);
            networkConnect.GAME_SMALL_Mammom(index_small);
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
                labelObj.transform.parent = curGun.transform.parent;
        labelObj.transform.localScale = 1.5f * Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
        labelObj.GetComponent<UILabel>().text = "+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = new Vector3(curGun.transform.localPosition.x, curGun.transform.localPosition.y+20, 0);
        tp.to = new Vector3(curGun.transform.localPosition.x, curGun.transform.localPosition.y+200, 0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void InstaniateGuns()
    {
        Vector3 startPos = new Vector3(0,-248,0);
        for(int i = 0;i<5;i++)
        {
            if(fiveGuns[i]!=null)
                Destroy(fiveGuns[i]);
        }
        for(int  i = 0;i<5;i++)
        {
            GameObject gun = Instantiate(powerGunPrefab) as GameObject;
            gun.name = (i+1).ToString();
            gun.transform.parent = gunParent.transform;
            gun.transform.localPosition = startPos + new Vector3(0,i*120,0);
            gun.transform.localScale = Vector3.one;
            gun.transform.localEulerAngles = Vector3.zero;

            EventDelegate ed = new EventDelegate(this,"SelectBtnMessage");
            ed.parameters[0] = new EventDelegate.Parameter(gun);
            EventDelegate.Add(gun.GetComponent<UIButton>().onClick,ed);

            fiveGuns[i] = gun;
        }
    }

    void InstaniateEnergyBox(int index)
    {
        for(int i = 0;i<energyBox.Count;i++)
        {
            energyBox[i].SetActive(false);
        }
        for(int i = 0;i<index;i++)
        {
            energyBox[i].SetActive(true);
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

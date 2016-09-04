using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Mammom : SmallGame
{
    public Object bigPrizeEffectObj;
    public Object addMoneyLabel;
    public Object hammmerObj;
    public Object smokeEffect;
    public Object syceePrefabs;
    private GameObject sycee;
    public Animator manAnim;
    public GameObject[] syceeParent = new GameObject[5];
    GameObject[] fiveSycees = new GameObject[5];
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
            
            //挥舞锤子效果
            GameObject Amin_Hammer = Instantiate(hammmerObj) as GameObject;
            Amin_Hammer.transform.parent = sycee.transform.parent;
            Amin_Hammer.transform.localScale = Vector3.one;
            Amin_Hammer.transform.eulerAngles = Vector3.zero;
            Amin_Hammer.transform.localPosition = new Vector3(31f, 105f, 0);

            Animator ham_anim = Amin_Hammer.GetComponent<Animator>();
            ham_anim.SetBool("HammerStart", true);
            yield return new WaitForSeconds(1f);
            aso.PlayOneShot(tapClip);
            yield return new WaitForSeconds(0.5f);
            ham_anim.SetBool("HammerStart", false);
            Destroy(Amin_Hammer);
            //中奖
            if (tag_small == 1)
            {
                aso.PlayOneShot(bigPrizeClip);
                manAnim.SetTrigger("laugh");
                TweenAlpha.Begin(sycee.transform.FindChild("light").gameObject, 0.3f, 1);
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
                GameObject amin_Broken = sycee;
                UISpriteAnimation ua = amin_Broken.GetComponent<UISpriteAnimation>();
                ua.enabled = true;
                ua.ResetToBeginning();
                yield return new WaitForSeconds(2f);

                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    ua.enabled = false;
                    amin_Broken.GetComponent<UISprite>().spriteName = "sycee";
                    count = 0;
                    for (int i = 0; i<fiveSycees.Length; i++)
                    {
                        Destroy(fiveSycees [i]);
                    }
                    InstaniateSycees();
                }
            }
            if (count >= 5)
            {
                GameObject bigPrizeEffect = Instantiate(bigPrizeEffectObj) as GameObject;
                bigPrizeEffect.transform.parent = this.transform;
                bigPrizeEffect.transform.localScale = Vector3.one;
                bigPrizeEffect.transform.localEulerAngles = Vector3.zero;
                bigPrizeEffect.transform.localPosition = new Vector3(-327,833,0);
                count = 0;
                //可用次数降低，金额增加
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text) - 1).ToString();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                
                if (int.Parse(smallGameTimesLabel.text) != 0)
                {
                    for (int i = 0; i<fiveSycees.Length; i++)
                    {
                        Destroy(fiveSycees [i]);
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
            sycee = obj;
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
        labelObj.transform.parent = sycee.transform;
        labelObj.transform.localScale = 1.5f * Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
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

        for (int i = 0; i<syceeParent.Length; i++)
        {
            GameObject syc = Instantiate(syceePrefabs) as GameObject;
            syc.name = (i + 1).ToString();
            syc.transform.parent = syceeParent [i].transform;
            syc.transform.localScale = Vector3.one;
            syc.transform.localEulerAngles = Vector3.zero;
            syc.transform.localPosition = Vector3.zero;
            UIEventListener.Get(syc).onClick = SelectBtnMessage;
            SmokeEffect(syc);

            fiveSycees [i] = syc;
        }
    }
    void SmokeEffect(GameObject obj)
    {
        GameObject lamp1Particle = Instantiate(smokeEffect) as GameObject;
        lamp1Particle.transform.parent = obj.transform.parent;
        lamp1Particle.transform.localScale = Vector3.one;
        lamp1Particle.transform.localEulerAngles = Vector3.zero;
        lamp1Particle.transform.localPosition = new Vector3(0, -33, 0);
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

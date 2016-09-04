using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_PinkBar : SmallGame
{
    public UI2DSpriteAnimation ui2s = new UI2DSpriteAnimation();
    public Object palm;
    private GameObject curPalmImage;
    public GameObject palmBG;
    
    public Object addMoneyLabel;
    
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    private Vector3[] startPoint = new Vector3[5];
    private Color[] starColor = new Color[5];
    void Start()
    {
        //如果是小游戏的话
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
        if (reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            StartCoroutine(SmallGameEffect());
        }
    }
    public void ShowTheSmallGameResult(int index, int tag, double prize)
    {
        index_small = index;
        tag_small = tag;
        money_small = prize;
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
            
//            GetComponent<AudioSource>().volume = 0.1f;
            aso.clip = bigPrizeClip;
            aso.Play();

            palmBG.GetComponent<UISprite>().alpha = 1;

            GameObject palmGameObject = Instantiate(palm) as GameObject;
            palmGameObject.transform.parent = ui2s.transform;
            palmGameObject.transform.localScale = Vector3.one;
            palmGameObject.transform.localEulerAngles = Vector3.zero;
            
            palmGameObject.GetComponent<UISprite>().spriteName = curPalmImage.GetComponent<UISprite>().spriteName;
            palmGameObject.SetActive(true);
            curPalmImage.SetActive(false);

            yield return new WaitForSeconds(0.75f);
            ui2s.enabled = true;
            ui2s.ResetToBeginning();
            AddMoney(money_small);
            TweenAlpha.Begin(palmBG,1f,0);
            yield return new WaitForSeconds(1.0f);
            smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+money_small).ToString();
            
        }
        if (int.Parse(smallGameTimesLabel.text) == 0)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
        } else
        {
            SmallGame.canSelect = true;
        }
    }
    public override void ConcretMethod()
    {
    }
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if (SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            obj.GetComponent<BoxCollider>().enabled = false;
            curPalmImage = obj;
            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
        
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =palmBG.transform;
        labelObj.transform.localScale =Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        //       labelObj.GetComponent<UILabel>().bitmapFont = smallGameTimesLabel.bitmapFont;
        
        
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
        NetworkConnect.sg_5Turnover_Callback += ShowTheSmallGameResult;
    }
    void OnDisable()
    {
        NetworkConnect.sg_5Turnover_Callback -= ShowTheSmallGameResult;        
    }
}

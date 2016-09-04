using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_Victory : SmallGame {
    public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public Object bombPrefab;
    public Object shipPrefab;
    public GameObject finger;
    private float timeCount;
    public Dictionary<int,Vector2> preLoadData = new Dictionary<int, Vector2>();
    private float tempTime = 2;
    public GameObject[] btnOBJs;
    private GameObject btnOBJ;
    private GameObject targetShip;

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
    }
    void Update()
    {
        if(reciveMsgOfSmallGame)
        {
            reciveMsgOfSmallGame = false;
            if(tempTime>0)
                networkConnect.GAME_SMALL_SpiderMan();
        }
        //spawn ships
        timeCount-=Time.deltaTime;
        if(timeCount<0)
        {
            timeCount = Random.Range(5f,8f);
            ShipSpawn();
        }
    }
    public void ShowTheSmallGameResult(int tag,double prize)
    {
        Vector2 data = new Vector2();
        data.x = tag;
        data.y = (float)prize;
        money_small = prize;
        Debug.Log(data);
        preLoadData.Add(preLoadData.Count,data);
        maxValue = preLoadData.Count;//0start
        if(tag==0)
            tempTime--;
        
        reciveMsgOfSmallGame = true;

    }
    //进入小游戏
    IEnumerator SmallGameEffect()
    {
        if(currentValue<maxValue)
        {
            //中奖特效
            Vector2 value;
            preLoadData.TryGetValue(currentValue,out value);
            currentValue++;
            //金额增加
            aso.PlayOneShot(tapClip);
            
            if(value.x>0)
            {
                //子弹
                money_small = value.y;
                smallTotalWin += money_small;
                GameObject bomb = Instantiate(bombPrefab) as GameObject;
                bomb.transform.parent = btnOBJ.transform;
                bomb.transform.localEulerAngles = new Vector3(0,0,74);
                bomb.transform.localScale = Vector3.one;
                bomb.transform.localPosition = new Vector3(0,72,0);
                bomb.GetComponent<VictoryBomb>().failDistance = 0.15f;
                bomb.GetComponent<VictoryBomb>().Ship = this.targetShip;
                
                
                yield return new WaitForSeconds(1f);
            }
            //nothing
            else
            {
 //               GetComponent<AudioSource>().volume = 0.12f;
                aso.PlayOneShot(smallPrizeClip);
                money_small = value.y;
                smallTotalWin += money_small;
                //子弹
                GameObject bomb = Instantiate(bombPrefab) as GameObject;
                bomb.transform.parent = btnOBJ.transform;
                bomb.transform.localEulerAngles = new Vector3(0,0,74);
                bomb.transform.localScale = Vector3.one;
                bomb.transform.localPosition = new Vector3(0,72,0);
                bomb.GetComponent<VictoryBomb>().failDistance = 0.28f;
                bomb.GetComponent<VictoryBomb>().Ship = this.targetShip;
                
                yield return new WaitForSeconds(1f);
                smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
                smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
                smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
                
            }
            if(currentValue == maxValue)
            {
                StartCoroutine(Small_Over(smallTotalWin));
                preLoadData.Clear();
                PlayerPrefs.SetInt("Victory", 1);
            }
            else
            {
                yield return new WaitForSeconds(2f);
                SmallGame.canSelect = true;
            }
        }
    }
    public void SelectBtnMessage(GameObject targetShip)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            this.targetShip = targetShip;
            //计算离船最近的炮台，然后开火
            GameObject btn = minDistanceBtn();
            btn.GetComponent<FireAnim>().startAnim();
            btnOBJ = btn;
            StartCoroutine(SmallGameEffect());
        }
        if (finger != null)
            finger.SetActive(false);
        //smallGameTipsManager.CloseFingerTip();

    }
    private GameObject minDistanceBtn()
    {
        GameObject minDistanceBtn = btnOBJs[0];
        float minDistance = float.MaxValue;
        foreach(GameObject btn in btnOBJs)
        {
            if(Vector2.Distance(btn.transform.position,this.targetShip.transform.position)<minDistance)
            {
                minDistance = Vector2.Distance(btn.transform.position,this.targetShip.transform.position);
                minDistanceBtn = btn;
            }
        }
        return minDistanceBtn;
    }
    void ShipSpawn()
    {
        GameObject ship = Instantiate(shipPrefab) as GameObject;
        ship.transform.parent = transform;
        ship.transform.localPosition = new Vector3(1200,Random.Range(-100,150),0);
        ship.transform.localScale = Vector3.one;
        ship.transform.localEulerAngles = new Vector3(0,0,0);

        ship.GetComponent<ShipAnimation>().sv = this;

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

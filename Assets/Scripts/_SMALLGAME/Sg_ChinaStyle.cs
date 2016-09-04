using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_ChinaStyle : SmallGame {
    public GameObject[] fiveLantern = new GameObject[5];
    public Object fireAnimPrefabs;
    
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
		if(reciveMsgOfSmallGame)
		{
			reciveMsgOfSmallGame = false;
			StartCoroutine(SmallGameEffect());
		}
	}
	public void ShowTheSmallGameResult(int index,int tag,double prize)
	{
		Debug.Log("This is the result:");
		index_small = index;
		tag_small = tag;
		money_small = prize;
		Debug.Log("index:"+index+";tag:"+tag+";prize:"+prize);
		reciveMsgOfSmallGame = true;
	}
	//进入小游戏
	IEnumerator SmallGameEffect()
	{
		if(int.Parse(smallGameTimesLabel.text)>0)
		{
			//可用次数降低，金额增加
			smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
            smallGameTimesLabel.GetComponentInParent<Animator>().SetTrigger("timeLose");
            smallGameTimesLabel.GetComponentInParent<UIPlaySound>().Play();
            
/*
 * //小奖
			if(tag_small ==1)
			{
				GetComponent<AudioSource>().volume = 0.1f;
				aso.clip = smallPrizeClip;
				aso.Play();
				GameObject	Amin_Tower = Instantiate(Resources.Load("SmallGame/ChinaStyle/SZTower")) as GameObject;
				Amin_Tower.transform.parent = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index_small).transform;
				Amin_Tower.transform.localScale = Vector3.one;
				Amin_Tower.transform.eulerAngles = Vector3.zero;
				Amin_Tower.transform.localPosition = new Vector3(48f,21f,0);
				anim_smalls.Add(int.Parse(smallGameTimesLabel.text),Amin_Tower);
			}
			//大奖
			else if(tag_small ==2)
			{
				GetComponent<AudioSource>().volume = 0.1f;
				aso.clip = bigPrizeClip;
				aso.Play();
				GameObject	Amin_Panda = Instantiate(Resources.Load("SmallGame/ChinaStyle/SZPanda")) as GameObject;
				Amin_Panda.transform.parent = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index_small).transform;
				Amin_Panda.transform.localScale = Vector3.one;
				Amin_Panda.transform.eulerAngles = Vector3.zero;
				Amin_Panda.transform.localPosition = new Vector3(48f,21f,0);
				anim_smalls.Add(int.Parse(smallGameTimesLabel.text),Amin_Panda);
			}
*/
            //等待灯笼上升
            yield return new WaitForSeconds(2f);
		}
		if(int.Parse(smallGameTimesLabel.text) ==0)
		{
			yield return new WaitForSeconds(2f);
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
		}
        else
        {
            SmallGame.canSelect = true;
        }
	}
	public override void ConcretMethod ()
	{
	}
    public void SelectBtnMessage(GameObject obj)
    {
        SmallGameTipsManager.timer = 0;
        if(SmallGame.canSelect)
        {
            SmallGame.canSelect = false;
            GameObject fire = Instantiate(fireAnimPrefabs) as GameObject;
            fire.transform.parent = obj.transform.parent;
            fire.transform.localScale = Vector3.one;
            fire.transform.localEulerAngles = Vector3.zero;
            fire.transform.localPosition = Vector3.zero;


            obj.GetComponent<BoxCollider>().enabled = false;
            obj.SetActive(false);
            //灯笼上升
            TweenColor tc = TweenColor.Begin(obj.transform.parent.gameObject,0.5f,Color.white);
            tc.delay = 1.5f;
            TweenPosition tp = TweenPosition.Begin(obj.transform.parent.gameObject,3f,new Vector3(obj.transform.parent.localPosition.x,obj.transform.parent.localPosition.y+1000,0));
            tp.delay = 2f;
            obj.transform.parent.GetComponent<TweenPosition>().method = UITweener.Method.EaseIn;
            networkConnect.GAME_SMALL_5TurnOver(int.Parse(obj.name));
        }
        smallGameTipsManager.CloseFingerTip();
        
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_SlientSamurai : SmallGame {
    public GameObject helpObj;
    
	void Start()
	{
        //如果是小游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        if (PlayerPrefs.GetInt("SilentSamuraiFlag")== 0)
            helpObj.SetActive(true);
	}
	void Update()
	{
		//收到小游戏的返回消息
		if(reciveMsgOfSmallGame)
		{
			reciveMsgOfSmallGame = false;
			StartCoroutine(SmallGameEffect(SmallGame.index_small));
		}
	}
	public void ShowTheSmallGameResult(int index,int tag,double prize)
	{
		Debug.Log("This is the result:");
		SmallGame.index_small = index;
		tag_small = tag;
		money_small = prize;
		Debug.Log("index:"+index+";tag:"+tag+";prize:"+prize);
		reciveMsgOfSmallGame = true;
	}
	//进入小游戏
	IEnumerator SmallGameEffect(int index)
	{
		GetComponent<AudioSource>().volume = 0.1f;
		GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index+"/knife").GetComponent<Animator>().SetTrigger("hua");

		aso.clip = tapClip;
		aso.Play();
		if(int.Parse(smallGameTimesLabel.text)>0)
		{
			//金额增加
			smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
			smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+money_small).ToString();
			//中奖特效
			if(tag_small ==1)
			{
				GameObject	success = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index);
				Animator discard = success.GetComponent<Animator>();
				discard.SetTrigger("discard");
				yield return new WaitForSeconds(0.5f);
				
			}
			//nothing
			else if(tag_small ==2)
			{
				GameObject	fail = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index);
				Animator shake = fail.GetComponent<Animator>();
				shake.SetTrigger("shake");
				yield return new WaitForSeconds(0.5f);
			}
			SmallGame.canSelect = true;
			
		}
		if(int.Parse(smallGameTimesLabel.text) ==0)
		{
            StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
		}
	}



	//结束小游戏：于子类中实现的有别于父类的操作
	public override void ConcretMethod ()
	{
		base.ConcretMethod ();
		for(int i = 1;i<=5;i++)
		{
			GameObject normal = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+i);
			Animator am = normal.GetComponent<Animator>();
			am.SetTrigger("idle");
		}
        PlayerPrefs.SetInt("SilentSamuraiFlag", 1);
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
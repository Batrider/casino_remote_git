using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sg_SpiderMan : SmallGame {
	public Dictionary<int,GameObject> anim_smalls = new Dictionary<int, GameObject>();
    public GameObject helpObj;
	void Start()
	{
        //如果是自动游戏的话
        if (manager.autoSpin)
        {
            base.StartCoroutine(AutoSmallGame());
            return;
        }
        if (PlayerPrefs.GetInt("SpiderManFlag")== 0)
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
	public void ShowTheSmallGameResult(int tag,double prize)
	{
		Debug.Log("This is the result:");
		tag_small = tag;
		money_small = prize;
		Debug.Log("tag:"+tag+";prize:"+prize);
		reciveMsgOfSmallGame = true;
	}
	//进入小游戏
	IEnumerator SmallGameEffect(int index)
	{
		if(int.Parse(smallGameTimesLabel.text)>0)
		{
			//金额增加
            aso.PlayOneShot(tapClip);
			smallPrizeLabel.text = (double.Parse(smallPrizeLabel.text)+money_small).ToString();
			//中奖特效
			if(tag_small ==1)
			{
				GetComponent<AudioSource>().volume = 0.1f;
                aso.PlayOneShot(bigPrizeClip);
				
				GameObject	SuccessCard = Instantiate(Resources.Load("SmallGame/SpiderMan/SuccessCard")) as GameObject;
				SuccessCard.transform.parent = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index).transform;
				SuccessCard.transform.localScale = Vector3.one;
				SuccessCard.transform.eulerAngles = Vector3.zero;
				SuccessCard.transform.localPosition = Vector3.zero;
				anim_smalls.Add(index,SuccessCard);

				yield return new WaitForSeconds(0.5f);
				
			}
			//nothing
			else if(tag_small ==0)
			{
				smallGameTimesLabel.text = (int.Parse(smallGameTimesLabel.text)-1).ToString();
				GetComponent<AudioSource>().volume = 0.1f;
                aso.PlayOneShot(smallPrizeClip);
				if(GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index+"/"+index))
					GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index+"/"+index).SetActive(false);
				
				GameObject	Net_Anim = Instantiate(Resources.Load("SmallGame/SpiderMan/Net_Anim")) as GameObject;
				Net_Anim.transform.parent = GameObject.Find("/UI Root/Camera/SmallGame/SmalGameBackground/chooseBtn/"+index).transform;
				Net_Anim.transform.localScale = Vector3.one;
				Net_Anim.transform.eulerAngles = Vector3.zero;
				Net_Anim.transform.localPosition = Vector3.zero;
				anim_smalls.Add(index,Net_Anim);
				yield return new WaitForSeconds(1.5f);
				//删除动画物体
				foreach(KeyValuePair<int,GameObject> ss in anim_smalls)
				{
					ss.Value.transform.parent.localEulerAngles = Vector3.zero;
					Destroy(ss.Value);
				}
				Net_Anim.transform.parent.localEulerAngles = Vector3.zero;
				//恢复图片按钮可点击
				foreach(KeyValuePair<int,GameObject> bb in btn_smalls)
				{
					bb.Value.GetComponent<BoxCollider>().enabled = true;
					bb.Value.SetActive(true);
				}
				anim_smalls.Clear();
				btn_smalls.Clear();
			}
			if(int.Parse(smallGameTimesLabel.text) ==0)
			{
                StartCoroutine(Small_Over(double.Parse(smallPrizeLabel.text)));
                PlayerPrefs.SetInt("SpiderManFlag", 1);
			}
			else
				SmallGame.canSelect = true;
		}
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

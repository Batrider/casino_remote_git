using UnityEngine;
using System.Collections;

public class InstanPrefabs3D : MonoBehaviour {
	// 全部预设，用于随机生成图标
	public Object[] prefabss;
	// 用来标记当前图标的位置信息
	private int curPrefabsFlag = -2;
	// 图标的目标位置
	private int targetPos;
	// 是否到达指定位置:(targetPos-20)
	private bool arrived;
	//父对象脚本
	private stopSelf sScript;
	GameObject targetPre;
	void Start () {
		//装载所有图标预设，用于随机生成图标
 //       prefabss = Resources.LoadAll(SceneManager.PreabsPath);
		
		//该对象将移到的位置
		int vIndex =int.Parse(transform.parent.name);
		targetPos = -180*(((curPrefabsFlag-4*(vIndex-1))%4)+1);
		arrived = false;
		sScript = transform.GetComponentInParent<stopSelf>();
		GetComponent<Animator> ().SetTrigger (gameObject.name);
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//如果该脚本的curPrefabsFlag变量不为-1，则表示该脚本所依附的对象是目标对象，
		//根据curPrefabsFlag变量值将该对象移至相应的位置
		if(curPrefabsFlag != -1&&!arrived&&curPrefabsFlag != -2)
		{
			transform.Translate(new Vector3(0,-5*Time.fixedDeltaTime,0));
			//每个位置减少20个单位，以做弹簧效果
			if(transform.localPosition.y<targetPos-10||transform.localPosition.y<-670)
			{
				arrived = true;
			}
		}
		//停止的时候到达指定位置
		else if(arrived)
		{
			if(!gameObject.GetComponent<SpringPosition>())
			{
				//添加弹簧脚本，并为对象指定即将到达的位置
				gameObject.AddComponent<SpringPosition>();
				SpringPosition sp = gameObject.GetComponent<SpringPosition>();
				sp.strength = 8;
				sp.target = new Vector3(transform.localPosition.x,targetPos,0);
			}
		}
		
		//如果该列没有停止，标识为-1，则滚动
		if(!sScript.Isstop||curPrefabsFlag == -1)
		{
			transform.Translate(new Vector3(0,-5*Time.fixedDeltaTime,0));
		}
		//图标移动到最底端，考虑下个预设生成的问题
		if(transform.localPosition.y<-600)
		{
			//消失的时候，是否生成目标预设
			if(sScript.Isstop)
			{//指定生成图标
				int vIndex =int.Parse(transform.parent.name);
				//根据目标图标的索引返回该位置图标的名字 *3
				string target = Conversion.SelectIcon(manager.IconNode[vIndex*3 - sScript.curIndexOfPrefabs]);
				Debug.Log("图标的名字:"+target);
				//根据游戏编号返回相应的图标
//                targetPre = Instantiate(Resources.Load(SceneManager.PreabsPath+"/"+target)) as GameObject;
				targetPre.name =target;
				targetPre.transform.parent = transform.parent;
				targetPre.transform.localPosition = new Vector3(-5,0,0);
				targetPre.transform.localScale = Vector3.one;
				targetPre.transform.localEulerAngles = Vector3.zero;
				//				targetPre.AddComponent<InstanPrefabs>();
				//给该对象打上标识,为其后位置的判定做标准 *4
				targetPre.GetComponent<InstanPrefabs3D>().curPrefabsFlag = (vIndex*4-1-sScript.curIndexOfPrefabs);
				int indexOfObject = vIndex*4-1-sScript.curIndexOfPrefabs;
				int objCountIndex = vIndex*3 - sScript.curIndexOfPrefabs;
				//				Debug.Log("indexOfObject:"+indexOfObject);
				//如果是屏幕上显示的15个图标，则把它加进object15里面
				if(indexOfObject!=3&&indexOfObject!=7&&indexOfObject!=11&&indexOfObject!=15&&indexOfObject!=19)
				{
					Debug.Log("Add into object15!");
					if(manager.object15.Count<15)
					{
						manager.object15.Add(objCountIndex,targetPre);
					}
				}
				
				
				sScript.curIndexOfPrefabs++;
				if(sScript.curIndexOfPrefabs == 4)
				{
					//标记这行已经不能动了
					//由于滚轮还需要一段时间才能到达到相应的位置，所以这边须等待点时间
					sScript.DelayTime(vIndex);
					//循环该值
					sScript.curIndexOfPrefabs = 0;
				}
				
				Destroy(gameObject);
			}
			else
			{   //随机生成图标
				//实物奖已删除,所以从0 ~ 15
				//根据目标图标的索引返回该位置图标的名字 *3
				GameObject newPre = Instantiate(prefabss[Random.Range(0,15)]) as GameObject;
				newPre.name = newPre.name.Split(new char[]{'('})[0];
				newPre.transform.parent = transform.parent;
				newPre.transform.localPosition = new Vector3(-5,0,0);
				newPre.transform.localScale = Vector3.one;
				newPre.transform.localEulerAngles = Vector3.zero;
				//				newPre.AddComponent<InstanPrefabs>();
				newPre.GetComponent<InstanPrefabs3D>().curPrefabsFlag = -1;
				Destroy(gameObject);
			}
		}
		
	}
}

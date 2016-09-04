/*                                 
 * 
 *                    			图标索引分布图
 * 
 *                  -------------------------------------
 * 					| 0		4		8		12		16	|
 * 					| 1		5		9		13		17	|
 * 					| 2		6		10		14		18	|
 * 					| 3		7		11		15		19	|
 *                  -------------------------------------
 * 
 * 
 * */
using UnityEngine;
using System.Collections;
public class InstanPrefabs : MonoBehaviour {
	// 全部预设，用于随机生成图标
	private Object[] prefabss;
	// 用来标记当前图标的位置信息
	private int curPrefabsFlag = -2;
	// 图标的目标位置
	private int targetPos;
	// 是否到达指定位置:(targetPos-20)
	private bool arrived;
	//父对象脚本
	private stopSelf sScript;
    public float runSpeed = 6f;
	GameObject targetPre;
	void Start () {
        //装载所有图标预设，用于随机生成图标
        prefabss = transform.parent.parent.GetComponent<StopController>().prefabss;   // Resources.LoadAll(SceneManager.PreabsPath);
        sScript = transform.GetComponentInParent<stopSelf>();
        InitValue();
	}
    void InitValue()
    {
        //该对象将移到的位置
        int vIndex =int.Parse(transform.parent.name);
        targetPos = -150*(((curPrefabsFlag-4*(vIndex-1))%4)+1);
        arrived = false;
    }
	// Update is called once per frame
	void FixedUpdate()
	{
		//如果该脚本的curPrefabsFlag变量不为-1，则表示该脚本所依附的对象是目标对象，
		//根据curPrefabsFlag变量值将该对象移至相应的位置
		if(curPrefabsFlag != -1&&!arrived&&curPrefabsFlag != -2)
		{
            transform.Translate(new Vector3(0,-runSpeed*Time.fixedDeltaTime,0));
			//每个位置减少10个单位，以做弹簧效果
			if(transform.localPosition.y<targetPos-10||transform.localPosition.y<-550)
			{
				arrived = true;
			}
		}
		//停止的时候到达指定位置
		else if(arrived)
		{
			if(!gameObject.GetComponent<SpringPosition2D>())
			{
				//添加弹簧脚本，并为对象指定即将到达的位置
                SpringPosition2D sp = gameObject.AddComponent<SpringPosition2D>();
				sp.strength = 15;
				sp.target = new Vector3(transform.localPosition.x,targetPos,0);
			}
		}
        
        //如果该列没有停止，标识为-1，则滚动
        if(!sScript.Isstop||curPrefabsFlag == -1)
        {
            transform.Translate(new Vector3(0,-8*Time.fixedDeltaTime,0));
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
                //              Debug.Log("图标的名字:"+target);
                //根据游戏编号返回相应的图标
                //targetPre = Instantiate(Resources.Load(SceneManager.PreabsPath+"/"+target)) as GameObject;
                targetPre = InstaniateIcon(prefabss[manager.IconNode[vIndex*3 - sScript.curIndexOfPrefabs]-5]);
                targetPre.name =target;
                //              targetPre.AddComponent<InstanPrefabs>();
                //给该对象打上标识,为其后位置的判定做标准 *4
                targetPre.GetComponent<InstanPrefabs>().curPrefabsFlag = (vIndex*4-1-sScript.curIndexOfPrefabs);
                int indexOfObject = vIndex*4-1-sScript.curIndexOfPrefabs;
                int objCountIndex = vIndex*3 - sScript.curIndexOfPrefabs;
                //              Debug.Log("indexOfObject:"+indexOfObject);
                //如果是屏幕上显示的15个图标，则把它加进object15里面
                if(indexOfObject!=3&&indexOfObject!=7&&indexOfObject!=11&&indexOfObject!=15&&indexOfObject!=19)
                {
                    if(manager.object15.Count<15)
                        manager.object15.Add(objCountIndex,targetPre);
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
                /*
                GameObject poolPre = InstaniateIcon(prefabss[Random.Range(0,15)]);
                poolPre.GetComponent<InstanPrefabs>().curPrefabsFlag = -1;
                Destroy(gameObject);
                */

                transform.localPosition = new Vector3(-5,0,0);
                if(GetComponent<UI2DSprite>())
                {
                    GetComponent<UI2DSprite>().sprite2D = GetComponentInParent<StopController>().sprite15[Random.Range(0,15)];
                    GetComponent<UI2DSprite>().depth = 0;
                }
                GetComponent<InstanPrefabs>().curPrefabsFlag = -1;

            }

            
            
        }
	}
    GameObject InstaniateIcon(Object iconPre)
    {
        GameObject poolPre = Instantiate(iconPre) as GameObject;// ObjectPoolManager.Spawn(iconPre as GameObject);
        poolPre.transform.parent = transform.parent;
        poolPre.transform.localPosition = new Vector3(-5,0,0);
        poolPre.transform.localScale = Vector3.one;
        poolPre.transform.localEulerAngles = Vector3.zero;

        return poolPre;
    }

}

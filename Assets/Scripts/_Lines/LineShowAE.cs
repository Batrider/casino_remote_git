using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowAE : LineShow {
    //线预设
    public Object linePreabs;
    //预设
    public Object czPrefabs;
    bool isStop;
    public override void LineShowMethod(int index, bool flag)
    {
        //返回线上的5个点的下标
        if (flag)
        {
            isStop = false;
            InstantLines(LinePoints(index));
        } else
        {
            isStop = true;
            foreach(KeyValuePair<int,GameObject> kp in linesIns)
            {
                Destroy(kp.Value);
            }
        }
    }
    //实例化“线”
    public void InstantLines (int[] linepoint)
    {
        Vector3 insPoint = Position15 [linepoint [0]];
        StartCoroutine (StartDraw (insPoint,linepoint));
    }
    //移动线
    IEnumerator StartDraw (Vector3 insPoint,int[] linepoint)
    {
        int tagOfline = 0;
        float tempTag = 0;

        //GetComponent<AudioSource>().PlayOneShot(lineClip);
        //实例化生成虫子
        GameObject czPrefab = Instantiate(czPrefabs) as GameObject;
        czPrefab.transform.parent = lineCanve.transform;
        czPrefab.transform.localScale = Vector3.one;
        czPrefab.transform.localEulerAngles = Vector3.zero;
        czPrefab.transform.localPosition = Position15 [linepoint [0]];
        linesIns.Add(linesIns.Count,czPrefab);

        for (int i = 1; i<5;)
        {
			yield return null;
            if(isStop)
            {
                break;
            }
            try{
                insPoint = Vector3.MoveTowards(insPoint, Position15 [linepoint [i]], moveSpeed * Time.fixedDeltaTime);
                float angle = Vector3.Angle(Vector3.down,Position15 [linepoint [i]]-Position15[linepoint [i-1]]) - 90;
                tempTag+= Time.fixedDeltaTime;
                
                if (Mathf.Abs(insPoint.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
                }
                if(tempTag>0.01f)//两点之间相隔距离大于0.2,则画图）
                {
                    tempTag = 0;
                    tagOfline++;
                    Debug.Log("tagOfline:"+tagOfline);
                    //同时移动虫子的位置
                    czPrefab.transform.parent = lineCanve.transform;
                    czPrefab.transform.localScale = Vector3.one;
                    czPrefab.transform.localEulerAngles = new Vector3(0,0,angle-90);
                    czPrefab.transform.localPosition = insPoint;
                        //虫子所在的地方生成宝石
						GameObject line = Instantiate(linePreabs) as GameObject;
						line.GetComponent<UISprite>().spriteName = (tagOfline%3).ToString();
						line.transform.parent = lineCanve.transform;
						line.transform.localScale = Vector3.one;
						line.transform.localEulerAngles = new Vector3(0,0,angle);
						line.transform.localPosition = insPoint;
                    
					linesIns.Add(linesIns.Count,line);
                }
            }
            catch(MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
    }
}




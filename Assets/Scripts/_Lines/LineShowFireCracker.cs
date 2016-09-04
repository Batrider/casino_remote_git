using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineShowFireCracker : LineShow {
    [Tooltip("主移动物体,如子弹……")]
    public Object
        moveObjectPrefabs;
    [Tooltip("用于表演发射的物体，如枪支……")]
    public Object performPrefabs;
    
    public override void LineShowMethod(int index, bool flag)
    {
        //返回线上的5个点的下标
        if (flag)
        {
            InstantLines(LinePoints(index));
        } else
        {
            foreach (KeyValuePair<int,GameObject> kp in linesIns)
            {
                Destroy(kp.Value);
            }
        }
    }
    //实例化“线”
    public void InstantLines(int[] linepoint)
    {
        Vector3 insPoint = Position15 [linepoint [0]];
        StartCoroutine(StartDraw(insPoint, linepoint));
    }
    //移动线
    IEnumerator StartDraw(Vector3 insPoint, int[] linepoint)
    {
        int tagOfline = 0;
        float tempTag = 0;
        //GetComponent<AudioSource>().PlayOneShot(lineClip);

        
        //实例化生成移动主体
        GameObject moveObject = Instantiate(moveObjectPrefabs) as GameObject;
        moveObject.transform.parent = lineCanve.transform;
        moveObject.transform.localScale =0.5f* Vector3.one;
        moveObject.transform.localEulerAngles = Vector3.zero;
        moveObject.transform.localPosition = Position15 [linepoint [0]];

        for (int i = 1; i<5;)
        {
            yield return null;
            try
            {
                insPoint = Vector3.MoveTowards(insPoint, Position15 [linepoint [i]], moveSpeed * Time.fixedDeltaTime);
                //float angle = Vector3.Angle(Vector3.down, Position15 [linepoint [i]] - Position15 [linepoint [i - 1]]) - 90;
                tempTag += Time.fixedDeltaTime;
                
                if (Mathf.Abs(insPoint.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
                }
                if (tempTag > 0.01f)//两点之间相隔距离大于0.2,则画图）
                {
                    tempTag = 0;
                    tagOfline++;
                    Debug.Log("tagOfline:" + tagOfline);
                    //同时移动虫子的位置
                    moveObject.transform.localPosition = insPoint;
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
        GameObject fireCracker = Instantiate(performPrefabs) as GameObject;
        fireCracker.transform.parent = lineCanve.transform;
        fireCracker.transform.localScale = 0.5f*Vector3.one;
        fireCracker.transform.localEulerAngles = Vector3.zero;
        fireCracker.transform.localPosition = moveObject.transform.localPosition;
        fireCracker.GetComponent<UISpriteAnimation>().enabled = true;
        Destroy(moveObject);
        //此处应有爆炸声
        //GetComponent<AudioSource>().PlayOneShot(lineClip);
        //执行金币飞行效果
        base.StartGoldFlyIntoAccount(fireCracker.transform.localPosition);
        yield return new WaitForSeconds(0.4f);
        Destroy(fireCracker);
    }
}

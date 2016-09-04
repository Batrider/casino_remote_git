using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowGhost : LineShow {
    //线预设
    [Tooltip("线及制造残影的预设")]
    public Object targetPrefabs;
    [Tooltip("星光效果预设")]
    public Object trailStarPre;
    public override void LineShowMethod(int index, bool flag)
    {
        //返回线上的5个点的下标
        if (flag)
        {
            InstantLines(LinePoints(index));
        } else
        {
            foreach(KeyValuePair<int,GameObject> kp in linesIns)
            {
                Destroy(kp.Value);
            }
        }
    }
    //实例化“线”
    public void InstantLines (int[] linepoint)
    {
        //移动的物体
        GameObject target = Instantiate(targetPrefabs) as GameObject;
        target.transform.parent = lineCanve.transform;
        target.transform.localScale = Vector3.one;
        target.transform.localEulerAngles = Vector3.zero;
        target.transform.localPosition = Position15 [linepoint [0]];

        StartCoroutine (StartDraw (target.transform,linepoint));
        linesIns.Add(linesIns.Count, target);
    }
    //移动物体，制造残影
    IEnumerator StartDraw(Transform target, int[] linepoint)
    {
        //划线的时候的声效
        //audio.PlayOneShot(lineClip);
        float tempTag = 0;
        for (int i = 1; i<5;)
        {
            yield return null;
            try
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, Position15 [linepoint [i]], moveSpeed * Time.deltaTime);
                tempTag += Time.fixedDeltaTime;
                
                if (Mathf.Abs(target.localPosition.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
                }
                if (tempTag > 0.1f)//如果需要星光，则每隔0.1秒生成！
                {
                    if (trailStarPre != null)
                    {
                        tempTag = 0;
                        GameObject trailStar = Instantiate(trailStarPre) as GameObject;
                        trailStar.transform.parent = lineCanve.transform;
                        trailStar.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        trailStar.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                        trailStar.transform.localPosition = target.localPosition + new Vector3(-5, Random.Range(10, -20), 0);
                        Debug.Log("Instaniate======================================");
                    }
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
        //移动到了最后一个点：可以做金币飞行效果——————>飞到账户中！
        //………
        //………
        //执行金币飞行效果
        base.StartGoldFlyIntoAccount(target.localPosition);
    }
}

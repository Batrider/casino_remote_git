using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowTrail : LineShow
{
    //线预设
    [Tooltip("线及制造拖尾的预设")]
    public Object
        targetPrefabs;
    [Tooltip("拖尾星光预设")]
    public Object
        trailStarPre;
    public bool starRandomRotate;
    public bool rotateLeadObj;
    public int RandomNeX = -5;
    public int RandomNeY = -20;
    public int RandomY = 10;
    

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
        //移动的物体
        GameObject target = Instantiate(targetPrefabs) as GameObject;
        target.transform.parent = lineCanve.transform;
        target.transform.localScale = new Vector3(-1, 1, 1);
        target.transform.localEulerAngles = Vector3.zero;
        target.transform.localPosition = Position15 [linepoint [0]];

        StartCoroutine(StartDraw(target.transform, linepoint));
        linesIns.Add(linesIns.Count, target);
    }
    //移动物体，制造拖尾
    IEnumerator StartDraw(Transform target, int[] linepoint)
    {
//      audio.PlayOneShot(lineClip);
        float tempTag = 0;
        float angle = 0;
        for (int i = 1; i<5;)
        {
            yield return null;
            try
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, Position15 [linepoint [i]], moveSpeed * Time.deltaTime);
                if(rotateLeadObj)
                {
                    angle = Vector3.Angle(Vector3.down, Position15 [linepoint [i]] - Position15 [linepoint [i - 1]]) - 90;
                    target.transform.localEulerAngles = new Vector3(0, 0, angle);
                }
                
                tempTag += Time.fixedDeltaTime;
                
                if (Mathf.Abs(target.localPosition.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
                }
                if (tempTag > 0.1f)//两点之间相隔距离大于0.2,则画图）
                {
                    if (trailStarPre != null)
                    {
                        tempTag = 0;
                        GameObject trailStar = Instantiate(trailStarPre) as GameObject;
                        trailStar.transform.parent = lineCanve.transform;
                        trailStar.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        if(starRandomRotate)
                        {
                            trailStar.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                        }
                        else
                        {
                            trailStar.transform.localEulerAngles =new Vector3(0, 0, angle);
                        }
                        trailStar.transform.localPosition = target.localPosition + new Vector3(RandomNeX, Random.Range(RandomNeY, RandomY), 0);
                     //   Debug.Log("Instaniate======================================");
                    }
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
        ParticleSystem[] pss =  GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in pss)
        {
            ps.Stop();
        }
        //移动到了最后一个点：可以做金币飞行效果——————>飞到账户中！
        //………
        //………
        base.StartGoldFlyIntoAccount(Position15 [linepoint [4]]);
        
        
    }
}

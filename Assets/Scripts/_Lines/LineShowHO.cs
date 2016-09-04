using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowHO : LineShow
{
    //虫子预设
    public Object star_Prefabs;
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
        
        //实例化生成子弹
        GameObject starPrefab = Instantiate(star_Prefabs) as GameObject;
        starPrefab.transform.parent = lineCanve.transform;
        starPrefab.transform.localScale = Vector3.one;
        starPrefab.transform.localEulerAngles = Vector3.zero;
        starPrefab.transform.localPosition = Position15 [linepoint [0]];
        linesIns.Add(linesIns.Count, starPrefab);
            
        for (int i = 1; i<5;)
        {
            yield return null;
            if (isStop)
            {
                break;
            }
            try
            {
                insPoint = Vector3.MoveTowards(insPoint, Position15 [linepoint [i]], moveSpeed * Time.fixedDeltaTime);
                float angle = Vector3.Angle(Vector3.down, Position15 [linepoint [i]] - Position15 [linepoint [i - 1]]) - 90;
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
                    //同时移动位置
                    starPrefab.transform.parent = lineCanve.transform;
                    starPrefab.transform.localScale = Vector3.one;
                    starPrefab.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
                    starPrefab.transform.localPosition = insPoint;
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
        base.StartGoldFlyIntoAccount(insPoint);
        if (starPrefab.transform.FindChild("bullet"))
        {
            Destroy(starPrefab.transform.FindChild("bullet").gameObject);
            starPrefab.transform.FindChild("fire 16").GetComponent<ParticleEmitter>().emit = false;

        }else if(transform.GetComponentsInChildren<ParticleSystem>().Length>0)
        {
            ParticleSystem[] pss = transform.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem ps in pss)
            {
                ps.enableEmission = false;
            }
        }
    }
}

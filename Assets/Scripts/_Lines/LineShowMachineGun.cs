using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowMachineGun : LineShow {
    [Tooltip("主移动物体,如子弹……")]
    public Object
        moveObjectPrefabs;
    [Tooltip("用于表演发射的物体，如枪支……")]
    public Object performPrefabs;
    GameObject gun;
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
        //如果有表演物体的话
        if (performPrefabs != null)
        {
            float angleGun = Position15 [linepoint [1]].y > Position15 [linepoint [0]].y ? Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right) : -Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right);
            gun = Instantiate(performPrefabs) as GameObject;
            gun.transform.parent = lineCanve.transform;
            gun.transform.localScale = Vector3.one;
            gun.transform.localEulerAngles = new Vector3(0, 0, angleGun);
            if(angleGun==0)
            {
                gun.transform.localPosition = Position15 [linepoint [0]]-new Vector3(100,15,0);
            }else if(Mathf.Abs(angleGun)>50){
                gun.transform.localPosition = Position15 [linepoint [0]] - new Vector3(80*Mathf.Cos(angleGun),10000*Mathf.Sin(angleGun), 0);

            }else
            {
                gun.transform.localPosition = Position15 [linepoint [0]] - new Vector3(100*Mathf.Cos(angleGun), 280*Mathf.Sin(angleGun), 0);
            }
            Debug.Log("angleGun:"+angleGun);        
            //此处应有枪声
            //GetComponent<AudioSource>().PlayOneShot(lineClip);
        }
        StartCoroutine(LineDraw(linepoint));
        
    }
    IEnumerator LineDraw(int[] linepoint)
    {
        for(int i = 0;i<4;i++)
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(StartDraw(linepoint));
        }
        yield return new WaitForSeconds(1f);
        //执行金币飞行效果
        base.StartGoldFlyIntoAccount(Position15 [linepoint [4]]);
    }
    //移动线
    IEnumerator StartDraw(int[] linepoint)
    {
        Vector3 insPoint = Position15 [linepoint [0]];
        int tagOfline = 0;
        float tempTag = 0;
        //GetComponent<AudioSource>().PlayOneShot(lineClip);
        gun.transform.GetChild(0).GetComponent<UISpriteAnimation>().enabled = true;
        gun.transform.GetChild(0).GetComponent<UISpriteAnimation>().ResetToBeginning();
        
        float angleGun = Position15 [linepoint [1]].y > Position15 [linepoint [0]].y ? Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right) : -Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right);
        
        //实例化生成移动主体
        GameObject moveObject = Instantiate(moveObjectPrefabs) as GameObject;
        moveObject.transform.parent = lineCanve.transform;
        moveObject.transform.localScale = Vector3.one;
        moveObject.transform.localEulerAngles = new Vector3(0,0,angleGun);
        moveObject.transform.localPosition = Position15 [linepoint [0]];
//        linesIns.Add(linesIns.Count, moveObject);
        for (int i = 1; i<5;)
        {
            yield return null;
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
                    moveObject.transform.localEulerAngles = new Vector3(0, 0, angle);
                    moveObject.transform.localPosition = insPoint;
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
    }

}

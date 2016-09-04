using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineShowParticle : LineShow
{
    [Tooltip("主移动物体,如子弹……")]
    public Object
        moveObjectPrefabs;
    [Tooltip("划线结束后留下的物体，如弹孔……")]
    public Object
        remnantPrefabs;
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
        //如果有表演物体的话
        if (performPrefabs != null)
        {
            float angleGun = Position15 [linepoint [1]].y > Position15 [linepoint [0]].y ? Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right) : -Vector3.Angle((Position15 [linepoint [1]] - Position15 [linepoint [0]]), Vector3.right);
            GameObject gun = Instantiate(performPrefabs) as GameObject;
            gun.transform.parent = lineCanve.transform;
            gun.transform.localScale = Vector3.one;
            gun.transform.localEulerAngles = new Vector3(0, 0, angleGun);
            gun.transform.localPosition = Position15 [linepoint [0]] - new Vector3(0, 10, 0);
            gun.GetComponent<Animator>().SetTrigger("fire");
            linesIns.Add(linesIns.Count, gun);
            //此处应有枪声
            //GetComponent<AudioSource>().PlayOneShot(lineClip);
        }
        
        //实例化生成移动主体
        GameObject moveObject = Instantiate(moveObjectPrefabs) as GameObject;
        moveObject.transform.parent = lineCanve.transform;
        moveObject.transform.localScale = Vector3.one;
        moveObject.transform.localEulerAngles = Vector3.zero;
        moveObject.transform.localPosition = Position15 [linepoint [0]];
        linesIns.Add(linesIns.Count, moveObject);
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
                    Debug.Log("tagOfline:" + tagOfline);
                    //同时移动虫子的位置
                    moveObject.transform.parent = lineCanve.transform;
                    moveObject.transform.localScale = Vector3.one;
                    moveObject.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
                    moveObject.transform.localPosition = insPoint;
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
        }
        //执行金币飞行效果
        base.StartGoldFlyIntoAccount(moveObject.transform.localPosition);
        

        if (remnantPrefabs != null)
        {
            GameObject bulletHole = Instantiate(remnantPrefabs) as GameObject;
            bulletHole.transform.parent = lineCanve.transform;
            bulletHole.transform.localScale = Vector3.one;
            bulletHole.transform.localEulerAngles = Vector3.zero;
            bulletHole.transform.localPosition = moveObject.transform.localPosition;
            linesIns.Add(linesIns.Count, bulletHole);
        }
        Destroy(moveObject.transform.GetChild(1).gameObject);
        yield return new WaitForSeconds(0.1f);
        moveObject.GetComponentInChildren<ParticleEmitter>().emit = false;
    }
}

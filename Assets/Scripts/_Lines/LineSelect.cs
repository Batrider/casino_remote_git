using UnityEngine;
using System.Collections;
using System;

public class LineSelect : MonoBehaviour {
    public GameObject lineOBJ;
    public Sprite[] lineSprites;
    //游戏开始时，实例化玩家选择的线
    void Start()
    {
        //实例化某某线
    }
    //主界面线数选择
    public void MenuLineShowMethod(int index,int LineCountDelta)
    {
        CancelInvoke("CancelInvokeShowLine");
        TweenAlpha.Begin(lineOBJ,0.3f,1);
        //将对应线数的图绑定给lineOBJ
        lineOBJ.GetComponent<UI2DSprite>().sprite2D = lineSprites [index /LineCountDelta - 1]; 
        //执行图标沿线运动
//       StartCoroutine(RoundTheLine(index));
        Invoke("CancelInvokeShowLine", 2f);
    }
    void CancelInvokeShowLine()
    {
        TweenAlpha.Begin(lineOBJ,0.3f,0);
    }
    /*
    IEnumerator RoundTheLine(int index)
    {
        for (int i = 1; i<=index; i++)
        {
            Vector3 insPoint = Position15 [LinePoints(i) [0]];
            StartCoroutine(StartRound(insPoint, LinePoints(i)));
            yield return new WaitForSeconds(0.02f);
        }
    }
    //图标开始移动
    IEnumerator StartRound(Vector3 insPoint, int[] linepoint)
    {
        GameObject icon = Instantiate(firePrefabs) as GameObject;
        icon.transform.parent = lineCanve.transform;
        icon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        icon.transform.localEulerAngles = Vector3.zero;
        icon.transform.localPosition = Position15 [linepoint [0]];
        linesIns.Add(linesIns.Count, icon);
        
        
        float tempTag = 0;
        for (int i = 1; i<5;)
        {
            yield return null;
            try
            {
                insPoint = Vector3.MoveTowards(insPoint, Position15 [linepoint [i]], 2*moveSpeed * Time.fixedDeltaTime);
                float angle = Vector3.Angle(Vector3.down, Position15 [linepoint [i]] - Position15 [linepoint [i - 1]]) - 90;
                tempTag += Time.fixedDeltaTime;
                
                if (Mathf.Abs(insPoint.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
                }
                if (tempTag > 0.01f)//两点之间相隔距离大于0.2,则画图）
                {
                    tempTag = 0;
                    //移动位置
                    icon.transform.parent = lineCanve.transform;
                    icon.transform.localScale = Vector3.one;
                    icon.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
                    icon.transform.localPosition = insPoint;
                }
            } catch (MissingReferenceException uebg)
            {
                Debug.Log(uebg.Message);
                break;
            }
            Debug.Log("still===========================================");
        }
        //动画结束，删除图标
        Destroy(icon);
    } 
    */
}

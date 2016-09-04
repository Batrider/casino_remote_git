using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineShowSM : LineShow {
    //线预设
    public Object linePreabs;

    public override void LineShowMethod(int index, bool flag)
    {
        //返回线上的5个点的下标
        if (flag)
        {
            InstantLines(LinePoints(index));
        } else
        {
            StopCoroutine("StartDraw");
            foreach(KeyValuePair<int,GameObject> kp in linesIns)
            {
                Destroy(kp.Value);
            }
        }
    }
    //实例化“线”
    public void InstantLines (int[] linepoint)
    {
        GameObject line = Instantiate(linePreabs) as GameObject;
        line.transform.parent = lineCanve.transform;
        line.transform.localScale = Vector3.one;
        line.transform.localEulerAngles = Vector3.zero;
        line.transform.localPosition = Position15 [linepoint [0]];
        StartCoroutine (StartDraw (line.transform,linepoint));
        linesIns.Add(linesIns.Count, line);
    }
    //移动线
    IEnumerator StartDraw (Transform line,int[] linepoint)
    {
        //      GetComponent<AudioSource>().PlayOneShot(lineClip);
        for (int i = 1; i<5;)
        {
            yield return null;
            try{
                line.localPosition = Vector3.MoveTowards(line.localPosition, Position15 [linepoint [i]], moveSpeed * Time.deltaTime);
                if (Mathf.Abs(line.localPosition.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    i++;
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

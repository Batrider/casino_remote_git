using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineShow3D : LineShow
{
    public Vector3[] modelPosition = new Vector3[15];
    private int[] linsPoints;
    public Object linePreabs;
//    public Transform firstAnchor;
	private Dictionary<int,GameObject> linesIns = new Dictionary<int,GameObject>();
    public override void LineShowMethod (int index, bool flag)
    {
        //返回线上的5个点的下标
        if (flag)
        {
            linsPoints = LinePoints(index);
            InstantLines(linsPoints);
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
 //       line.transform.parent = firstAnchor;
        line.transform.localScale = Vector3.one;
        line.transform.localEulerAngles = Vector3.zero;
        line.transform.position = Position15 [linepoint [0]];
        StartCoroutine (StartDraw (line.transform,linepoint));
        linesIns.Add(linesIns.Count, line);
    }
    //移动线
  IEnumerator StartDraw (Transform line,int[] linepoint)
  {
        GetComponent<AudioSource>().PlayOneShot(lineClip);
        float startPositionX = Position15 [linepoint [0]].x;
        float startPositionY = Position15 [linepoint [0]].y;
        Debug.Log("linepoint [0]:"+linepoint [0]);
        for (int i = 1; i<5;)
        {
            yield return null;
            try{
                line.position = Vector3.MoveTowards(line.position, Position15 [linepoint [i]], moveSpeed * Time.deltaTime);
                if (Mathf.Abs(line.position.x - Position15 [linepoint [i]].x) < 0.01f)
                {
                    startPositionX = Position15 [linepoint [i]].x;
                    startPositionY = Position15 [linepoint [i]].y;
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
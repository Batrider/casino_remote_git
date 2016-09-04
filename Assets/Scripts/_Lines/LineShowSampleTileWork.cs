    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    
    public class LineShowSampleTileWork : LineShow
    {
        //线预设
        public Object brickPreabs;        
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
                    if (tempTag > 0.01f)//间隔时间大于0.01s,则画图）
                    {
                        tempTag = 0;
                        tagOfline++;
                        Debug.Log("tagOfline:" + tagOfline);
                        GameObject line = Instantiate(brickPreabs) as GameObject;
                        line.transform.parent = lineCanve.transform;
                        line.transform.localScale = Vector3.one;
                        line.transform.localEulerAngles = new Vector3(0, 0, angle);
                        line.transform.localPosition = insPoint;
                        
                        linesIns.Add(linesIns.Count, line);
                    }
                } catch (MissingReferenceException uebg)
                {
                    Debug.Log(uebg.Message);
                    break;
                }
            }
            //执行金币飞行效果
            base.StartGoldFlyIntoAccount(insPoint);
        }
    }

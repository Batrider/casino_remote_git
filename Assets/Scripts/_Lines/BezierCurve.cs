using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve  {
    
    public List<Vector3> CreateCurve(List<Vector3> list,float k = 0.6f,float d=0.005f) {
        List<Vector3> midPointsList = new List<Vector3>();
        //计算中点
        for (int i = 0; i < list.Count; i++)
        {
            int nexti = (i + 1) % list.Count;
            float x = (list[i].x + list[nexti].x) * 0.5f;
            float z = (list[i].z + list[nexti].z) * 0.5f;
            float y = (list[i].y+ list[nexti].y) * 0.5f;//y轴暂时无用 现在只再3d 场景中的2d面做曲线
            Vector3 midPoint = new Vector3(x, y, z);
            midPointsList.Add(midPoint);
        }
        
        Vector3[] extraPoints = new Vector3[2*list.Count];
        //平移中点
        for (int i = 0; i < list.Count; i++)
        {
            int nexti = (i + 1) % list.Count;
            int backi = (i + list.Count - 1) % list.Count;
            Vector3 midinmid = new Vector3();
            midinmid.x = (midPointsList[i].x + midPointsList[backi].x) * 0.5f;
            midinmid.z = (midPointsList[i].z + midPointsList[backi].z) * 0.5f;
            float offsetx = list[i].x - midinmid.x;
            float offsetz = list[i].z - midinmid.z;
            int extraIndex = 2 * i;
            Vector3 extraPoint = new Vector3();
            extraPoint.x = midPointsList[backi].x + offsetx;
            extraPoint.z = midPointsList[backi].z + offsetz;
            extraPoint.y = midPointsList[backi].y;
            
            //朝 originPoint[i]方向收缩   
            
            float addx = (extraPoint.x - list[i].x) * k;
            float addz = (extraPoint.z - list[i].z) * k;
            extraPoint.x = list[i].x + addx;
            extraPoint.z = list[i].z + addz;
            extraPoints[extraIndex] = extraPoint;
            
            int extranexti = (extraIndex + 1) % (2 * list.Count);
            Vector3 extraNext = new Vector3();
            extraNext.x = midPointsList[i].x + offsetx;
            extraNext.z = midPointsList[i].z + offsetz;
            extraNext.y = midPointsList[i].y;
            
            addx = (extraNext.x - list[i].x) * k;
            addz = (extraNext.z - list[i].z) * k;
            extraNext.x = list[i].x + addx;
            extraNext.z = list[i].z + addz;
            extraPoints[extranexti] = extraNext;
        }
        
        
        Vector3[] controPoint = new Vector3[4];
        List<Vector3> curvePoint = new List<Vector3>();
        int iii = 0;
        //生成4个控制点，产生贝塞尔曲线
        for (int i = 0; i < list.Count-1; i++)
        {
            controPoint[0] = list[i];
            int extraindex = 2 * i;
            controPoint[1] = extraPoints[extraindex + 1];
            int extranexti = (extraindex + 2) % (2 * list.Count);
            controPoint[2] = extraPoints[extranexti];
            int nexti = (i + 1) % list.Count;
            controPoint[3] = list[nexti];
            if (i == 0 || i == list.Count - 2)
            {
                if(i ==0){
                    curvePoint.Add(list[i]);
                    
                }
                else { 
                    curvePoint.Add(list[nexti]);
                }
            }
            else
            {
                float u = 1f;//u的步长决定曲线的疏密;
                
                while (u >= 0.0f)
                {
                    
                    float px = bezier3funcX(u, controPoint);
                    float pz = bezier3funcZ(u, controPoint);
                    u -= d;
                    
                    Vector3 temp = new Vector3(px, controPoint[1].y, pz);
                    curvePoint.Add(temp);
                    
                    
                }
            }
        }
        
        
        return curvePoint;
        
    }
    //三次bezier
    private float bezier3funcX(float uu, Vector3[] array)
    {
        float part0 = array[0].x * uu * uu * uu;
        float part1 = 3 * array[1].x * uu * uu *(1 - uu);
        float part2 = 3 * array[2].x * uu * (1 - uu) * (1 - uu);
        float part3 = array[3].x * (1 - uu) * (1 - uu) * (1 - uu);
        return part0 + part1 + part2 + part3;
    }
    
    //三次bezier
    private float bezier3funcZ(float uu, Vector3[] array)
    {
        float part0 = array[0].z * uu * uu * uu;
        float part1 = 3 * array[1].z * uu * uu * (1 - uu);
        float part2 = 3 * array[2].z * uu * (1 - uu) * (1 - uu);
        float part3 = array[3].z * (1 - uu) * (1 - uu) * (1 - uu);
        return part0 + part1 + part2 + part3;
    }
}
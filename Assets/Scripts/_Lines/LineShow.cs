using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
public class LineShow :MonoBehaviour{
    //第一个点坐标 以及 相邻的点的位移 OffsetX,OffsettY
    private Vector2 offset = new Vector2();
    private Vector3 firstPoint = new Vector3();
    //15个图标的位置坐标
    protected Vector3[] Position15 = new Vector3[15];
    //线移动速度
    public float moveSpeed = 0f;
    //线的父物体
    public GameObject lineCanve;
    //连线声效
    public AudioClip lineClip;
    //线存储数据结构
    protected Dictionary<int,GameObject> linesIns = new Dictionary<int,GameObject>();
    void Start()
    {
        Invoke("SetPointVale", 1f);
    }
    /// <summary>
    /// 线调用方法
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="flag">If set to <c>true</c> flag.</param>
    public virtual void LineShowMethod(int index, bool flag)
    {
    }
    /// <summary>
    /// 返回线上连上的5个点
    /// </summary>
    /// <returns>The points.</returns>
    /// <param name="index">Index.</param>
    protected int[] LinePoints(int index)
    {
        int[] iconIndex = new int[5];
        //返回线上连上的5个点
        SecurityParser sp = new SecurityParser();
        string xmlPath = "XMLData/GameSlot";
        
        Object xml = Resources.Load(xmlPath);
        sp.LoadXml(xml.ToString());
        System.Security.SecurityElement se = sp.ToXml();
        foreach (System.Security.SecurityElement child in se.Children)
        {
            if(int.Parse(child.Attribute("id"))==index)
            {
                iconIndex[0] = int.Parse(child.Attribute("offset1"))-1;
                iconIndex[1] = int.Parse(child.Attribute("offset2"))-1;
                iconIndex[2] = int.Parse(child.Attribute("offset3"))-1;
                iconIndex[3] = int.Parse(child.Attribute("offset4"))-1;
                iconIndex[4] = int.Parse(child.Attribute("offset5"))-1;
            }
        }
        return iconIndex;
    }
    //为第一个点和点偏移动态赋值
    void SetPointVale()
    {
        SecurityParser sp = new SecurityParser();
        string xmlPath = "XMLData/LinePoint";
        Object xml = Resources.Load(xmlPath);
        sp.LoadXml(xml.ToString());
        System.Security.SecurityElement se = sp.ToXml();
        foreach (System.Security.SecurityElement child in se.Children)
        {
            if(int.Parse(child.Attribute("id"))==manager.GAME_ID)
            {
                firstPoint.x = float.Parse(child.Attribute("pointX"));
                firstPoint.y = float.Parse(child.Attribute("pointY"));
                firstPoint.z = float.Parse(child.Attribute("pointZ"));
                offset.x = float.Parse(child.Attribute("offsetX"));
                offset.y = float.Parse(child.Attribute("offsetY"));
            }
        }
        //转换15个坐标点
        int index = 0;
        for (int i = 0; i<5; i++)
        {
            for(int j = 0;j<3;j++)
            {
                Position15[index] =new Vector3(firstPoint.x + i*offset.x,firstPoint.y + j*offset.y,firstPoint.z);
                index++;
            }
        }
    }
    protected void StartGoldFlyIntoAccount(Vector3 pos)
    {
        GameObject.FindGameObjectWithTag("GoldPanel").GetComponent<GoldFlyPanel>().GoldPanelStart(pos);
    }

}

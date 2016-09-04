using UnityEngine;
using System.Collections;

public class EllipseInstaniate : MonoBehaviour {
    public Transform[] ho12;
    public float ellipseA;
    public float ellipseB;
    Ellipse ep;
    public float angle;
    private float deltaAngle;
    private float countAngle;
    private UISprite[] uis = new UISprite[12];
    float time;
    void Start()
    {
        ep = new Ellipse(ellipseA, ellipseB);
        deltaAngle =Mathf.PI/6;
        for (int j = 0; j<ho12.Length; j++)
        {
            uis[j] = ho12[j].GetComponent<UISprite>();
        }
        for (int k = 0; k<12; k++)
        {
            if(k<6)
                uis[k].depth = k;
            else
                uis[k].depth = 11-k;
        }
    }
    void FixedUpdate()
    {
        countAngle = countAngle + 0.01f;
        angle = angle + 0.01f;
        for (int i = 0; i<12; i++)
        {
            ho12[i].localPosition = ep.XYPoint(angle+i*deltaAngle);
            ho12[i].localScale = 0.25f * (3 - Mathf.Cos(angle+i*deltaAngle)) * Vector3.one;
            
            if(countAngle>deltaAngle)
            {
                uis[i].depth = uis[i].depth + (int)Mathf.Sign(Mathf.Sin(angle+i*deltaAngle));
            }
        }
        if(countAngle>deltaAngle)
        {
            countAngle = 0;
        }
    }


}

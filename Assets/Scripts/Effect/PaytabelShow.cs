using UnityEngine;
using System.Collections;

public class PaytabelShow : MonoBehaviour {
    public Color titleColor;
    public UILabel[] titleLabels;
    public Color contentColor;
    public UILabel[] contentLabels;
    public Color lineColor;
    public UI2DSprite[] lineImages;
    public Color numColorLeft;
    public Color numColorRight;
    public GameObject[] icons;
    void Awake()
    {
        SetColor();
    }
    public void ShowMethod()
    {
        
        TweenAlpha ta = GetComponent<TweenAlpha>();
        if (GetComponent<UIPanel>().alpha < 1)
        {
            ta.from = 0.0f;
            ta.to = 1.0f;
            ta.PlayForward();
        } else
        {
            ta.PlayReverse();
        }

    }
    //设置字体的颜色
    public void SetColor()
    {
        foreach (UILabel ul in titleLabels)
        {
            ul.color = titleColor;
        }
        foreach (UILabel ul2 in contentLabels)
        {
            ul2.color = contentColor;
        }
        foreach (UI2DSprite us in lineImages)
        {
            us.color = lineColor;
        }
        foreach (GameObject go in icons)
        {
            UILabel[] uls = go.GetComponentsInChildren<UILabel>();
            foreach(UILabel ul in uls)
            {
                if(ul.name == "multiple")
                    ul.color = numColorRight;
                else
                    ul.color = numColorLeft;
            }
        }
    }
}

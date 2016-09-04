using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;
using System.Collections.Generic;
public class SmallGameTipsManager : MonoBehaviour {
    public static SmallGameTipsManager Instance;
    public string tipPlayerPrefabs;
//    public UILabel tipText;
    public GameObject tipBox;
    public GameObject finger;
    public Object xml;
    public GameObject tipLabel;
    string tipStr;
    public static float timer = 1000;
    void Start()
    {
        tipStr = ReadTheXml();
    }
    void Update()
    {
        if(!manager.smallgame||manager.autoSpinTimess>0)
        {
            return;
        }
        if (timer > 999) return;//表明游戏尚未开始，不进行计时
        timer += Time.deltaTime;
        if(timer>8f&&timer<100f)
        {
            timer = 0;
            ShowTip();
        }
    }

    public void ShowTip()
    {
        GameObject tipObj = Instantiate(tipLabel) as GameObject;
        tipObj.transform.parent = this.transform;
        tipObj.transform.localScale = Vector3.one;
        tipObj.GetComponent<UILabel>().text = tipStr;
    }
    void OnEnable()
    {
        Instance = this;
  //      Debug.Log("Enable script!");
        
//        ReadTheXml();
    }
    string ReadTheXml()
    {
        string attrbute = string.Empty;
        if(Localization.language == "English")
        {
            attrbute = "smallTip_EN";
        }else
        {
            attrbute = "smallTip_CN";
        }

        SecurityParser sp = new SecurityParser();
        sp.LoadXml(xml.ToString());
        SecurityElement se = sp.ToXml();
        foreach (SecurityElement child in se.Children)
        {
            if(manager.GAME_ID ==int.Parse(child.Attribute("id")))
            {
                return child.Attribute(attrbute);
            }
        }
        return "";
    }
    public void OpenOrCloseTip()
    {
        tipBox.SetActive(!tipBox.activeSelf);
    }
    public bool CheckTip()
    {
        bool isTipShow = true;//= PlayerPrefs.GetInt(tipPlayerPrefabs) == 0;
        if (isTipShow)
        {
            tipBox.SetActive(true);
            if(finger != null)
                finger.SetActive(true);
        }
        return isTipShow;
    }
    public void CloseFingerTip()
    {
        if(finger != null)
            finger.SetActive(false);
        //如果要开启每次小游戏都提示，请注释下行代码
        //PlayerPrefs.SetInt(tipPlayerPrefabs,1);
    }
    void OnDisable()
    {
 //       Debug.Log("Disable script!");
    }
}

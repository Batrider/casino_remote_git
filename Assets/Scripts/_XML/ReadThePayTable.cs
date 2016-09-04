using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;
using LitJson;
using System.Collections.Generic;

public class PaytableInfoList
{
    public List<PaytableInfo> slotpaids;
    public List<PayCountInfo> slotcounts;
}
public class PaytableInfo {
    
    public int iconId;
    public int paid1;
    public int paid2;
    public int paid3;
    public int paid4;
    public int paid5;
    
}
public class PayCountInfo {
    
    public int iconCount;
    public int smallCount;
    public int freeCount;
    
}
public class ReadThePayTable : MonoBehaviour {
    public PaytableInfoList payTableList = null;
    
    public Object xml;
    public UILabel[] helpLabels = new UILabel[34];
    bool isRecieve;
    void Start()
    {
        isRecieve = false;
        SwitchLanguage();
        StartCoroutine(WaitDataBack());
        GameObject.Find("/GameManager").GetComponent<NetworkConnect>().PayTableRequest();
 //       ReadTheXml();
    }

    IEnumerator WaitDataBack()
    {
        while(!isRecieve)
        {
            yield return null;
        }
        WritePaytable();
    }
    public void JsonParse(string content)
    {
        Debug.Log(content);
        payTableList = JsonMapper.ToObject<PaytableInfoList>(content);
        isRecieve = true;
    }
    public void WritePaytable()
    {
        if(payTableList ==null) return;
        foreach(PaytableInfo info in payTableList.slotpaids)
        {
//            Debug.Log("IconID:"+info.iconId+";p1:"+info.paid1+";p2:"+info.paid2+";p3:"+info.paid3+";p4:"+info.paid4+";p5:"+info.paid5);
            switch(info.iconId)
            {
                case 8:
                    helpLabels[0].text = info.paid5.ToString();break;
                case 16:
                    helpLabels[1].text = info.paid3.ToString();
                    helpLabels[2].text = info.paid4.ToString();
                    helpLabels[3].text = info.paid5.ToString();
                    break;
                case 15:
                    helpLabels[4].text = info.paid3.ToString();
                    helpLabels[5].text = info.paid4.ToString();
                    helpLabels[6].text = info.paid5.ToString();
                    break;
                case 14:
                    helpLabels[7].text = info.paid3.ToString();
                    helpLabels[8].text = info.paid4.ToString();
                    helpLabels[9].text = info.paid5.ToString();
                    break;
                case 13:
                    helpLabels[10].text = info.paid3.ToString();
                    helpLabels[11].text = info.paid4.ToString();
                    helpLabels[12].text = info.paid5.ToString(); 
                    break;
                case 12:
                    helpLabels[13].text = info.paid3.ToString();
                    helpLabels[14].text = info.paid4.ToString();
                    helpLabels[15].text = info.paid5.ToString();
                    break;
                case 17:
                    helpLabels[16].text = info.paid3.ToString();
                    helpLabels[17].text = info.paid4.ToString();
                    helpLabels[18].text = info.paid5.ToString();
                    break;
                case 18:
                    helpLabels[19].text = info.paid3.ToString();
                    helpLabels[20].text = info.paid4.ToString();
                    helpLabels[21].text = info.paid5.ToString();
                    break;
                case 19:
                    helpLabels[22].text = info.paid3.ToString();
                    helpLabels[23].text = info.paid4.ToString();
                    helpLabels[24].text = info.paid5.ToString();
                    break;
                case 11:
                    helpLabels[25].text = info.paid3.ToString();
                    helpLabels[26].text = info.paid4.ToString();
                    helpLabels[27].text = info.paid5.ToString();
                    break;
            }
        }
        foreach(PayCountInfo info in payTableList.slotcounts)
        {
            switch(info.iconCount)
            {
                case 3:
                    helpLabels[28].text = info.freeCount.ToString();
                    helpLabels[31].text = info.smallCount.ToString();
                    //设置不可用，恢复时可删除下列两行代码
                    helpLabels[31].text = "";
                    helpLabels[31].transform.parent.GetComponent<UILabel>().text = "";
                    break;
                case 4:
                    helpLabels[29].text = info.freeCount.ToString();
                    helpLabels[32].text = info.smallCount.ToString();
                    //设置不可用，恢复时可删除下列两行代码
                    helpLabels[32].text = "";
                    helpLabels[32].transform.parent.GetComponent<UILabel>().text = "";
                    break;
                case 5:
                    helpLabels[30].text = info.freeCount.ToString();
                    helpLabels[33].text = info.smallCount.ToString();
                    //设置不可用，恢复时可删除下列两行代码
                    helpLabels[33].text = "";
                    helpLabels[33].transform.parent.GetComponent<UILabel>().text = "";
                    break;
                    
            }
        }
    }


    //废弃 20150906
    // 利用xml解析paytable方法
    void ReadTheXml()
    {
        int i = 0;
        SecurityParser sp = new SecurityParser();
        string xmlPath = "GamePayTable.xml";
        sp.LoadXml(xml.ToString());
        SecurityElement se = sp.ToXml();
        foreach (SecurityElement child in se.Children)
        {
            string id = child.Attribute("id");
            if(id == manager.GAME_ID.ToString())
            {
                foreach (SecurityElement ce in child.Children)
                {
//                    Debug.Log(ce.Attribute("id")+"______"+ce.Attribute("name"));
                    foreach(SecurityElement ch in ce.Children)
                    {
//                        Debug.Log(ch.Attribute("multiplier")+"-----------"+ch.Text);
                        helpLabels[i].text = ch.Text;
                        i++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Switchs the language.
    /// </summary>
    [ContextMenu("Switch Language")]
    void SwitchLanguage()
    {
        if(Localization.language == "简体中文")
        {
            UIFont targetFont = Resources.Load("_Font/CasinoFont",typeof(UIFont)) as UIFont;
            UILocalize[] uiLocalizes = transform.GetComponentsInChildren<UILocalize>();
            UILabel[] allLabelNeedToSwitchLangage = new UILabel[uiLocalizes.Length];
            for(int i = 0;i<allLabelNeedToSwitchLangage.Length;i++)
            {
                allLabelNeedToSwitchLangage[i] = uiLocalizes[i].GetComponent<UILabel>();
            }
            for(int i = 0;i<allLabelNeedToSwitchLangage.Length;i++)
            {
                allLabelNeedToSwitchLangage[i].bitmapFont = targetFont;
                Debug.Log(helpLabels[i].bitmapFont.ToString());
            }
        }
    }
    void OnEnable()
    {
        NetworkConnect.paytable_callback += JsonParse;
    }
    void OnDisable()
    {
        NetworkConnect.paytable_callback -= JsonParse;        
    }

}

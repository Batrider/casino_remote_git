using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;
using System.IO;
/*
 * 
 * 遊戲進入時的初始化操作&&判斷
 * */
public class GameStartSetting : MonoBehaviour {
    public Object gameManager;
    GameObject GameManager;
    Transform bnb;
    void Awake()
    {
        ReadTheXml();
        Camera.main.audio.volume = PlayerData.Music;
        NGUITools.soundVolume = PlayerData.Volume;
        GameManager = GameObject.FindWithTag("GameController");
        if(GameManager == null||SceneManager.loseConnection)
        {
            DestroyImmediate(GameManager);
            GameManager =  Instantiate(gameManager) as GameObject;
            GameManager.name = "GameManager";
        }
        bnb = GameObject.Find ("/GameManager/Camera/BrokenNetBox").transform;
        
        //斷線，重連
        if(SceneManager.loseConnection)
        {
            bnb.localScale = Vector3.one;
            GameManager.GetComponent<NewLogin>().Start();
            SceneManager.loseConnection = false;

            StartCoroutine(BrokenNet());
        }
    }
    IEnumerator BrokenNet()
    {
        yield return new WaitForSeconds(1.5f);
        bnb.localScale = Vector3.zero; 
    }
    /// <summary>
    /// 讀寫場景信息
    /// </summary>
    void ReadTheXml()
    {
        string xmlpath = "XMLData/GameInformation";
        Object xml = Resources.Load(xmlpath);
        SecurityParser sp = new SecurityParser();
        sp.LoadXml(xml.ToString());
        SecurityElement se = sp.ToXml();
        foreach (SecurityElement child in se.Children)
        {
//            Debug.Log(child.Attribute("id") + "______" + child.Attribute("name"));
            GameInformation gInfo = new GameInformation(int.Parse(child.Attribute("id")), int.Parse(child.Attribute("line")), child.Attribute("name"));
            if (!SceneManager.gInfos.ContainsKey(int.Parse(child.Attribute("id"))))
            {
                SceneManager.gInfos.Add(int.Parse(child.Attribute("id")), gInfo);
            }
        }
    }
    void OnDisable()
    {
        if(bnb!=null)
            bnb.localScale = Vector3.zero;
    }
}

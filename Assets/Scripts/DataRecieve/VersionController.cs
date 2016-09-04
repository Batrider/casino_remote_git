using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using LitJson;

public enum Operators
{
    Asia888,
    Dafa888,
    YinHe,
    Slot666,
    Igt777,
    Jubaopen,
    TBao18,
    Caishen,
    CEO888,
    Test,
    Demo,
    OGigt,
    SLOT777,
    OGSLOT

}


public class VersionController : MonoBehaviour {

    public static VersionController Instance;
    //版本号修改：
    //132
    public static int curVersion;

    //——————————————————————————
    const string hostName = "";
    public static string ftpUrl = "";
    public static string signUrl = "";
    public static string postUrl = "";
    public static string keyStr = "";
    public static string apkName = "";
    public static string versionUrl = "";
    public static string platform = "";
    public static string TCPserverName = "songtang.linkpc.net";
    public static string TCPserverIP = "";//"58.96.187.33";//"45.119.96.7";//"192.168.1.195";//"45.119.96.7";//"203.88.171.232";//192.168.1.199";//Demo网站"58.96.187.33";//"192.168.1.210";//"192.168.1.209";//旧服务器"198.11.178.11";
    public static int TCPserverPort ;//16001;//15001;// 14001;//15001;//旧服务器13510;
    public static string tokenUrl = "";
    public static string gameUrl = "";
    public static string systemTag = "mobile";
    public static string agent = "";
    [HideInInspector]
    public Operators operatorAgent = Operators.Test;

    public Transform TipBoxRoot;
    public Object tipPrefabs;
    public UISlider uiSlider;
    public float progress = 0;
    public GameObject downloadComponent;
    public GameObject TipBox;
    public TextAsset versionTxt;
    public string debugInfo = string.Empty;
	// Use this for initialization
    void Awake()
    {
        Instance = this;
        TextAsset archiveSetting = Resources.Load("ArchiveSetting/config") as TextAsset;
        string content = archiveSetting.text;
        ArchiveDataForm msgJsonRecieve = JsonMapper.ToObject<ArchiveDataForm>(content);
        curVersion = int.Parse(msgJsonRecieve.bundleVersion.Remove(1, 1).Remove(2, 1));
        //Debug.Log(msgJsonRecieve.bundleVersion.Remove(1,1).Remove(2,1));
        platform = msgJsonRecieve.platform;
        ftpUrl = msgJsonRecieve.ftpUrl;
        tokenUrl = msgJsonRecieve.tokenUrl;
        gameUrl = msgJsonRecieve.gameUrl;
        versionUrl = msgJsonRecieve.versionUrl;
        signUrl = msgJsonRecieve.signUrl;
        keyStr = msgJsonRecieve.keyStr;
        apkName = msgJsonRecieve.apkName;
        agent = msgJsonRecieve.agent;
    }
	void Start ()
    {
        InitGameInfo();
        //network check
        //StartCoroutine(NetWorkDetect());
        //no network check
        StartCoroutine(Load());
	}
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(1);
    }
    //url,language
    void InitGameInfo()
    {
        if (PlayerPrefs.GetInt("launchTimes", 0) == 0)
        {
            string language = "English";
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English: language = "English"; break;
                case SystemLanguage.Chinese: language = "简体中文"; break;
                default: language = "English"; break;
            }
            Localization.language = language;
            //初始化游戏版本信息
            GameVersionInit();
        }
        PlayerPrefs.SetInt("launchTimes", PlayerPrefs.GetInt("launchTimes", 0) + 1);
        if (File.Exists(Application.persistentDataPath + "/" + apkName))
        {
            File.Delete(Application.persistentDataPath + "/" + apkName);
        }
    }

	IEnumerator NetWorkDetect()
    {
        float timer = 0;
        while(Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (timer <= 0||timer>=5)
            {
                timer = 0;
                InstaniateTipBox("NetWorkTip");
            }
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(CheckTheVersionInfo());
    }
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	
	}
    /// <summary>
    /// 开始校验版本--
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckTheVersionInfo()
    {
        WWW versionTxt = new WWW(versionUrl);
        while (!versionTxt.isDone)
        {
            debugInfo = "Downloading";
            yield return null;
        }
        yield return versionTxt;
        if (versionTxt.error != null)
        {
            Debug.Log(versionTxt.error);
            Start();
        }
        else
        {
            string versionInfo = versionTxt.text;

            string[] versionInfos = versionInfo.Split('|');
            //检测版本更新
            Debug.Log("网上版本：" + int.Parse(versionInfos[0].Split(':')[1]));
            Debug.Log("当前版本：" + curVersion);
            if (int.Parse(versionInfos[0].Split(':')[1]) > curVersion)
            {
                TipBox.SetActive(true);
                Debug.Log(Application.internetReachability);
                if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    GameObject tipWarning = TipBox.transform.FindChild("warning").gameObject;
                    tipWarning.SetActive(false);
                }
            }
            else
            {
                //检测资源更新
                string[] sceneInfo = versionInfo.Split('|');
                for (int i = 1; i <= 25; i++)
                {
                    DetectResource(sceneInfo[i]);
                }
                yield return new WaitForSeconds(1.5f);
                Application.LoadLevel(1);
            }
        }
    }
    public void DetectResource(string info)
    {

        string[] sceneInfo = info.Split(':');
        string name = sceneInfo[0];
        int version =int.Parse(sceneInfo[1]);

        if (version > PlayerPrefs.GetInt(name,100))
        {
            PlayerPrefs.SetInt(name, version);
            //delete resource
            if (File.Exists(Application.persistentDataPath + "/" + name + ".unity3d"))
            {
                PlayerPrefs.SetInt(name + "Tag", 1);
                File.Delete(Application.persistentDataPath + "/" + name + ".unity3d");
            }
        }
    }
    public void StartDownload()
    {
        downloadComponent.SetActive(true);
        TweenAlpha.Begin(TipBox, 0.2f,0);
        StartCoroutine(DownLoadIE());
    }
    /// <summary>
    /// 开始下载
    /// </summary>
    /// <returns></returns>
    IEnumerator DownLoadIE()
    {
        WWW download = new WWW(ftpUrl+ apkName);
        while (!download.isDone)
        {
            uiSlider.value = download.progress;
            yield return null;
            Debug.Log(download.progress);
        }
        yield return download;
        // Handle error
        if (download.error != null)
        {
            Debug.LogError(download.error);
            yield break;
        }
        else
        {
            File.WriteAllBytes(Application.persistentDataPath + "/" + apkName, download.bytes);
            yield return new WaitForSeconds(0.5f);
            Application.OpenURL(Application.persistentDataPath + "/" + apkName);
        }
        
    }
    void InstaniateTipBox(string key = null)
    {
        GameObject tipBox = Instantiate(tipPrefabs) as GameObject;
        tipBox.transform.parent = TipBoxRoot;
        tipBox.transform.localScale = Vector3.one;
        tipBox.transform.localEulerAngles = Vector3.zero;
        tipBox.transform.localPosition = new Vector3(0,43,0);

        GameObject tipLabel = tipBox.transform.FindChild("TipLabel").gameObject;
        tipLabel.GetComponent<UILocalize>().key = key;

    }
    //调用一次
    void GameVersionInit()
    {
        string[] sceneInfo = versionTxt.text.Split('|');
        //curVersion =int.Parse(sceneInfo[0].Split(':')[1]);
        for(int i = 1;i<=25;i++)
        {
            PlayerPrefs.SetInt(sceneInfo[i].Split(':')[0], int.Parse(sceneInfo[i].Split(':')[1]));
        }
        for(int i = 1;i<=25;i++)
        {
            Debug.Log(sceneInfo[i].Split(':')[0]+PlayerPrefs.GetInt(sceneInfo[i].Split(':')[0]));
        }
    }
}

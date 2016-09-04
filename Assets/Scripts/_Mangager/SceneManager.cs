using UnityEngine;
using System.Collections;
using System.Security;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

public class SceneManager : MonoBehaviour
{
    //StreamingAssets
    public static readonly string PathURL =
        #if UNITY_EDITOR
//        Application.dataPath + "/StreamingAssets/";
        "file://" + Application.dataPath + "/StreamingAssets/";
    #elif UNITY_ANDROID 
    "jar:file://" + Application.dataPath + "!/assets/";
    #elif UNITY_IPHONE
    Application.dataPath + "/Raw/";
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    "file://" + Application.dataPath + "/StreamingAssets/";
    #else
    string.Empty;
    #endif
    public bool OpenSimlateBtn;
    public GameObject LogOutObj;
    public GameObject Setting;
    public static Dictionary<int,SceneDataDetect.DataState> sceneDataState = new Dictionary<int, SceneDataDetect.DataState>();
    public static string preSceneStr = "Lobby";
    public static string curSceneStr = "Lobby";
    public string[] allSceneName;
    public bool sceneEnable = false;
    public static int lineCount;
    public static bool loseConnection = false;
    [HideInInspector]
    public float downloadProgresss = 0;
    public static Dictionary<string,Vector3> cardPos = new Dictionary<string, Vector3>();
    public static Dictionary<int,GameInformation> gInfos = new Dictionary<int, GameInformation>();
    // Use this for initialization

    public static SceneManager Instance;
    /*
    [MenuItem("Custom Editor/GetEnableSceneName")]
    static void GetEnableSceneName()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        int i = 0;
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            Regex regex = new Regex(@"([a-z]|[A-Z])*\.unity");


            scene.enabled = true;
            string result = regex.Match(scene.path).ToString().Split('.')[0];

            Debug.Log(result);
            i++;
        }
        EditorBuildSettings.scenes = scenes;

    }
    */
    void Awake()
    {
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// </summary>
    /// <returns><c>true</c> if this instance is in available scene,return true; otherwise, return false</returns>
    /// <param name="curSceneName">Current scene name.</param>
    public bool IsInAvailableScene(string curSceneName)
    {
        if (curSceneName == "ChinaStyle") return true;
        if (!sceneEnable) return false;
        for(int i = 0;i< allSceneName.Length;i++)
        {
            if(curSceneName == allSceneName[i])
                return true;
        }
        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    public void ChangeTheScene(int index)
    {
        manager.GAME_ID = index;
        GameInformation ginfo;
        gInfos.TryGetValue(manager.GAME_ID, out ginfo);
        preSceneStr = curSceneStr;
        curSceneStr = ginfo.NAME;
        lineCount = ginfo.LINES;
        GetComponent<NetworkConnect>().ChooseGameType(manager.GAME_ID);
        Application.LoadLevel("Loading");
        if (curSceneStr != "Lobby")
            StartCoroutine(SettingBtnState(false));
        else
            StartCoroutine(SettingBtnState(true));
    }

    /// <summary>
    /// </summary>
    public void BackToMenu()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkConnect>().GAME_Balance_Refresh();

        Time.timeScale = 1.0f;
        manager.startGame = false;
        manager.FreeStart = false;
        manager.smallgame = false;
        manager.jackpot = false;
        manager.isDraw = false;
        preSceneStr = curSceneStr;
        curSceneStr = "Lobby";
        GetComponent<NetworkConnect>().ExitRoom();
        Application.LoadLevel("Lobby");
        Caching.CleanCache();
        StartCoroutine(SettingBtnState(true));
    }
    /// <summary>
    /// </summary>
    /// <returns>The button state.</returns>
    /// <param name="value">If set to <c>true</c> value.</param>
    IEnumerator SettingBtnState(bool value)
    {
        if (!value)
        {
            while (true)
            {
                yield return null;
                if (Application.loadedLevelName != "Lobby")
                {
                    LogOutObj.SetActive(false);
                    Setting.SetActive(false);
                    break;
                }
            }
        } else
        {
            while (true)
            {
                
                yield return null;
                if (Application.loadedLevelName == "Lobby")
                {
                    LogOutObj.SetActive(true);
                    Setting.SetActive(true);
                    break;
                }
            }
        }
    }
    public void DownLoadData(int index)
    {
        StartCoroutine(DownLoadDataFromNet(Conversion.SceneNameIDConver(index)));
    }
    IEnumerator DownLoadDataFromNet(string sceneName)
    {
        string pattern = @"\/\w+\/";
        Regex regex = new Regex(pattern);
        string sceneUrl = regex.Replace(VersionController.ftpUrl, "/resource/");
        Debug.LogWarning(sceneUrl + sceneName + ".unity3d");
        WWW assetbndle = new WWW(sceneUrl + sceneName + ".unity3d");
        int indexTime = 0;
        downloadProgresss = 0;
        bool networkError = false;
        while (!assetbndle.isDone)
        {
            downloadProgresss = assetbndle.progress;
            yield return new WaitForSeconds(0.2f);
            if (downloadProgresss == assetbndle.progress)
            {
                indexTime++;
            }
            else
            {
                indexTime = 0;
            }
            if (indexTime > 50)
            {
                networkError = true;
                break;
            }
        }
        downloadProgresss = 0;
        Debug.Log("complete!");
        if (assetbndle.error != null|| networkError)
        {
            Debug.Log("failed");
            SceneManager.sceneDataState[Conversion.SceneNameIDConver(sceneName)] = SceneDataDetect.DataState.unDownload;
            if (curSceneStr == "Lobby")
            {
                SceneDataDetect sd = GameObject.Find("/FlowRoot/Views/" + Conversion.SceneNameIDConver(sceneName)).GetComponent<SceneDataDetect>();
                sd.DowmloadFailed();
            }
        }
        else
        {
            Debug.Log("success");
            Debug.Log(Conversion.SceneNameIDConver(sceneName));
            SceneManager.sceneDataState[Conversion.SceneNameIDConver(sceneName)] = SceneDataDetect.DataState.Downloaded;
            if (curSceneStr == "Lobby")
            {
                SceneDataDetect sd = GameObject.Find("/FlowRoot/Views/" + Conversion.SceneNameIDConver(sceneName)).GetComponent<SceneDataDetect>();
                sd.DownLoadComplete();
            }
            Debug.Log("Start write");
            File.WriteAllBytes(Application.persistentDataPath + "/" + sceneName + ".unity3d", assetbndle.bytes);
        }
    }


}

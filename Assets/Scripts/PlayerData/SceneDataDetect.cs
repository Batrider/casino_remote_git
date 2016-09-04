using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

public class SceneDataDetect : MonoBehaviour
{
    public enum DataState
    {
        unDownload,
        Downloading,
        Downloaded
    }
    public DataState dataState = DataState.unDownload;
    private SceneManager sManager;
    [HideInInspector]
    public ProgressShowCC psc;

    string sceneName;
    TextMesh tt;
    void Start()
    {
        SceneManager.sceneDataState.TryGetValue(int.Parse(gameObject.name), out dataState);
        sManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneManager>();
        psc = GetComponentInChildren<ProgressShowCC>();
        sceneName = Conversion.SceneNameIDConver(int.Parse(gameObject.name));
        StartCoroutine(GetStreamData());
        tt = GetComponentInChildren<TextMesh>();
    }
    void Update()
    {
        if (dataState == DataState.Downloading && sManager.downloadProgresss > 0)
        {
            psc.ShowProgress(sManager.downloadProgresss);
        }
    }
    public IEnumerator DetectData()
    {
        yield return null;

        FileInfo info = new FileInfo(Application.persistentDataPath + "/" + sceneName + ".unity3d");
        //        WWW www = new WWW(SceneManager.PathURL+sceneName + ".unity3d");
        if (info.Exists || sManager.IsInAvailableScene(sceneName))
        {
            psc.UnDownLoadObj(false);
            psc.DownLoadObj(false);
            if (!SceneManager.sceneDataState.ContainsKey(int.Parse(gameObject.name)))
            {
                dataState = DataState.Downloaded;
                SceneManager.sceneDataState.Add(int.Parse(gameObject.name), dataState);
            }
            else
            {
                dataState = DataState.Downloaded;
                SceneManager.sceneDataState[int.Parse(gameObject.name)] = dataState;
            }

        }
        else
        {
            if (dataState == DataState.Downloading)
            {
                psc.DownLoadObj(true);
                psc.UnDownLoadObj(false);
            }
            else
            {
                psc.UnDownLoadObj(true);
            }
            if (!SceneManager.sceneDataState.ContainsKey(int.Parse(gameObject.name)))
            {
                dataState = DataState.unDownload;
                SceneManager.sceneDataState.Add(int.Parse(gameObject.name), dataState);
            }

        }
    }
    IEnumerator GetStreamData()
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + "/" + sceneName + ".unity3d");
        //版本号初始默认值为100，只有版本号==100时才解压本地文件
//        Debug.Log("sceneName:"+ sceneName+";version:"+ PlayerPrefs.GetInt(sceneName));
        if (!info.Exists)//&&PlayerPrefs.GetInt(sceneName)==100)
        {
            WWW loadData = new WWW(SceneManager.PathURL + sceneName + ".unity3d");
            yield return loadData;
            if (loadData.error != null)
            {
                //                Debug.Log(loadData.error);

            }
            else if (loadData.bytes.Length != 0)
            {
                File.WriteAllBytes(Application.persistentDataPath + "/" + sceneName + ".unity3d", loadData.bytes);

            }
        }
        info = new FileInfo(Application.persistentDataPath + "/" + sceneName + ".unity3d");
        //        Debug.Log(info.Exists);

        StartCoroutine(DetectData());

    }
    public void DownLoadComplete()
    {
        Debug.Log(gameObject.name + dataState);

        psc.UnDownLoadObj(false);
        psc.DownLoadObj(false);
        dataState = DataState.Downloaded;
    }
    public void DowmloadFailed()
    {
        Debug.Log("DowmloadFailed");
        psc.UnDownLoadObj(true);
        psc.DownLoadObj(false);
        dataState = DataState.unDownload;
    }

}

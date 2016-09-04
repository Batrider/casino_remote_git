using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;


public class LoadingScene : MonoBehaviour
{
    public int version = 0;
    
    public UILabel numPercentage;
    public UISprite progressBar;
    AsyncOperation async_hao;
    float timeWait;
    void Start()
    {
        timeWait = 0;
        StartCoroutine(LoadProgressShow());
    }
    IEnumerator LoadProgressShow()
    {
//        WWW download =new WWW(Application.persistentDataPath+"/" + SceneManager.PreSceneStr + ".unity3d");
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneManager>().IsInAvailableScene(SceneManager.curSceneStr))
        {
            byte[] bs = File.ReadAllBytes(Application.persistentDataPath + "/" + SceneManager.curSceneStr + ".unity3d");
            AssetBundleCreateRequest abq = AssetBundle.CreateFromMemory(bs);
            while (!abq.isDone)
            {
                yield return null;
                Debug.Log(abq.progress);
            }
            yield return abq;
            var bundle = abq.assetBundle;
            yield return StartCoroutine(Load());
            bundle.Unload(false);
        } else
        {
            yield return StartCoroutine(Load());
        }
    }
    IEnumerator Load()
    {
        async_hao = Application.LoadLevelAsync(SceneManager.curSceneStr);
        async_hao.allowSceneActivation = false;
        while (async_hao.progress<0.9f)
        {
            timeWait += Time.deltaTime;
            numPercentage.text = (async_hao.progress / 0.9f).ToString("p");
            progressBar.fillAmount = (async_hao.progress / 0.9f);
            //            Debug.Log("progress:" + async_hao.progress);
            
            yield return null;
        }
        numPercentage.text = "100%";
        progressBar.fillAmount = 1f;
        yield return new WaitForSeconds(timeWait > 2f ? 0 : (2f - timeWait));
        async_hao.allowSceneActivation = true;
    }
}

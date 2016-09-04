using UnityEngine;
using System.Collections;
using System.IO;

public class DecompressionScence : MonoBehaviour {

    public string[] scenceNames;
    void Start()
    {
        StartCoroutine(GetStreamData());
    }
    //将在文件拷贝出来
    IEnumerator GetStreamData()
    {
        for (int i = 0; i<scenceNames.Length; i++)
        {
            FileInfo info = new FileInfo(Application.persistentDataPath + "/" + scenceNames[i] + ".unity3d");
            if (!info.Exists)
            {
                WWW loadData = new WWW(SceneManager.PathURL + scenceNames[i] + ".unity3d");
                yield return loadData;
                if (loadData.error != null)
                {
                    Debug.Log(loadData.error);
                    
                } else if (loadData.bytes.Length != 0)
                {
                    File.WriteAllBytes(Application.persistentDataPath + "/" + scenceNames[i] + ".unity3d", loadData.bytes);
                    
                }
            }
            info = new FileInfo(Application.persistentDataPath + "/" + scenceNames[i] + ".unity3d");
            Debug.Log(info.Exists);
            
            //         StartCoroutine(DetectData());
        }
    }
}

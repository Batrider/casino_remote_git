using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
public class ArchiveSetting : MonoBehaviour {

    [MenuItem("Custom Editor/Archive")]
    static void Setting()
    {
        TextAsset archiveSetting = Resources.Load("ArchiveSetting/config") as TextAsset;
        string content = archiveSetting.text;
        ArchiveDataForm msgJsonRecieve = JsonMapper.ToObject<ArchiveDataForm>(content);

        Debug.Log(content);
        PlayerSettings.companyName = msgJsonRecieve.companyName;
        PlayerSettings.productName = msgJsonRecieve.productName;
        PlayerSettings.bundleIdentifier = msgJsonRecieve.bundleIdentifier;
        PlayerSettings.bundleVersion = msgJsonRecieve.bundleVersion;
        PlayerSettings.shortBundleVersion = msgJsonRecieve.shortBundleVersion;

    }

}

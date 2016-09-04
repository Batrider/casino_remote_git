using UnityEngine;
using System.Collections;

public class ProgressShowCC : MonoBehaviour {
    public GameObject progressObj;
    public GameObject downLoadObj;
    public GameObject unDownloadObj;
    public GameObject updateObj;
    public TextMesh tm;
    public Material downLoading_EN;
    public Material downLoading_CN;
    public Material undownLoad_EN;
    public Material undownLoad_CN;
    public Material newVersion_EN;
    public Material newVersion_CN;
     
    void Start()
    {
        if(Localization.language == "English")
        {
            downLoadObj.transform.FindChild("Down").GetComponent<MeshRenderer>().material = downLoading_EN;
            unDownloadObj.GetComponent<MeshRenderer>().material = undownLoad_EN;
            updateObj.GetComponent<MeshRenderer>().material = newVersion_EN;

        }
        else
        {
            downLoadObj.transform.FindChild("Down").GetComponent<MeshRenderer>().material = downLoading_CN;
            unDownloadObj.GetComponent<MeshRenderer>().material = undownLoad_CN;
            updateObj.GetComponent<MeshRenderer>().material = newVersion_CN;
        }





        tm.GetComponent<MeshRenderer>().sortingLayerName = "progress";
        tm.GetComponent<MeshRenderer>().sortingOrder = 0;
    }

    public void ShowProgress(float progress)
    {
        progressObj.transform.localPosition = new Vector3((progress-1.26f)/0.78f,progressObj.transform.localPosition.y,progressObj.transform.localPosition.z);
        progressObj.transform.localScale = new Vector3(2.7f*(progress),0.34f,0.1f);
        tm.text = progress.ToString("p");
    }
    public void DownLoadObj(bool state)
    {
        downLoadObj.SetActive(state);
    }
    public void UnDownLoadObj(bool state)
    {
        updateObj.SetActive(false);
        unDownloadObj.SetActive(false);
        //如果存在新的场景版本
        if (PlayerPrefs.GetInt(Conversion.SceneNameIDConver(int.Parse(transform.parent.name))+"Tag") > 0)
        {
            updateObj.SetActive(state);
        }
        else
        {
            unDownloadObj.SetActive(state);
        }
    }
    void OnLocalize()
    {
        if (Localization.language == "English")
        {
            downLoadObj.transform.FindChild("Down").GetComponent<MeshRenderer>().material = downLoading_EN;
            unDownloadObj.GetComponent<MeshRenderer>().material = undownLoad_EN;
            updateObj.GetComponent<MeshRenderer>().material = newVersion_EN;

        }
        else
        {
            downLoadObj.transform.FindChild("Down").GetComponent<MeshRenderer>().material = downLoading_CN;
            unDownloadObj.GetComponent<MeshRenderer>().material = undownLoad_CN;
            updateObj.GetComponent<MeshRenderer>().material = newVersion_CN;
        }
    }
    void OnEnable()
    {
        Localization.changeLanguage += OnLocalize;
    }
    void OnDisable()
    {
        Localization.changeLanguage -= OnLocalize;
    }
}

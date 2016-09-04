using UnityEngine;
using System.Collections;

public class MenuLineSelect : MonoBehaviour
{
    public UILabel label;
    void Start()
    {
        GetComponent<UIPopupList>().value = PlayerPrefs.GetString("LineTag","Defalut");
    }
    public void SelectLine()
    {
        string text;
        if (UIPopupList.current != null)
        {
            text = UIPopupList.current.isLocalized ?
                Localization.Get(UIPopupList.current.value) :
                    UIPopupList.current.value;
        }
        PlayerPrefs.SetString("LineTag", label.text);
    }
}

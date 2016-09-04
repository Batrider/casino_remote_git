using UnityEngine;
using System.Collections;

public class Wild2DEffect : MonoBehaviour {
    public Object wildCharPrefabs;
    public UIAtlas curAltas;
    public void ShowWildEffect()
    {
        GameObject wildChar = Instantiate(wildCharPrefabs) as GameObject;
        UISprite[] uss = wildChar.GetComponentsInChildren<UISprite>();
        foreach(UISprite us in uss)
        {
            us.atlas = curAltas;
        }
        wildChar.transform.parent = transform;
        wildChar.transform.localPosition = Vector3.zero;
        wildChar.transform.localScale = Vector3.one;
        wildChar.transform.localEulerAngles = Vector3.zero;


    }

}

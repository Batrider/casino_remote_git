using UnityEngine;
using System.Collections;

public class AutoSpinManager : MonoBehaviour {
    public GameObject[] autoSpins;
    public GameObject spinTimes;
    public virtual void AutoSpinMethod(bool value)
    {
        spinTimes.SetActive(!value);
        GetComponent<BoxCollider>().enabled = value;
        if (GetComponent<UI2DSprite>())
            GetComponent<UI2DSprite>().enabled = value;
        else
            GetComponent<UISprite>().enabled = value;
    }
    void OnEnable()
    {
        manager.autoSpinEndTag += AutoSpinMethod;        
    }
    void OnDisable()
    {
        manager.autoSpinEndTag -= AutoSpinMethod;              
    }
}

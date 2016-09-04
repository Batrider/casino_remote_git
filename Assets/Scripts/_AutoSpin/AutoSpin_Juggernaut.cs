using UnityEngine;
using System.Collections;

public class AutoSpin_Juggernaut : AutoSpinManager {
	// Use this for initialization
    public GameObject SpinButton;
    public void StartAutoSpin(GameObject objTime)
    {
        if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
        {
            SpinButton.GetComponent<BoxCollider>().enabled = false;
            //设置自动转动次数
            manager.autoSpinTimess =int.Parse(objTime.name);
            GameObject.Find("/UI Root").GetComponent<manager>().reset();
        }
    }
    public override void AutoSpinMethod(bool value)
    {
        spinTimes.SetActive(!value);
        for(int i = 0;i<autoSpins.Length;i++)
        {
            autoSpins[i].SetActive(value);
        }
        GetComponent<BoxCollider>().enabled = value;
        GetComponent<UI2DSprite>().enabled = value;
    }
}

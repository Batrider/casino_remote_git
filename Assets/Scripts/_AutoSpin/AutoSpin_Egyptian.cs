using UnityEngine;
using System.Collections;

public class AutoSpin_Egyptian : AutoSpinManager {
    public GameObject SpinButton;
    private bool IsOpen = false;
    
    public void StartAutoSpin(GameObject objTime)
    {
        if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
        {
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                autoSpins[i].GetComponent<BoxCollider>().enabled = false;
            }
            manager.autoSpinTimess =int.Parse(objTime.name);
            GameObject.Find("/UI Root").GetComponent<manager>().reset();
            ShowOrHideTheTimeObj();
        }
    }
    
    //中国风、埃及做法
    public void ShowOrHideTheTimeObj()
    {
        StartCoroutine(showorhide());
    }
    IEnumerator showorhide()
    {
        Vector3 bs = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().size = Vector3.zero;
        if(!IsOpen)
        {
            IsOpen = true;
            for(int i = 0;i<autoSpins.Length;i++)
            {
                autoSpins[i].SetActive(true);
                TweenScale.Begin(autoSpins[i],.5f,new Vector3(1f,1f,1f));
                autoSpins[i].GetComponent<TweenScale>().method = UITweener.Method.BounceIn;
                yield return new WaitForSeconds(0.2f);
            }
            
            for(int i = 0;i<autoSpins.Length;i++)
            {
                autoSpins[i].GetComponent<BoxCollider>().enabled = true;
            }
            
        }
        else
        {
            IsOpen = false;
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                TweenScale.Begin(autoSpins[i],.3f,new Vector3(0,0,0));
                autoSpins[i].GetComponent<TweenScale>().method = UITweener.Method.Linear;
                yield return new WaitForSeconds(0.1f);
            }
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                autoSpins[i].SetActive(true);
                autoSpins[i].GetComponent<BoxCollider>().enabled = false;
            }
            
        }
        GetComponent<BoxCollider>().size = bs;
    }
}

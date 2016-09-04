using UnityEngine;
using System.Collections;

public class AutoSpin_Sugar : AutoSpinManager {
    public GameObject SpinButton;
    
    private bool IsOpen = false;
    public void StartAutoSpin(GameObject objTime)
    {
        if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
        {
            manager.autoSpinTimess =int.Parse(objTime.name);
            GameObject.Find("/UI Root").GetComponent<manager>().reset();
            ShowOrHideTheTimeObj();
        }
    }
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
                TweenAlpha ta = autoSpins[i].GetComponent<TweenAlpha>();
                ta.enabled = true;
                ta.PlayForward();
                Debug.Log("PlayForward");
            }
        }
        else
        {
            IsOpen = false;
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                TweenAlpha ta = autoSpins[i].GetComponent<TweenAlpha>();
                ta.enabled = true;
                ta.PlayReverse();
                Debug.Log("PlayReverse");
            }
        }
        GetComponent<BoxCollider>().size = bs;
        yield return new WaitForSeconds(0.1f);
    }
}

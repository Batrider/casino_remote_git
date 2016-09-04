using UnityEngine;
using System.Collections;

public class AutoSpin_Bigshot : AutoSpinManager {
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
                autoSpins[i].GetComponent<SpringPosition>().enabled = true;
            }
        }
        else
        {
            IsOpen = false;
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                TweenAlpha.Begin(autoSpins[i],0.3f,0);
            }
            yield return new WaitForSeconds(0.5f);
            for(int i = autoSpins.Length-1;i>=0;i--)
            {
                autoSpins[i].GetComponent<SpringPosition>().enabled = false;
                autoSpins[i].transform.localPosition = Vector3.zero;
                autoSpins[i].GetComponent<UISprite>().alpha = 1f;
            }
        }
        GetComponent<BoxCollider>().size = bs;
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator hideSpinBtns()
    {
        IsOpen = false;
        for(int i = autoSpins.Length-1;i>=0;i--)
        {
            TweenAlpha.Begin(autoSpins[i],0.3f,0);
        }
        yield return new WaitForSeconds(0.5f);
        for(int i = autoSpins.Length-1;i>=0;i--)
        {
            autoSpins[i].GetComponent<SpringPosition>().enabled = false;
            autoSpins[i].transform.localPosition = Vector3.zero;
            autoSpins[i].GetComponent<UISprite>().alpha = 1f;
        }
         }
}

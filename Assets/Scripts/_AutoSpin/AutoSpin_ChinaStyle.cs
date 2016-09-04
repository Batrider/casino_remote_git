using UnityEngine;
using System.Collections;

public class AutoSpin_ChinaStyle : AutoSpinManager {
	public GameObject SpinButton;
	public GameObject timeObjBackground;
    public AudioClip boundOutClip;
	private bool IsOpen = false;

	public void StartAutoSpin(GameObject objTime)
	{
		if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
		{
            for (int i = autoSpins.Length-1; i>=0; i--)
            {
                autoSpins [i].GetComponent<BoxCollider>().enabled = false;
                autoSpins[i].transform.localScale = Vector3.zero;
			}
			timeObjBackground.transform.localScale = Vector3.zero;
			manager.autoSpinTimess =int.Parse(objTime.name);
			GameObject.Find("/UI Root").GetComponent<manager>().reset();
			IsOpen = false;
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
			TweenScale.Begin(timeObjBackground,.5f,new Vector3(1f,1f,1f));
			timeObjBackground.GetComponent<TweenScale>().method = UITweener.Method.BounceIn;
			IsOpen = true;
            for(int i = 0;i<autoSpins.Length;i++)
			{
                autoSpins[i].GetComponent<BoxCollider>().enabled = false;
                TweenScale.Begin(autoSpins[i],.5f,new Vector3(1f,1f,1f));
                autoSpins[i].GetComponent<TweenScale>().method = UITweener.Method.BounceIn;
                //播放弹出音效
                yield return new WaitForSeconds(0.1f);
                GetComponent<AudioSource>().PlayOneShot(boundOutClip);
				yield return new WaitForSeconds(0.1f);
			}
            //等待按钮完全弹出
            yield return new WaitForSeconds(0.2f);
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
                autoSpins[i].GetComponent<BoxCollider>().enabled = false;
			}
			timeObjBackground.transform.localScale = Vector3.zero;
			
		}
		GetComponent<BoxCollider>().size = bs;
	}
}

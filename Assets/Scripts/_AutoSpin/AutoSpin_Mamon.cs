using UnityEngine;
using System.Collections;

public class AutoSpin_Mamon : AutoSpinManager {
	public GameObject SpinButton;
	public GameObject timeObjBackground;
	private bool IsOpen = false;
	public void StartAutoSpin(GameObject objTime)
	{
		if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
		{
			TweenAlpha.Begin(timeObjBackground,.3f,0);
            IsOpen = false;
			
			manager.autoSpinTimess =int.Parse(objTime.name);
			GameObject.Find("/UI Root").GetComponent<manager>().reset();
		}
	}
	//财神
	public void ShowOrHide_Mammon()
	{
		if(!IsOpen)
		{
			TweenAlpha.Begin(timeObjBackground,.3f,1);
			IsOpen = true;
		}
		else
		{
			TweenAlpha.Begin(timeObjBackground,.3f,0);
			IsOpen = false;
		}
	}
}

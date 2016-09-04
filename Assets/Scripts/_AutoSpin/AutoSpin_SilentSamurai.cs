using UnityEngine;
using System.Collections;

public class AutoSpin_SilentSamurai :AutoSpinManager {
	public GameObject SpinButton;
	private bool isReceived = true;
	//记录是分散还是收拢
	private bool isBack = true;
	// Use this for initialization
	public void StartAutoSpin(GameObject objTime)
	{

		if(!GameObject.Find("/UI Root").GetComponent<manager>().IsNowMoving()&&!manager.isDraw)
		{
			//设置自动转动次数
			manager.autoSpinTimess =int.Parse(objTime.name);
			GameObject.Find("/UI Root").GetComponent<manager>().reset();
		}
	}
	public void ClickAutoSpin()
	{
		if(isReceived)
		{
			isReceived = false;
			if(isBack)
			{
			//set any autospins's obj animation
				for(int i = 0;i<autoSpins.Length;i++)
				{
					TweenPosition.Begin(autoSpins[i],0.2f*i,Vector3.zero);
				}
				isBack = false;
			}
			else
			{
				for(int i = 0;i<autoSpins.Length;i++)
				{
					if(i<3)
						TweenPosition.Begin(autoSpins[i],0.2f*i,new Vector3(-130-100*i,0,0));
					else
						TweenPosition.Begin(autoSpins[i],0.2f*i,new Vector3(-200-100*i,0,0));
				}
				isBack = true;
			}
			isReceived = true;
		}
	}
}

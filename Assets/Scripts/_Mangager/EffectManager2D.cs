using UnityEngine;
using System.Collections;

public class EffectManager2D : EffectManager {
    void Start()
    {

      
    }
	public  override void OpenEffect()
	{
		//动画控制语句
		string triggerName = gameObject.name;
		if(GetComponent<Animator>())
		{
			GetComponent<Animator>().SetTrigger(triggerName);
			if(GetComponentInChildren<UI2DSprite>())
			{
				UI2DSprite[] uss = GetComponentsInChildren<UI2DSprite>();
				foreach(UI2DSprite us in uss)
				{
					us.enabled = true;
					us.depth = 12;
				}
			}
		}
		StartCoroutine("CloseEffect");
	}
	IEnumerator CloseEffect()
	{
		//表演时间
		yield return new WaitForSeconds(2f);
		if(GetComponentInChildren<UI2DSprite>())
		{
			UI2DSprite[] uss = GetComponentsInChildren<UI2DSprite>();
			foreach(UI2DSprite us in uss)
			{
				us.enabled = true;
				us.depth = 8;
			}
		}
	}

}

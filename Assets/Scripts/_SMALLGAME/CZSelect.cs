using UnityEngine;
using System.Collections;

public class CZSelect : MonoBehaviour {

	public UILabel resTime;
	/// <summary>
	/// Selects the button message.
	/// </summary>
	public void SelectBtnMessage(GameObject obj)
	{
//		obj.GetComponentInParent<RotationSelf>().SetBtnUnable();
		if(obj.transform.parent.name =="chooseBtn1")
		{
			//左边
			GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_EGYPTIAN(5018);
		}
		else 
		{
			//右边
			GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_EGYPTIAN(5019);
		}
	}
}

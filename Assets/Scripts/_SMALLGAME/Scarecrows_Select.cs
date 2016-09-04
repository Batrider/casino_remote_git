using UnityEngine;
using System.Collections;

public class Scarecrows_Select : MonoBehaviour {
	public UILabel resTime;
	public void SelectBtnMessage(GameObject obj)
	{
		Debug.Log("SelectBtnMessage:"+obj.name);
		Debug.Log("canSelect:"+SmallGame.canSelect);
		if(SmallGame.canSelect)
		{
			SmallGame.canSelect = false;
			GetComponent<Sg_SlientSamurai>().btn_smalls.Add(int.Parse(obj.name),obj);
			obj.GetComponent<BoxCollider>().enabled = false;
			SmallGame.index_small = int.Parse(obj.name);
			
			GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_5TurnOver(SmallGame.index_small);
			
		}
	}
}
	
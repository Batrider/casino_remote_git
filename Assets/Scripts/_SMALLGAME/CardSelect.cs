using UnityEngine;
using System.Collections;

public class CardSelect : MonoBehaviour {
	public UILabel resTime;
	public SpiderShoot sshoot;
	public void SelectBtnMessage(GameObject obj)
	{
		Debug.Log("SelectBtnMessage:"+obj.name);
		Debug.Log("canSelect:"+SmallGame.canSelect);
		if(SmallGame.canSelect)
		{
			sshoot.Shoot(obj);
			SmallGame.canSelect = false;
			GetComponent<Sg_SpiderMan>().btn_smalls.Add(int.Parse(obj.name),obj);
			obj.GetComponent<BoxCollider>().enabled = false;
			SmallGame.index_small = int.Parse(obj.name);

			GameObject.Find("/GameManager").GetComponent<NetworkConnect>().GAME_SMALL_SpiderMan();

		}
	}
}

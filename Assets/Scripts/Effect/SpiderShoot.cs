using UnityEngine;
using System.Collections;

public class SpiderShoot : MonoBehaviour {
	public Animator am;
	public GameObject[] nets;
	private GameObject childUI;
	public void Shoot(GameObject btnObj)
	{
		am.SetTrigger("shoot");
		StartCoroutine(NetAnimation(int.Parse(btnObj.name)));
	}
	IEnumerator NetAnimation(int index)
	{
		childUI = nets[index-1].transform.FindChild("net").gameObject;
		yield return new WaitForSeconds(0.3f);
		TweenScale.Begin(nets[index-1],0.15f,Vector3.one);
		yield return new WaitForSeconds(0.3f);
		TweenAlpha.Begin(childUI,.2f,0);
		yield return new WaitForSeconds(.3f);
		childUI.GetComponent<UI2DSprite>().alpha = 1f;
		nets[index-1].transform.localScale = new Vector3(0.1f,0.1f,0.1f);
	}
}

using UnityEngine;
using System.Collections;

public class EgyptianOpen : MonoBehaviour {
	private bool isEnd;
	// Use this for initialization
	void Start () {
		isEnd = false;
		StartCoroutine(changeTheSprite());
	}


	IEnumerator changeTheSprite()
	{
		int index = 1;
		UISprite scrollUS = GetComponent<UISprite>();
		yield return new WaitForSeconds(0.52f);
        while(!isEnd)
		{
			scrollUS.spriteName = "scroll"+(index%4+1);
			yield return new WaitForSeconds(0.02f);
			
			index++;
			
		}

	}
	public void EndScroll()
	{
		isEnd = true;
		transform.parent.gameObject.SetActive(false);

	}
}

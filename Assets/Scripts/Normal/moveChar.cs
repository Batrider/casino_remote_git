using UnityEngine;
using System.Collections;

public class moveChar : MonoBehaviour {
	float posStart;
	private bool arrived;
	void Start()
	{
		posStart = transform.localPosition.y;
		arrived = false;
	}

	void FixedUpdate() {
		if(!arrived)
		{
			if(transform.name =="1")
			{
//				每个位置减少20个单位，以做弹簧效果
				float posy = Mathf.Lerp(posStart,-160f,Time.timeSinceLevelLoad);
				transform.localPosition = new Vector3(transform.localPosition.x, posy, 0);
				if(transform.localPosition.y == -160f)
				{
					arrived = true;
				}
			}
			if(transform.name =="2")
			{
//				每个位置减少20个单位，以做弹簧效果
				float posy = Mathf.Lerp(posStart,-85f,Time.timeSinceLevelLoad/1.5f);
				transform.localPosition = new Vector3(transform.localPosition.x, posy, 0);
				if(transform.localPosition.y == -85f)
				{
					arrived = true;
				}
			}
			if(transform.name =="3")
			{
//				每个位置减少20个单位，以做弹簧效果
				float posy = Mathf.Lerp(posStart,-2f,Time.timeSinceLevelLoad/2f);
				transform.localPosition = new Vector3(transform.localPosition.x, posy, 0);
				if(transform.localPosition.y == -2f)
				{
					arrived = true;
				}
			}
			if(transform.name =="4")
			{
//				每个位置减少20个单位，以做弹簧效果
				float posy = Mathf.Lerp(posStart,-240f,Time.timeSinceLevelLoad/2.5f);
				transform.localPosition = new Vector3(transform.localPosition.x, posy, 0);
				if(transform.localPosition.y == -240f)
				{
					arrived = true;
					//效果结束时加载新的场景
                    GameObject.Find("/GameManager").GetComponent<SceneManager>().StartCoroutine("LoadScene");
				}
			}
		}
		else
		{
			gameObject.GetComponent<SpringPosition>().enabled = true;
		}
	}

}

using UnityEngine;
using System.Collections;

public class Winds : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(windEffectStart());
		GetComponent<AudioSource>().Play();
	}
	IEnumerator  windEffectStart()
	{
		yield return new WaitForSeconds(1.5f);
		for(int i = 10002;i<10019;i++)
		{

			GetComponent<UISprite>().spriteName = i.ToString();
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(gameObject);
	}
}

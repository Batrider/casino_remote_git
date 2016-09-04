using UnityEngine;
using System.Collections;

public class NumsAdd : MonoBehaviour {
	public GameObject[] numObjs;
	private bool isChange = true;
	// Use this for initialization
	public void StartAdd(int money)
	{
		StartCoroutine(AddEffect(money));
	}

	IEnumerator AddEffect(int winNums)
	{
		isChange = true;
		StartCoroutine(NumsChange(numObjs[0]));
		if(winNums>=10)
		{
			this.transform.localPosition = new Vector3(26,-50,0);
			numObjs[1].SetActive(true);
			StartCoroutine(NumsChange(numObjs[1]));
		}
		if(winNums>=100)
		{
			this.transform.localPosition = new Vector3(52,-50,0);
			numObjs[2].SetActive(true);
			StartCoroutine(NumsChange(numObjs[2]));
				
		}
		if(winNums>=1000)
		{
			this.transform.localPosition = new Vector3(78,-50,0);
			numObjs[3].SetActive(true);
			StartCoroutine(NumsChange(numObjs[3]));
					
		}
		yield return new WaitForSeconds(1.5f);
		isChange = false;
		yield return null;
		yield return null;
		if(winNums<10)
		{
			numObjs[0].GetComponent<UISprite>().spriteName = winNums.ToString();
		}
		else if(winNums>=10&&winNums<100)
		{
			numObjs[0].GetComponent<UISprite>().spriteName =(winNums%10).ToString();
			numObjs[1].GetComponent<UISprite>().spriteName =(winNums/10).ToString();
		}
		else if(winNums>=100&&winNums<1000)
		{
			numObjs[0].GetComponent<UISprite>().spriteName =(winNums%10).ToString();
			numObjs[1].GetComponent<UISprite>().spriteName =((winNums/10)%10).ToString();
			numObjs[2].GetComponent<UISprite>().spriteName =(winNums/100).ToString();
		}
		else if(winNums>=1000&&winNums<10000)
		{
			numObjs[0].GetComponent<UISprite>().spriteName =(winNums%10).ToString();
			numObjs[1].GetComponent<UISprite>().spriteName =((winNums%100)/10).ToString();
			numObjs[2].GetComponent<UISprite>().spriteName =((winNums/100)%10).ToString();
			numObjs[3].GetComponent<UISprite>().spriteName =(winNums/1000).ToString();
		}
		yield return new WaitForSeconds(3f);
		numObjs[1].SetActive(false);
		numObjs[2].SetActive(false);
		numObjs[3].SetActive(false);
		


		
//		for (int i = 0; i <= winNums; i++) 
//		{
//			if(i<10)
//			{
//				numObjs[0].GetComponent<UISprite>().spriteName = i.ToString();
//				yield return null;
//			}
//			else if(i>=10&&i<100)
//			{
//				numObjs[0].GetComponent<UISprite>().spriteName =(i%10).ToString();
//				numObjs[1].GetComponent<UISprite>().spriteName =(i/10).ToString();
//				yield return null;
//			}
//			else if(i>=100&&i<1000)
//			{
//				numObjs[0].GetComponent<UISprite>().spriteName =(i%10).ToString();
//				numObjs[1].GetComponent<UISprite>().spriteName =((i/10)%10).ToString();
//				numObjs[2].GetComponent<UISprite>().spriteName =(i/100).ToString();
//				yield return null;
//			}
//			else if(i>=1000&&i<10000)
//			{
//				numObjs[0].GetComponent<UISprite>().spriteName =(i%10).ToString();
//				numObjs[1].GetComponent<UISprite>().spriteName =((i%100)/10).ToString();
//				numObjs[2].GetComponent<UISprite>().spriteName =((i/100)%10).ToString();
//				numObjs[3].GetComponent<UISprite>().spriteName =(i/1000).ToString();
//				yield return null;
//			}
//		}
	}
	IEnumerator NumsChange(GameObject numObj)
	{
		int i = 0;
		while(isChange)
		{
			yield return null;
			numObj.GetComponent<UISprite>().spriteName = i.ToString();
			i++;
			if(i>=10)
			{
				i=0;
			}
		}
	}
}

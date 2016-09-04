using UnityEngine;
using System.Collections;

public class LineShow2D : MonoBehaviour {
	//根据3d或者2D来画不同的线
	public GameObject alphaDark;
	public GameObject[] LinesOf25;
	public void LineShow(int index,bool flag)
	{
		LinesOf25[index].SetActive(flag);
	}


}

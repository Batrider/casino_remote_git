using UnityEngine;
using System.Collections;

public class GoldFlyPanel : MonoBehaviour {
    public Object goldPrefabs;
    public Vector3 Offset = Vector3.one;
	public void GoldPanelStart(Vector3 initPos)
	{
        StartCoroutine(GoldInstaniate(5,initPos));
	}
    IEnumerator GoldInstaniate(int nums,Vector3 initPos)
    {
        for(int i = 0;i<nums;i++)
        {
            GameObject gold = Instantiate(goldPrefabs) as GameObject;
            gold.transform.parent = transform;
            gold.transform.localPosition = initPos+Offset;
           // Debug.Log("Gold'localposition"+gold.transform.localPosition);
            gold.transform.localScale = Vector3.one;
            gold.transform.localEulerAngles = Vector3.zero;

            yield return new WaitForSeconds(0.1f);
        }



    }
}

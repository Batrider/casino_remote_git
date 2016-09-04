using UnityEngine;
using System.Collections;

public class CharacterAnim : MonoBehaviour {
	// Use this for initialization
    public string prefixName = string.Empty;
    public float cyclesTime = 0;
    public int totalCount = 0;
    float time = 0;
    void Update()
    {
        time = time + Time.deltaTime;
        if(time>cyclesTime)
        {
            time = 0;
            StartCoroutine(eyesAnim());
        }
    }


    IEnumerator eyesAnim()
    {
        for (int i = 1; i<totalCount; i++)
        {
            yield return new WaitForSeconds(0.05f);
            GetComponent<UISprite>().spriteName = prefixName+i;
        }
        for (int i = totalCount-1; i>0; i--)
        {
            yield return new WaitForSeconds(0.05f);
            GetComponent<UISprite>().spriteName = prefixName+i;
        }
    }
}

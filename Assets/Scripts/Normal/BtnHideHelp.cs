using UnityEngine;
using System.Collections;

public class BtnHideHelp : MonoBehaviour {
    public string playerPrefabsStr;
	public void DestroySelf()
    {
        StartCoroutine(WaitDestroy());
    }
    IEnumerator WaitDestroy()
    {
        PlayerPrefs.SetInt(playerPrefabsStr, 1);
        TweenAlpha.Begin(gameObject, .3f, 0);
        yield return new WaitForSeconds(0.5f);
 //       Destroy(gameObject);
    }

}

using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {
	public float DestroyTime=3;
	public bool isSetHide=false;
    public AudioClip destroyAudio;
	//保存中文
	// Use this for initialization
	void Start () 
	{
		StartCoroutine("DestroyGameObject");
	}

	IEnumerator DestroyGameObject()
	{
		yield return new WaitForSeconds(DestroyTime);
        if(destroyAudio!=null)
        {
            UIPlaySound us = gameObject.AddComponent<UIPlaySound>();
            us.audioClip = destroyAudio;
            us.Play();
        }
		if(isSetHide)
			gameObject.SetActive(false);
		else
			Destroy(gameObject);


	}
	
}

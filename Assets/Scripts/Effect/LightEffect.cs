using UnityEngine;
using System.Collections;

public class LightEffect : MonoBehaviour {
	private Animator[] ams;
	// Use this for initialization
	void Start()
	{
		ams = GetComponentsInChildren<Animator>();
	}
	public void MoveLight()
	{
		foreach(Animator am in ams)
		{
			am.SetTrigger("light");
		}
	}
}

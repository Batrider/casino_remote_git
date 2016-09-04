using UnityEngine;
using System.Collections;

public class ParticleSpeedCheck : MonoBehaviour {
	private ParticleSystem[] pct; 
	// Use this for initialization
	void Start () {
		pct = GetComponentsInChildren<ParticleSystem>();
        StartSim();
	}
	public void ParticleSpeedChange()
	{
		StartCoroutine(KeepSpeed());
	}
	IEnumerator KeepSpeed()
	{
		yield return 0;
		foreach(ParticleSystem ps in pct)
		{
			ps.playbackSpeed = 1f/Time.timeScale;
		}
	}
    void StartSim()
    {
        foreach(ParticleSystem ps in pct)
        {
            ps.Play();
        }
    }
}

using UnityEngine;
using System.Collections;

public class SwitchEffect : MonoBehaviour {
    public GameObject up;
    public GameObject down;
    public GameObject words;
    public GameObject uipanel;
    void Start()
    {

        StartCoroutine(SetAlphao());
    }
    IEnumerator SetAlphao()
    {
        yield return new WaitForSeconds(0.5f);
        TweenPosition.Begin(up, 0.5f, new Vector3(0, 100, 0));
        TweenPosition.Begin(down, 0.5f, new Vector3(0, -72, 0));
        TweenAlpha.Begin(words, 0.5f, 1);
        TweenAlpha.Begin(uipanel, 0.5f, 1);
        yield return new WaitForSeconds(2f);
        
        TweenPosition.Begin(up, 0.5f, new Vector3(-1600, 100, 0));
        TweenPosition.Begin(down, 0.5f, new Vector3(1600, -72, 0));
        TweenAlpha.Begin(words, 0.5f, 0);
        TweenAlpha.Begin(uipanel, 0.5f, 0);
        yield return new WaitForSeconds(.3f);
    }
}

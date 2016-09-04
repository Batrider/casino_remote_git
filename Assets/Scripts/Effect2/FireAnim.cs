using UnityEngine;
using System.Collections;

public class FireAnim : MonoBehaviour {

    private GameObject fort;
    private bool isAnimation;
    void Start()
    {
        isAnimation= false;
        fort = transform.FindChild("GameObject").gameObject;
    }
    public void startAnim()
    {
        StartCoroutine(AnimationFort());
    }
    private IEnumerator AnimationFort()
    {
        yield return new WaitForSeconds(0.1f);
        TweenPosition.Begin(fort,0.02f,fort.transform.localPosition-new Vector3(0,20,0));
        yield return new WaitForSeconds(0.5f);
        TweenPosition.Begin(fort,0.3f,fort.transform.localPosition+new Vector3(0,20,0));
    }
}

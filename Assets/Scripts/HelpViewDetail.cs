using UnityEngine;
using System.Collections;

public class HelpViewDetail : MonoBehaviour {
    private Color cl;
    public UI2DSprite[] uiPoint5 = new UI2DSprite[5];
    public UICenterOnChild uc;
	// Use this for initialization
	void Start () {
        uc.onCenterIndex += SetImage;
        cl = uiPoint5 [0].color;
	
	}
	
	public void SetImage(int index)
    {
        Debug.Log("index:" + index);
        foreach (UI2DSprite u2d in uiPoint5)
        {
            u2d.color = Color.white;
            u2d.SetDimensions(40,6);
            u2d.alpha = 0.2f;
        }
        uiPoint5 [index].color = Color.Lerp(Color.white, cl, 1f);
        uiPoint5 [index].SetDimensions(40, 8);
        uiPoint5 [index].alpha = 0.8f;
    }
}

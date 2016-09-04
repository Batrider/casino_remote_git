using UnityEngine;
using System.Collections;

public class SpinEffect_Mamon : MonoBehaviour {

    void Update()
    {
        GetComponent<UI2DSprite>().width = 248;
        GetComponent<UI2DSprite>().height = 392;
    }
	public void settrigger()
	{
        GetComponent<UI2DSpriteAnimation>().enabled = true;
        GetComponent<UI2DSpriteAnimation>().ResetToBeginning();
	}
}

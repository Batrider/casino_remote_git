using UnityEngine;
using System.Collections;

public class ClickEffect3D : MonoBehaviour {
	public manager manScript;
	public MeshRenderer msd; 
	private Shader shader1;
	private Shader shader3;
	void Start()
	{
		shader1 = Shader.Find("Particles/Additive");
		shader3 = Shader.Find("Transparent/Cutout/Diffuse");
	}
/*	void OnPress (bool pressed)
	{
		if(pressed)
		   msd.sharedMaterial.shader = shader1;
		else
		   msd.sharedMaterial.shader = shader3;
			
	}*/
	void OnClick()
	{
		if(!(manScript.IsNowMoving()||manager.isDraw||manager.autoSpinTimess>0))
		{
			manScript.reset ();
		}

	}
}

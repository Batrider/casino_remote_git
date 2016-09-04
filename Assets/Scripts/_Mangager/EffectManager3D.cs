using UnityEngine;
using System.Collections;

public class EffectManager3D : EffectManager {
    public bool haveAttackEffect = false;
    public Transform effectInsPos;
	public Shader shaderNormal;
	public Shader shaderShow;
	public  override void OpenEffect()
	{
		ChangeShader (shaderShow);
		if(GetComponent<Animator>())
		{
			GetComponent<Animator>().SetTrigger("WIN");
		}
        if (haveAttackEffect)
        {
            switch(gameObject.name)
            {
                case "K":StartCoroutine(KMethod());break;
                case "BASE07":StartCoroutine(BASE07Method());break;
                case "SMALL":StartCoroutine(SMALLMethod());break;
                default:
                    StartCoroutine(ShakeCamera());
                    break;
            }
        }
		StartCoroutine("CloseEffect");
	}
	IEnumerator CloseEffect()
	{
		yield return new WaitForSeconds (2f);
		ChangeShader (shaderNormal);
	}
    //虚化模型
	public void ChangeShader(Shader shader)
	{
		SkinnedMeshRenderer[] mrs = GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (SkinnedMeshRenderer mr in mrs) 
		{
			Material[] mts = mr.materials;
			foreach(Material mt in mts)
			{
				mt.shader = shader;
			}
		}
	}
    //抖动屏幕
    IEnumerator ShakeCamera()
    {
        yield return new WaitForSeconds(0.5f);
        Transform cameraTran= GameObject.FindGameObjectWithTag("MonsterCamera").transform;
        iTween.ShakePosition(cameraTran.gameObject,new Vector3(-0.015f, 0.015f, 0), 0.3f);
        StartCoroutine(CameraShake(cameraTran,0));
        StartCoroutine(CameraShake(cameraTran,0.3f));
    }
    IEnumerator CameraShake(Transform tran,float time)
    {
        yield return new WaitForSeconds(time);
        tran.localPosition=new Vector3(19,59,-602);
        
    }
    #region 实例化生成相应的特效
    IEnumerator KMethod()
    {
        yield return null;
        GameObject bombEffect=Instantiate(Resources.Load("__Model/KBomb")) as GameObject;
        bombEffect.transform.position=effectInsPos.position;
        bombEffect.transform.localEulerAngles=Vector3.zero;
        bombEffect.transform.localScale=new Vector3(0.03f,0.03f,0.03f);
    }
    IEnumerator BASE07Method()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject ballEffect=Instantiate(Resources.Load("__Model/MagicBall")) as GameObject;
        ballEffect.transform.position=effectInsPos.position;
        ballEffect.transform.localEulerAngles=Vector3.zero;
        ballEffect.transform.localScale=Vector3.one;
    }
    IEnumerator SMALLMethod()
    {
        yield return null;
        GameObject zLEffect=Instantiate(Resources.Load("__Model/ZLEffect")) as GameObject;
        zLEffect.transform.position=new Vector3(-1.3f,1.27f,-3f);
        zLEffect.transform.localEulerAngles=Vector3.zero;
        zLEffect.transform.localScale=Vector3.one;
    }

    #endregion
}

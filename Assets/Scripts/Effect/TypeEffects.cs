using UnityEngine;
using System.Collections;

public class TypeEffects : MonoBehaviour
{
    // Use this for initialization
    private AudioSource asource;
    public Object freeEffectObj;
    public Object freeEndEffectObj;
    public Object MegaEffectObj;
    public Object BigEffectObj;
    public Object MinEffectObj;
    public Object smallEffectObj;
//	public GameObject smallEffectObj;
    public Object miniJackpot;
    public Object minorJackpot;
    public Object megaJackpot;
    public Object wildEffectObj;
    public Object scatterEffectObj;

//    public GameObject wildEffectObj;
    public Animator[] ams;
    public void FreeEffectRealize()
    {
        GameObject freeObj = Instantiate(freeEffectObj) as GameObject;
        freeObj.transform.SetParent(transform);
        freeObj.transform.localEulerAngles = Vector3.zero;
        freeObj.transform.localPosition = Vector3.zero;
        freeObj.transform.localScale = Vector3.one;
    }
    public void FreeEndEffectRealize(double num)
    {
        GameObject freeEndObj = Instantiate(freeEndEffectObj) as GameObject;
        freeEndObj.transform.SetParent(transform);
        freeEndObj.transform.localEulerAngles = Vector3.zero;
        freeEndObj.transform.localPosition = Vector3.zero;
        freeEndObj.transform.localScale = Vector3.one;
//        SmallGame_Num(num.ToString(),freeEndObj);
        freeEndObj.GetComponentInChildren<PrizeCount>().StartAdd(num);
        
    }
    public void WinEffectRealize(double money)
    {
        if (money >= manager.win_threshold*manager.betBaseMoney)
        {
            GameObject winEffect;
            if(money >=20f*manager.betBaseMoney)
            {
                winEffect = Instantiate(MegaEffectObj) as GameObject;
            }else if(money >=8f*manager.betBaseMoney)
            {
                winEffect = Instantiate(BigEffectObj) as GameObject;
            }else
            {
                winEffect = Instantiate(MinEffectObj) as GameObject;
            }
            winEffect.transform.SetParent(transform);
            winEffect.transform.localEulerAngles = Vector3.zero;
            winEffect.transform.localPosition = new Vector3(0,0,500);
            winEffect.transform.localScale = Vector3.one;
            winEffect.GetComponentInChildren<PrizeCount>().StartAdd(money);
        }
    }
    public void SamllEffectRealize()
    {
        GameObject freeObj = Instantiate(smallEffectObj) as GameObject;
        freeObj.transform.SetParent(transform);
        freeObj.transform.localEulerAngles = Vector3.zero;
        freeObj.transform.localPosition = Vector3.zero;
        freeObj.transform.localScale = Vector3.one;

//        GameObject smallEffect = Instantiate(smallEffectObj) as GameObject;
 //       smallEffect.transform.localPosition = new Vector3(0, 5, 0);
	
    }
    public void WildEffectRealize()
    {
        GameObject WildObj = Instantiate(wildEffectObj) as GameObject;
        WildObj.transform.SetParent(transform);
        WildObj.transform.localEulerAngles = Vector3.zero;
        WildObj.transform.localPosition = Vector3.zero;
        WildObj.transform.localScale = Vector3.one;
    }
    public void ScatterEffectRealize(int countOfScatter)
    {
        GameObject scatterObj = Instantiate(scatterEffectObj) as GameObject;
        string suffix = string.Empty;
        if (Localization.language == "English")
            suffix = "_EN";
        else
            suffix = "_CN";

        scatterObj.transform.FindChild("Bg/num").GetComponent<UISprite>().spriteName = countOfScatter.ToString()+suffix;
        
        scatterObj.transform.SetParent(transform);
        scatterObj.transform.localEulerAngles = Vector3.zero;
        scatterObj.transform.localPosition = Vector3.zero;
        scatterObj.transform.localScale = Vector3.one;


    }
    public void JackpotEffectRealize(int val, double money)
    {
        Object prefab = null;
        //JACKPOT_TYPE_MINI
        if (val == 1)
            prefab = miniJackpot;
        else if(val == 2)
            prefab = minorJackpot;
        else if(val == 3)
            prefab = megaJackpot;
        if(prefab != null)
        {
            GameObject jackpot = Instantiate(prefab) as GameObject;
            jackpot.transform.SetParent(transform);
            jackpot.transform.localEulerAngles = Vector3.zero;
            jackpot.transform.localPosition = Vector3.zero;
            jackpot.transform.localScale = Vector3.one;
        }
    }
    IEnumerator CaidaiIns()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i<40; i++)
        {
            yield return null;
            GameObject gold = Instantiate(Resources.Load("PrefabsDone/caidai")) as GameObject;
            gold.transform.parent = transform;
            gold.transform.localScale = new Vector3(100, 100, 100);
            gold.transform.eulerAngles = Vector3.zero;
            gold.transform.localPosition = new Vector3(Random.Range(-300, 300), Random.Range(0, 400), -200);

            gold.GetComponent<UISprite>().spriteName = "caidai_" + Random.Range(1, 14);
            gold.GetComponent<UISprite>().MakePixelPerfect();
        }
    }
    IEnumerator CharAnimation(Animator[] ams, GameObject valueObj, float durTime)
    {
        foreach (Animator am in ams)
        {
            yield return new WaitForSeconds(0.1f);
            am.SetTrigger("start");
        }
        yield return new WaitForSeconds(durTime);
        foreach (Animator am in ams)
        {
            yield return new WaitForSeconds(0.1f);
            am.SetTrigger("stop");
        }
        TweenAlpha.Begin(valueObj, 0.2f, 0);
        yield return new WaitForSeconds(0.2f);
        valueObj.transform.localScale = Vector3.zero;
        valueObj.GetComponent<UI2DSprite>().alpha = 1;
        valueObj.SetActive(false);
    }
    public void SmallGame_Num(string num,GameObject freeEnd)
    {
        GameObject OffsetObj =freeEnd.transform.FindChild("add").gameObject;
        OffsetObj.GetComponent<UISprite>().width = 80;
        OffsetObj.GetComponent<UISprite>().height = 77;
        OffsetObj.transform.localPosition = new Vector3(-40*num.Length,0,0);
        Vector3 offsetChar = Vector3.zero;
        Debug.Log("Instantiate nums"+"!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        foreach(char n in num)
        {
            offsetChar += new Vector3(80,0,0);
            
            GameObject numIocn = Instantiate(Resources.Load("__Normal/nums")) as GameObject;
            numIocn.name = n.ToString();
            if(n=='.')
            {
                numIocn.GetComponent<UISprite>().spriteName ="e";
            }
            else
            {
                numIocn.GetComponent<UISprite>().spriteName = n.ToString();
            }
            numIocn.transform.parent = OffsetObj.transform;
            numIocn.transform.localScale = Vector3.one;
            numIocn.transform.eulerAngles = Vector3.zero;
            numIocn.transform.localPosition = offsetChar;
        }
    }





    void OnEnable()
    {
        manager.freeEffect += FreeEffectRealize;
        manager.freeEndEffect += FreeEndEffectRealize;
        
        manager.winEffect += WinEffectRealize;
        manager.smallEffect += SamllEffectRealize;
        manager.wildEffect += WildEffectRealize;
        manager.scatterEffect += ScatterEffectRealize;
        manager.jackEffect += JackpotEffectRealize;
    }
    void OnDisable()
    {
        manager.freeEffect -= FreeEffectRealize;
        manager.freeEndEffect -= FreeEndEffectRealize;
        
        manager.winEffect -= WinEffectRealize;
        manager.smallEffect -= SamllEffectRealize;
        manager.wildEffect -= WildEffectRealize;
        manager.scatterEffect -= ScatterEffectRealize;
        manager.jackEffect -= JackpotEffectRealize;
    }

}

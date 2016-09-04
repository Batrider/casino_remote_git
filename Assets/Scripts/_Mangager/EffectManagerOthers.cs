using UnityEngine;
using System.Collections;

public class EffectManagerOthers : EffectManager {
    [ContextMenu("OpenEffect()")]
    public override void OpenEffect()
    {
        GetComponentInChildren<UI2DSpriteAnimation>().enabled = true;
        GetComponentInChildren<UI2DSpriteAnimation>().ResetToBeginning();
        if(GetComponentInChildren<UI2DSprite>())
        {
            UI2DSprite[] uss = GetComponentsInChildren<UI2DSprite>();
            foreach(UI2DSprite us in uss)
            {
                us.enabled = true;
                us.depth = 12;
            }
        }
        StartCoroutine("CloseEffect");
    }
    IEnumerator CloseEffect()
    {
        //表演时间
        yield return new WaitForSeconds(2f);
        if(GetComponentInChildren<UI2DSprite>())
        {
            UI2DSprite[] uss = GetComponentsInChildren<UI2DSprite>();
            foreach(UI2DSprite us in uss)
            {
                us.enabled = true;
                us.depth = 8;
            }
        }
    }
}

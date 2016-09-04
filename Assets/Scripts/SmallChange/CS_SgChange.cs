using UnityEngine;
using System.Collections;

public class CS_SgChange : SmallGameChange {
    public GameObject payTable;
    public override void ChangeToSg()
    {
        StartCoroutine(InstaniateSmallGame());
        manager.smallgame = true;
        foreach (GameObject go in AlphaOBJ1)
        {
            TweenAlpha ta = TweenAlpha.Begin(go, 0.2f, 0);
            ta.delay = 4f;
            
        }
        foreach (GameObject go in AlphaOBJ2)
        {
            TweenAlpha ta = TweenAlpha.Begin(go, 0.8f, 1f);
            ta.delay = 4f;
        }
        TweenAlpha.Begin(payTable, 0.1f, 0);
        
    }
    public override void ReturnToGame()
    {
        foreach (GameObject go in AlphaOBJ1)
        {
            TweenAlpha ta = TweenAlpha.Begin(go, 0.2f, 1);
            ta.delay = 0;
        }
        foreach (GameObject go in AlphaOBJ2)
        {
            TweenAlpha ta =  TweenAlpha.Begin(go, 0.2f, 0);
            ta.delay = 0f;
        }
        StartCoroutine(YieldDelay());
    }

}

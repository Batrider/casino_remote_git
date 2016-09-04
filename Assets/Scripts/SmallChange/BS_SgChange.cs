using UnityEngine;
using System.Collections;

public class BS_SgChange : SmallGameChange {
    public GameObject stickLeft;
    public GameObject stickLeft2;
    public GameObject stickRight;
    public GameObject stickRight2;
    public GameObject iconBG;
    public GameObject iconScrollView;
    public GameObject payTable;
    public GameObject BtnsPanel;

    public GameObject smallGamePanel;
    public override void ChangeToSg()
    {
        StartCoroutine(InstaniateSmallGame());
        
        manager.smallgame = true;
        //按钮
        TweenAlpha btn = TweenAlpha.Begin(BtnsPanel, 0.3f, 0);
        btn.delay = 0;
        //杆
        TweenPosition sl = TweenPosition.Begin(stickLeft, 2, new Vector3(-3, -600, 0));
        sl.delay = 1;
        TweenPosition sr = TweenPosition.Begin(stickRight, 2, new Vector3(-3, -600, 0));
        sr.delay = 1;
        //枝
        TweenPosition sl2 = TweenPosition.Begin(stickLeft2, 1, new Vector3(-0, -194, 0));
        sl2.delay = 0;
        TweenPosition sr2 = TweenPosition.Begin(stickRight2, 1, new Vector3(-207.7f, -190, 0));
        sr2.delay = 0;
        //图标
        TweenAlpha isv = TweenAlpha.Begin(iconScrollView, 0.8f, 0);
        isv.delay = 0;
        TweenAlpha ib = TweenAlpha.Begin(iconBG, 0.8f, 0);
        ib.delay = 0;
        TweenAlpha.Begin(payTable, 0.1f, 0);
        TweenAlpha sgp = TweenAlpha.Begin(smallGamePanel, 0.5f, 1);
        sgp.delay = 3;

    }
    public override void ReturnToGame()
    {
        //按钮
        TweenAlpha btn = TweenAlpha.Begin(BtnsPanel, 0.3f, 1);
        btn.delay = 4;
        //杆
        TweenPosition sl = TweenPosition.Begin(stickLeft, 2, new Vector3(-3, -5, 0));
        sl.delay = 0;
        TweenPosition sr = TweenPosition.Begin(stickRight, 2, new Vector3(-3, 0, 0));
        sr.delay = 0;
        //枝
        TweenPosition sl2 = TweenPosition.Begin(stickLeft2, 1, new Vector3(0, 130, 0));
        sl2.delay = 2;
        TweenPosition sr2 = TweenPosition.Begin(stickRight2, 1, new Vector3(-207.7f, 128.98f, 0));
        sr2.delay = 2;
        //图标
        TweenAlpha isv = TweenAlpha.Begin(iconScrollView, 0.8f, 1);
        isv.delay = 4;
        TweenAlpha ib = TweenAlpha.Begin(iconBG, 0.8f, 1);
        ib.delay = 3.5f;
        TweenAlpha sgp = TweenAlpha.Begin(smallGamePanel, 0.5f, 0);
        sgp.delay = 0;
        
        StartCoroutine(YieldDelay());
    }

}

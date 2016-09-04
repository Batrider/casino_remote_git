using UnityEngine;
using System.Collections;
using LitJson;
using System.Text;

public class ClickEffect2D : MonoBehaviour {
	public manager manScript;
    bool isCanTap = true;
    bool isRequiring = false;
    public void BtnsState(GameState gs)
    {
        if(gs == GameState.stop)
        {
            isCanTap = true;

        }
        else
        {
            //音效
            GetComponent<UIPlaySound>().Play();
            isCanTap = false;
        }
    }
	void OnClick()
	{
        //没有在转动、没有中奖线展示、没有自动转动的情况下 spin 按钮可按
        /*
		if (!(manScript.IsNowMoving() || manScript.isDraw || manager.autoSpinTimess > 0))
        {
            TweenAlpha.Begin(GameObject.FindGameObjectWithTag("Help"),0.2f,0);
            manScript.reset();
        }
        */
        if (isCanTap&&manScript.DoTheMoneyEnough())
        {
            isCanTap = false;
            GameObject.FindGameObjectWithTag("Help").GetComponent<UIPanel>().alpha = 0;
            StartCoroutine(WheelsEffect());
        }else if(!manScript.DoTheMoneyEnough())
        {
            //
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkConnect>().GAME_Balance_Refresh();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkConnect>().StartCoroutine("ShowInsufficientBalanceTip");

        }
    }
    IEnumerator WheelsEffect()
    {
        TweenPosition tp = TweenPosition.Begin(GameObject.FindGameObjectWithTag("Wheels").transform.FindChild("Grid").gameObject,.3f,new Vector3(0,15,0));
        yield return new WaitForSeconds(.3f);
        tp.from = new Vector3(0,15,0);
        tp.to = Vector3.zero;
        tp.duration = 0.2f;
        tp.enabled = true;
        tp.ResetToBeginning();
        manScript.reset();
        
    }
    public void DragSpin()
    {
        if(!(manScript.IsNowMoving()||manager.isDraw||manager.autoSpinTimess>0)&&manScript.DoTheMoneyEnough())
        {
            manScript.reset ();
        }
    }
    void OnEnable()
    {
        manager.gameStateDelegate += BtnsState;

    }
    void OnDisable()
    {
        manager.gameStateDelegate -= BtnsState; 

    }
}

/*
 * 用来管理游戏中按钮的状态 及 物体的状态
 * 
 * 
 * */
using UnityEngine;
using System.Collections;

public class BtnObjManager : MonoBehaviour
{
    public GameObject stopbtn;
    public GameObject spinTimes;
    public void BtnsState(GameState gs)
    {
        if (gs == GameState.start)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("buttons");
            foreach (GameObject go in gos)
            {
                go.GetComponent<UIButton>().state = UIButtonColor.State.Disabled;
                go.GetComponent<BoxCollider>().enabled = false;
            }
            GameObject[] labels = GameObject.FindGameObjectsWithTag("BetLabels");
            foreach (GameObject label in labels)
            {
                TweenAlpha.Begin(label, 0.2f, 0.5f);
            }
            if (manager.autoSpinTimess > 0)
            {
                stopbtn.SetActive(true);
            }

        }
        if (gs == GameState.stop)
        {
            if (manager.autoSpinTimess == 0)
            {
                stopbtn.SetActive(false);
            }
            GameObject[] gos = GameObject.FindGameObjectsWithTag("buttons");
            foreach (GameObject go in gos)
            {
                go.GetComponent<BoxCollider>().enabled = true;
                go.GetComponent<UIButton>().state = UIButtonColor.State.Normal;
            }
            GameObject[] labels = GameObject.FindGameObjectsWithTag("BetLabels");
            foreach (GameObject label in labels)
            {
                TweenAlpha.Begin(label, 0.2f, 1f);
            }
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
    //set auto time value to 0,and speed up the speed of game
    public void StopMethod()
    {
        manager.autoSpinTimess = 0;
//        Time.timeScale = 3.0f;
        stopbtn.SetActive(false);
        spinTimes.SetActive(false);
        spinTimes.transform.parent.GetComponent<UISprite>().enabled = true;
    }
}

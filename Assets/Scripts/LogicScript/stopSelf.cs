using UnityEngine;
using System.Collections;

public class stopSelf : MonoBehaviour
{

    //子图标是否开始停止转动
    [HideInInspector]
    public bool
        Isstop = true;
    //当前对应的即将生成的目标图标索引
    [HideInInspector]
    public int
        curIndexOfPrefabs = 0;

    public void JudgeIfStop(int lineIndex)
    {
        //每隔1秒钟停止一列
//        Debug.Log("manager.canmove[" + lineIndex + "]:" + manager.canmove [lineIndex]);
        if (manager.canmove [lineIndex] == true)//&&gameObject.name==(lineIndex+1).ToString())
        {
            Isstop = true;
            if (lineIndex == 4)
            {
                manager.startGame = false;
            }
        }
    }
    //延迟设置 滚轮转动状态为false
    public void DelayTime(int vIndex)
    {
        StartCoroutine(WaitToStop(vIndex));
    }
    IEnumerator WaitToStop(int vIndex)
    {
        if (GameObject.Find("UI Root").GetComponent<AudioManager>())
        {
            if (!GetComponentInParent<StopController>().DetachIconOne(vIndex))
                GameObject.Find("UI Root").GetComponent<AudioManager>().WheelStopMusic(true);
            else
                GameObject.Find("UI Root").GetComponent<AudioManager>().WheelStopMusic(false);
        }
   //     NGUIDebug.Log(Time.realtimeSinceStartup + "Into Detect Icon move!+" + vIndex);
        SpringPosition2D[] sp = transform.GetComponentsInChildren<SpringPosition2D>();
        //while wheels are stop,stop;
        while (true)
        {
            bool isStop = true;
            for (int i=0; i<sp.Length; i++)
            {
                isStop = isStop & (!sp [i].enabled);
            }
            if (isStop)
                break;
            yield return null;

      //      NGUIDebug.Log("is stop?:" + isStop);
        }
        yield return new WaitForSeconds(1f);
        //播放图标动画
        UI2DSpriteAnimation[] ui2dSpriteAnimations = transform.GetComponentsInChildren<UI2DSpriteAnimation>();
        if (ui2dSpriteAnimations.Length > 0)
        {
            foreach (UI2DSpriteAnimation ui2dspriteanimation in ui2dSpriteAnimations)
            {
                if (ui2dspriteanimation.transform.name.ToLower().Contains("free") || ui2dspriteanimation.transform.name.ToLower().Contains("small"))
                {
                    ui2dspriteanimation.enabled = true;
                    ui2dspriteanimation.loop = true;
                }
            }
        }
        else
        {
            EffectManager2D[] effectManagers = transform.GetComponentsInChildren<EffectManager2D>();
            foreach (EffectManager2D effectManager in effectManagers)
            {
                if (effectManager.name == "FREE" || effectManager.name == "SCATTER"|| effectManager.name == "SMALL")
                {
                    effectManager.OpenEffect();
                }
            }
        }
        manager.canmove [vIndex - 1] = false;
   //     NGUIDebug.Log(Time.realtimeSinceStartup + "vIndex: " + vIndex);
    }


}
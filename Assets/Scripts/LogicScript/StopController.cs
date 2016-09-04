using UnityEngine;
using System.Collections;

public class StopController : MonoBehaviour
{
    public stopSelf[] ss;
    public float StopTime = 0.5f;
    
    private float yieldTime = -0.5f;
    private int whichline;
    //15个图标预设
    public Object[] prefabss;
    public Sprite[] sprite15 = new Sprite[15];
    //中奖特效
    private Object SpeedEffect;
    void Awake()
    {
        whichline = 0;
        SpeedEffect = Resources.Load("SpeedEffect");
        Debug.Log(prefabss.Length);
        for(int i = 0;i<15;i++)
        {
            GameObject sp = (Instantiate(prefabss[i]) as GameObject);
            if (sp.GetComponentInChildren<UI2DSprite>())
            {
                sprite15[i] = sp.GetComponentInChildren<UI2DSprite>().sprite2D;
            }
            Destroy(sp);
        }
    }
    void FixedUpdate()
    {
        if (manager.startGame && manager.isRecieveData)
            yieldTime += Time.fixedDeltaTime;
		
        if (yieldTime > StopTime)
        {
            yieldTime = 0;
            ss [whichline].JudgeIfStop(whichline);
            whichline++;
            if (whichline >= 2 && DetachIcon(whichline) && whichline <= 4)
            {
                //检测前两列是否有2个free或2个
                StopTime += 0.7f;
                //加速旋转
                GameObject lineGameObj = transform.FindChild((whichline + 1).ToString()).gameObject;
                InstanPrefabs[] ips = lineGameObj.GetComponentsInChildren<InstanPrefabs>();
                foreach (InstanPrefabs ip in ips)
                {
                    ip.runSpeed = 2 * ip.runSpeed;
                }
                StartCoroutine(InstaniateSpeedUpEffect(lineGameObj, StopTime));
            }
            //Debug.Log("whichline:" + whichline);
            if (whichline == 5)
            {
                manager.isRecieveData = false;
                StopTime = 0.5f;
                whichline = 0;
            }
        }
    }
    public bool DetachIcon(int which)
    {
        int freeIconCount = 0;
        int bonsIconCount = 0;
        for (int i = 0; i<3*which; i++)
        {
            if (manager.IconNode [i] == 9)
                freeIconCount++;
            else if (manager.IconNode [i] == 10)
                bonsIconCount++;
        }
        return freeIconCount >= 2 | bonsIconCount >= 2;
    }
    //检测当前列是否有free或bons
    public bool DetachIconOne(int which)
    {
        int freeIconCount = 0;
        int bonsIconCount = 0;
        if (which <= 3)
        {
            for (int i = 3*(which-1); i<3*which; i++)
            {
                if (manager.IconNode [i] == 9)
                    freeIconCount++;
                else if (manager.IconNode [i] == 10)
                    bonsIconCount++;
            }
            return freeIconCount >= 1 | bonsIconCount >= 1;
        } else
        {
            return EnoughIconCount(9,which,which-2)|EnoughIconCount(10,which,which-2);
        }
    }
    bool EnoughIconCount(int iconTag,int whichLine,int minCount)
    {
        int count =0;
        bool specialMusic;
        for (int i = 0; i<3*whichLine; i++)
        {
            if (manager.IconNode [i] == iconTag)
                count++;
        }
        specialMusic = count >= minCount;
        count = 0;
        for (int i = 3*(whichLine-1); i<3*whichLine; i++)
        {
            if (manager.IconNode [i] == iconTag)
                count++;
        }
        return (count >= 1)&specialMusic;
    }
    IEnumerator InstaniateSpeedUpEffect(GameObject whichLine, float time)
    {
        yield return null;
        GameObject speedUpObj = Instantiate(SpeedEffect) as GameObject;
        speedUpObj.GetComponent<DestroyEffect>().DestroyTime = time + 0.3f;
        speedUpObj.transform.parent = whichLine.transform;
        speedUpObj.transform.localPosition = new Vector3(-5, -300, 0);
        speedUpObj.transform.localScale = Vector3.one;
        speedUpObj.transform.localEulerAngles = Vector3.zero;
        yield return null;

        speedUpObj.GetComponent<UISpriteAnimation>().enabled = true;
    }
              
}

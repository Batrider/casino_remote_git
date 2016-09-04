using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public enum CarState{
        runForward = 1,
        runBack = -1,
        stop = 0
    }
    public enum RopeState
    {
        Elong = 1,
        Eshort = -1,
        Stop = 0
    }
    public CarState carState = CarState.stop;
    public GameObject[] carWheels;
    public UISprite rope;
    public GameObject toolHead;
    public float translateSpeed ;
    public GameObject[] support = new GameObject[2];
    public GameObject ropeRotate;
    public RopeState ropeState = RopeState.Stop;
    private bool arrived = false;
    void Update()
    {
        switch(carState)
        {
            case CarState.stop:break;
            case CarState.runForward:CarRun(1);break;
            case CarState.runBack:CarRun(-1);break;
            default:break;
        }
    }
    public void CarRun(int value)
    {
        for (int i = 0; i<carWheels.Length; i++)
        {
            carWheels [i].transform.Rotate(new Vector3(0, 0,value*-500f*Time.deltaTime*translateSpeed));
        }
//        transform.Translate(new Vector3(value*translateSpeed*Time.deltaTime,0,0));
    }
    IEnumerator RunToTargetPositon(float targetPosX)
    {
        arrived = false;
        StartCoroutine(OpenTools());
        Debug.Log("============start==============");
        float curTime = 0;
        Vector3 startPos = this.transform.position;
        carState = (targetPosX - this.transform.position.x)>0?CarState.runForward:CarState.runBack;
        while(Mathf.Abs(targetPosX - this.transform.position.x)>0.001f)
        {
            yield return null;
            curTime +=Time.deltaTime;
            this.transform.position = new Vector3(Mathf.Lerp(startPos.x,targetPosX,curTime),this.transform.position.y,this.transform.position.z);
        }
        carState = CarState.stop;
        ropeState = RopeState.Elong;
        yield return StartCoroutine(OperateTheRope());
        while(!arrived)
        {
            yield return null;
        }
        
    }
    public IEnumerator OpenTools()
    {
        Debug.Log(support[1].transform.localEulerAngles.z);
        while(support[1].transform.localEulerAngles.z<30)
        {
            yield return null;
            support[0].transform.localEulerAngles += new Vector3(0,0,-50*Time.deltaTime);
            support[1].transform.localEulerAngles += new Vector3(0,0,50*Time.deltaTime);
        }
    }
    public IEnumerator CloseTools()
    {
        ropeState = RopeState.Stop;
        while(support[0].transform.localEulerAngles.z>1)
        {
            yield return null;
            support[0].transform.localEulerAngles += new Vector3(0,0,50*Time.deltaTime);
            support[1].transform.localEulerAngles += new Vector3(0,0,-50*Time.deltaTime);
        }
        ropeState = RopeState.Eshort;
        StartCoroutine(OperateTheRope());
    }
    public IEnumerator OperateTheRope()
    {
        float countHeight = 0;
        int relative = 0;
        while( ropeState!= RopeState.Stop)
        {
            relative = (int)ropeState;
            yield return null;
            countHeight += 100*Time.deltaTime;
            if(countHeight>1)
            {
                rope.height = rope.height+8*relative;
                toolHead.transform.localPosition += new Vector3(0,-8*relative,0);
                ropeRotate.transform.Rotate(new Vector3(0,0,-5*relative));
                countHeight = 0;
                if(toolHead.transform.localPosition.y>-60)//收缩回来停止
                {
                    ropeState = RopeState.Stop;
                    arrived = true;
                }
            }
        }
    }
    public void GetPrize()
    {
        ropeState = RopeState.Stop;
    }

}

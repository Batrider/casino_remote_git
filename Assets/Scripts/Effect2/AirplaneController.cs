using UnityEngine;
using System.Collections;

public class AirplaneController : MonoBehaviour {
    public Object bombObj;
    public Vector3 targetPos;
    public Vector3 aimPos;
    public static bool move;
    private Vector3 startPos;
    private float time;
    //炸弹投放位置
    public float[] xPosition;
    private int index;
    private bool success;
    private bool putBomb = false;
    private AudioSource airPlaneAs;
    private float distance;
    private float updateDistance;
    void Start()
    {
        distance = Vector3.Distance(transform.localPosition,targetPos);
        airPlaneAs = GetComponent<AudioSource>();
        move = true;
        transform.localPosition = new Vector3(-1180,188.5f,0);
        startPos = transform.localPosition;
    }
    //飞机的移动
    void FixedUpdate()
    {
        if(!move)
        {
            time +=.1f*Time.fixedDeltaTime;
            transform.localPosition = Vector3.Lerp(startPos,targetPos,time);
            updateDistance = Vector3.Distance(transform.localPosition,targetPos);
            airPlaneAs.volume = (updateDistance/distance)<0.2f?(updateDistance/distance)/0.2f:(updateDistance/distance)>0.8f?((distance-updateDistance)/distance)/0.2f:1;

            if(updateDistance<1)
            {
                
                transform.localPosition = startPos;
                move = true;
                time = 0;
                putBomb = false;

            }
            if(!putBomb&&transform.localPosition.x>xPosition[index])
            {
                putBomb = true;
                // InstaniateBomb();
                StartCoroutine(BombTrack());
            }
        }
    }
    //生成炸弹
    void InstaniateBomb()
    {
        GameObject bomb = Instantiate(bombObj) as GameObject;
        bomb.transform.parent = transform.parent;
        bomb.transform.localPosition = transform.localPosition-new Vector3(-100,60,0);
        bomb.transform.localEulerAngles = new Vector3(0,0,90);
        bomb.transform.localScale = Vector3.one;
        bomb.GetComponent<Rigidbody2D>().velocity = new Vector2((1080/Screen.height),0);
        bomb.GetComponent<Bomb>().success = this.success;
    }
    IEnumerator BombTrack()
    {
        GameObject bomb = Instantiate(bombObj) as GameObject;
        bomb.transform.parent = transform.parent;
        bomb.transform.localPosition = transform.localPosition - new Vector3(-100, 60, 0);
        bomb.transform.localEulerAngles = new Vector3(0, 0, 90);
        bomb.transform.localScale = Vector3.one;
        bomb.GetComponent<Bomb>().success = this.success;

        while (bomb != null&&Vector3.Distance(bomb.transform.position, aimPos) >0.2f)
        {
            bomb.transform.position = Vector3.Lerp(bomb.transform.position, aimPos, Time.deltaTime*5f);
            yield return null;
        }
        bomb.GetComponent<Bomb>().BombEffect(aimPos);

    }
    public void AirplaneStartRun(float yPosition,bool success,int index)
    {
        this.index = index-1;
        Debug.Log("this.index :"+this.index);
        this.success = success;
        transform.localPosition = new Vector3(transform.localPosition.x,yPosition,0);
        targetPos = new Vector3(targetPos.x,yPosition,0);
        move = false;
        airPlaneAs.volume = 1;
    }

}

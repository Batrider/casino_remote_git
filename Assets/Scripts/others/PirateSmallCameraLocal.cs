using UnityEngine;
using System.Collections;

public class PirateSmallCameraLocal : MonoBehaviour {
    private  double normalScale1 = 1.6f;
    private  double normalFactor1 = 0.1723f;
    float deltaFreshTime;
    Vector3 startPos;
    void Start()
    {
        startPos = transform.localPosition;
        FreshPosition();
    }

    void Update()
    {
        deltaFreshTime += Time.deltaTime;
        if(deltaFreshTime>.5f)
        {
            deltaFreshTime =  0;
            FreshPosition();
        }
    }
    void FreshPosition()
    {
        Debug.Log("Screen.width:"+Screen.width+";Screen.height:"+Screen.height);
        double deltaX = ((1f*Screen.width/Screen.height)-normalScale1)/normalFactor1;
        Debug.Log(deltaX);
        transform.localPosition = new Vector3(startPos.x-(float)deltaX,0,0);
    }
}

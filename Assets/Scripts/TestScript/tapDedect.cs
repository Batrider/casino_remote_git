using UnityEngine;
using System.Collections;

public class tapDedect : MonoBehaviour {
    public Camera cardmain;

    void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cardmain.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo))
            {
                Debug.DrawLine(ray.origin,hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                if(gameObj.name == "LeftClick"||gameObj.name == "RightClick")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    EventDelegate.Execute(gameObj.GetComponent<UIButton>().onClick);
                    gameObj.GetComponent<UIButton>().state = UIButtonColor.State.Pressed;
                    
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = cardmain.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo))
            {
                Debug.DrawLine(ray.origin,hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                if(gameObj.name == "LeftClick"||gameObj.name == "RightClick")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    gameObj.GetComponent<UIButton>().state = UIButtonColor.State.Normal;
                }
            }
        }
    }

}

using UnityEngine;
using System.Collections;

public class DragAndPlay : MonoBehaviour {
//    public Object starObj;
    manager manScript;
    public float distance;
    bool isCanTap = true;
//    float insTimedelta;
                               	// Use this for initialization
    public void BtnsState(GameState gs)
    {
        if(gs == GameState.stop)
        {
            isCanTap = true;
        }
        else
        {
            isCanTap = false;
        }
    }
    void OnDrag (Vector2 delta)
    {
        distance += delta.y;

        Debug.Log(Input.mousePosition);

    }
    void OnDragEnd()
    {
        if (distance < -100&&isCanTap)
        {
            isCanTap = false;
            TweenAlpha.Begin(GameObject.FindGameObjectWithTag("Help"),0.2f,0);
            manScript.reset();
        }
        distance = 0;
    }
    public void OnHoverEvent()
    {
        Debug.Log("hover");
    }
    void Start()
    {
        manScript = GameObject.Find("/UI Root").GetComponent<manager>();
        manager.gameStateDelegate += BtnsState;
    }
    void OnDisable()
    {
        manager.gameStateDelegate -= BtnsState;        
    }
}

using UnityEngine;
using System.Collections;

public class GunSightEffect : MonoBehaviour {
    public Transform clothParent;
    public Object dusterClothPre;
    private float timeMax = 0;
    public Camera ca;
    public Sg_Victory sgVictory;
    GameObject dusterCloth;
    void Start()
    {
        ca = transform.parent.parent.parent.GetComponent<Camera>();
    }
    void Update()
    {
#if UNITY_EDITOR
        //鼠标版
        if (Input.GetMouseButtonDown(0))
        {
            dusterCloth = Instantiate(dusterClothPre) as GameObject;
            dusterCloth.transform.position = ca.ScreenToWorldPoint(Input.mousePosition);
            dusterCloth.transform.parent = clothParent;
            dusterCloth.transform.localScale = Vector3.one;
            dusterCloth.transform.localEulerAngles = Vector3.zero;
        }
        if (Input.GetMouseButton(0))
        {
            dusterCloth.transform.position = ca.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            Ray ray = ca.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "1" || hit.collider.name == "2")
                {
                    timeMax += Time.deltaTime;                    
                    if (timeMax > 1f)
                    {
                        timeMax = 0;
                        //触发按钮 
                        Debug.Log("Send Message Trigger");
                        transform.parent.GetComponent<Sg_Aladdin>().SelectBtnMessage(hit.collider.gameObject);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //删掉抹布
            Destroy(dusterCloth);
            timeMax = 0;
        }
#else

        //触屏版
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    dusterCloth = Instantiate(dusterClothPre) as GameObject;
                    dusterCloth.transform.position = ca.ScreenToWorldPoint(Input.GetTouch(0).position);
                    dusterCloth.transform.parent = clothParent;
                    dusterCloth.transform.localScale = Vector3.one;
                    dusterCloth.transform.localEulerAngles = Vector3.zero;
                    break;
                case TouchPhase.Moved:
                    dusterCloth.transform.position = ca.ScreenToWorldPoint(Input.GetTouch(0).position);
                    RaycastHit hit;
                    Ray ray = ca.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.name == "1" || hit.collider.name == "2")
                        {
                            timeMax += Time.deltaTime;                    
                            if (timeMax > 1f)
                            {
                                timeMax = 0;
                                //触发按钮                                 
                                transform.parent.GetComponent<Sg_Aladdin>().SelectBtnMessage(hit.collider.gameObject);
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    //删掉抹布
//                    sgVictory.SelectBtnMessage();
                    Destroy(dusterCloth);
                    timeMax = 0;
                    break;
            }
        }
#endif

        }
}

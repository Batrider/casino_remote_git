using UnityEngine;
using System.Collections;

public class VictoryBomb : MonoBehaviour {
    public Object bombEffect;
    public Object addMoneyLabel;
    private float speed = 2f;
    [HideInInspector]
    public GameObject Ship;
    public float failDistance = 0.1f;
//    private float 
    void Update()
    {
//        Ship = FigureOutTheNearestShip();
        transform.position = Vector3.Lerp(transform.position,Ship.transform.position,speed*Time.deltaTime);
        if(Vector3.Distance(transform.position,Ship.transform.position)<failDistance)
        {
            if(failDistance>0.2f)
            {
            GameObject bombE = Instantiate(bombEffect) as GameObject;
            bombE.transform.parent = transform.parent;
            bombE.transform.localPosition = transform.localPosition;
            bombE.transform.localEulerAngles = Vector3.zero;
            bombE.transform.localScale = Vector3.one;
                AddMethod();
            }
            else
            {
                Ship.GetComponent<ShipAnimation>().StartCoroutine("Explosion");
                AddMethod();
            }

            //执行动画
            Destroy(gameObject);
        }

    }
    GameObject FigureOutTheNearestShip()
    {
        GameObject nearestShip = null;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("ship");
        float distance = float.MaxValue;
        foreach(GameObject ship in ships)
        {
            if(Vector3.Distance(ship.transform.position,transform.position)<distance)
            {
                distance = Vector3.Distance(ship.transform.position,transform.position);
                nearestShip = ship;
            }
        }
        return nearestShip;
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =this.transform.parent;
        labelObj.transform.localScale =1.5f*Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        
        
        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();
        
        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.duration = 2;
        tp.from = transform.localPosition;
        tp.to = transform.localPosition + new Vector3(0,150,0);
        tp.enabled = true;
        tp.PlayForward();
        
        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        tp.duration = 2;
        ta.enabled = true;
        ta.PlayReverse();
    }
    void AddMethod()
    {
        SmallGame sgScript = GetComponentInParent<SmallGame>();
        Debug.Log("sgScript.money_small:"+sgScript.money_small);
        AddMoney(sgScript.money_small);
        sgScript.smallPrizeLabel.text = (double.Parse(sgScript.smallPrizeLabel.text)+sgScript.money_small).ToString();
    }
}

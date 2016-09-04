using UnityEngine;
using System.Collections;
public class Bomb : MonoBehaviour {
    public Object addMoneyLabel;
    public Object bombEffect;
    public Object fireEffect;
    [HideInInspector]
    public bool success;
    public void BombEffect(Vector3 aimPos)
    {
        AddMethod();
        //爆炸效果
 //       if(success)
  //      {
        GameObject effect = Instantiate(bombEffect) as GameObject;
        effect.transform.parent = transform.parent;
        effect.transform.position = aimPos;
        effect.transform.localPosition += new Vector3(0, 100, 0);
        effect.transform.localScale = Vector3.one;
        effect.transform.localEulerAngles = Vector3.zero;

        GameObject fire = Instantiate(fireEffect) as GameObject;
        fire.transform.parent = transform.parent;
        fire.transform.position = aimPos;
        fire.transform.localPosition += new Vector3(0, 100, 0);

        fire.transform.localScale = Vector3.one;
        fire.transform.localEulerAngles = Vector3.zero;

        Destroy(gameObject);
    }
    void AddMoney(double moneyCount)
    {
        GameObject labelObj = Instantiate(addMoneyLabel) as GameObject;
        labelObj.transform.parent =this.transform.parent;
        labelObj.transform.localScale =1.5f*Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;
        
        labelObj.GetComponent<UILabel>().bitmapFont = GameObject.FindGameObjectWithTag("SgCamera").GetComponentInChildren<Sg_ZombieHead>().smallGameTimesLabel.bitmapFont;
        
        
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
        GetComponentInParent<Sg_ZombieHead>().ShoeZombieEffect();
    }
}

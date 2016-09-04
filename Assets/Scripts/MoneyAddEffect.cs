using UnityEngine;
using System.Collections;

public class MoneyAddEffect : MonoBehaviour {
    public Object labelPrefabs;
    public GameObject background;

    public void ExcuteAddMoneyEffect(double moneyCount)
    {
        /*
        GameObject labelObj = Instantiate(labelPrefabs) as GameObject;
        labelObj.transform.parent = transform;
        labelObj.transform.localScale = Vector3.one;
        labelObj.transform.localEulerAngles = Vector3.zero;



        labelObj.GetComponent<UILabel>().text ="+" + moneyCount.ToString();

        TweenPosition tp = labelObj.GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.PlayForward();

        TweenAlpha ta = labelObj.GetComponent<TweenAlpha>();
        ta.enabled = true;
        ta.PlayForward();
        **/


//        GameObject.FindGameObjectWithTag("GameManager").GetComponent<manager>().AddMoney(moneyCount,false);
        StartCoroutine(SplahTheBackgrond());

    }
    IEnumerator SplahTheBackgrond()
    {
        if (background != null)
            background.GetComponent<TweenColor>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        if (background != null)
        {
            background.GetComponent<TweenColor>().enabled = false;
            background.GetComponent<UISprite>().color = Color.white;
        }


    }
}

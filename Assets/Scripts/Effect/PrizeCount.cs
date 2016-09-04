using UnityEngine;
using System.Collections;

public class PrizeCount : MonoBehaviour {
    public UILabel text;
    public bool addadd = false;
    public bool isAddFormBaseMoney = false;
    [SerializeField]
    public double totalMoney = 0;
    private float timeMax = 1f;
    public void StartAdd(double index)
    {
        StartCoroutine(IAddMoney(index));
    }
    IEnumerator IAddMoney(double count)
    {
        yield return new WaitForSeconds(1f);
        double addDelta =Time.fixedDeltaTime*(count/timeMax);
        double currentMoney = 0;
        if(isAddFormBaseMoney)
            currentMoney = double.Parse(text.text);
        float tempTime = 0;
        while(tempTime<timeMax-Time.fixedDeltaTime)
        {
            currentMoney += addDelta;
            if(addadd)
                text.text = "+" + currentMoney.ToString("f2");
            else
                text.text = currentMoney.ToString("f2");
            tempTime+=Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        if(addadd)
            text.text ="+" + count.ToString("f2");
        else
            text.text = count.ToString("f2");
            
    }
    public void StartAdd2(double index)
    {
        StartCoroutine(IAddMoney2(index));
    }
    IEnumerator IAddMoney2(double count)
    {
        double addDelta =Time.fixedDeltaTime*(count/timeMax);
        double currentMoney = totalMoney;
        totalMoney +=count;
        while(double.Parse(text.text)<totalMoney)
        {
            currentMoney += 0.22f;
            if(addadd)
                text.text = "+" + currentMoney.ToString("f2");
            else
                text.text = currentMoney.ToString("f2");
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        if(addadd)
            text.text ="+" + totalMoney.ToString("f2");
        else
            text.text = totalMoney.ToString("f2");
        
    }
}

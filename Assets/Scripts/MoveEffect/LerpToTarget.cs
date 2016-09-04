using UnityEngine;
using System.Collections;
public class LerpToTarget : MonoBehaviour {
    private GameObject credit;
    private Vector3 startPos;
    public float coefficient =1;
    private float coeYOffset =0;
    private float timeSum;
    private float distance = 0f;
    void Start()
    {
        credit = GameObject.FindGameObjectWithTag("Credits");
        startPos = transform.position;
        timeSum = 0;
        distance = Vector3.Distance(startPos, credit.transform.position);
    }
    void FixedUpdate()
    {
        timeSum += coefficient*Time.fixedDeltaTime;
        coeYOffset = Mathf.PI*Mathf.Abs(1-timeSum);
        transform.position = Vector3.Lerp(startPos, credit.transform.position+0.1f*Vector3.up, timeSum) + new Vector3(0, Mathf.Sin(coeYOffset) * 0.15f * distance);
        if (Vector3.Distance(transform.position, credit.transform.position+0.1f*Vector3.up) < 0.001f)
        {
            if(credit.GetComponent<Animator>())
                credit.GetComponent<Animator>().SetTrigger("add");
            credit.GetComponent<AudioSource>().Play();
            double winMoney = manager.win;
            credit.GetComponent<MoneyAddEffect>().ExcuteAddMoneyEffect(winMoney);
            Destroy(gameObject);
        }
    }
}
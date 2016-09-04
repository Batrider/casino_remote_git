using UnityEngine;
using System.Collections;

public class CreditShow : MonoBehaviour {
    private UILabel realCredit;
    bool isUpdate = false;
    void Start()
    {
        realCredit = GetComponent<UILabel>();
    }
    void OnHover(bool state)
    {
        if(state)
        {
            Conversion.isTransCredits = false;
            realCredit.text = manager.cur_User_Account.ToString("f2");
        }
        else
        {
            Conversion.isTransCredits = true;
            realCredit.text = Conversion.MoneyUnitConver(manager.user_Account);
        }

    }
    void Update()
    {

        if(isUpdate)
        {
            isUpdate = false;
            realCredit.text = manager.user_Account.ToString("f2"); ;
        }
    }
    void BalanceRefresh(double balance)
    {
        manager.user_Account = balance;
        isUpdate = true;
    }

    void OnEnable()
    {
        NetworkConnect.balanceRefresh += BalanceRefresh;
    }
    void OnDisable()
    {
        NetworkConnect.balanceRefresh-= BalanceRefresh;  
    }
}

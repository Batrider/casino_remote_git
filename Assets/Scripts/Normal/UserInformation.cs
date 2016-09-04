using UnityEngine;
using System.Collections;

public class UserInformation : MonoBehaviour {
    public UILabel name;
    public UILabel credit;
    int flagOfIndex;
    bool recieve;
    bool isUpdate = false;
    void Start()
    {
        if(Conversion.SceneNameIDConver(SceneManager.preSceneStr) != 0)
            setUserInformation();
    }
    void Update()
    {
        if(recieve)
        {
            recieve = false;
            Invoke("setUserInformation", 0.2f);
        }
        if(isUpdate)
        {
            isUpdate = false;
            credit.text = manager.user_Account.ToString();
        }
    }

    public void LoginResult(int flag)
    {
        recieve = true;
        flagOfIndex = flag;
    }

    public void setUserInformation()
    {
        name.text = PlayerPrefs.GetString("user_name");
        credit.text = manager.user_Account.ToString();
    }
    void BalanceRefresh(double balance)
    {
        manager.user_Account = balance;
        isUpdate = true;
    }

    void OnEnable()
    {
        NetworkConnect.login_Callback += LoginResult;
        NetworkConnect.balanceRefresh += BalanceRefresh;

    }
    void OnDisable()
    {
        NetworkConnect.login_Callback -= LoginResult;
        NetworkConnect.balanceRefresh -= BalanceRefresh;

    }
}

using UnityEngine;
using System.Collections;

public class NewLogin : MonoBehaviour {
    public GameObject SettingPanel;
    public GameObject LoginBox;
    public GameObject waitBox;
    public GameObject SignInBox;
    public GameObject shadowBackgrond;
    public GameObject exitBox;
    public UILabel LoginTip;
    public UILabel SignUpTip;
    public UILabel agentCode;
    public UILabel user_Name;
    public UIInput user_password;
    public UILabel register_name;
    public UILabel register_password;
    public UILabel confirm_password;

    private bool islogin;
    private bool isSign;
    private bool isLoginByInterface;
    private int flagOfIndex;
    public void Start()
    {
        //设置背景卡片不可点击
        StartCoroutine(SetCardState(false));
        //检查是否有用户登录过,有则预先填入信息
        if(PlayerPrefs.GetString("user_name")!=""&&!SceneManager.loseConnection)
        {
            agentCode.text = PlayerPrefs.GetString("agentCode");
            user_Name.text = PlayerPrefs.GetString("user_name");
            user_password.value = PlayerPrefs.GetString("user_password");
            StartCoroutine(SureLogin());
        }
        else
            SetScaleToOne(LoginBox);
    }
    void Update()
    {
        if(islogin)
        {
            islogin = false;
            LoginDispose();
        }
        if(isSign)
        {
            isSign = false;
            SignDispose();
        }
        if(Application.platform == RuntimePlatform.Android&&Input.GetKeyDown(KeyCode.Escape))
        {
            //弹出提示窗口是否退出游戏、或者继续游戏
            SetScaleToOne(exitBox);
        }
        if (isLoginByInterface)
        {
            waitBox.SetActive(false);
            isLoginByInterface = false;
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error" + flagOfIndex;
        }
    }
    //登录请求结果
    public void LoginResult(int flag)
    {
        flagOfIndex = flag;
        Debug.LogWarning("error:"+flag);
        islogin = true;
    }
    public void LoginResultByInterface(int flag)
    {
        isLoginByInterface = true;
        flagOfIndex = flag;
    }
    //注册请求结果
    public void SignResult(int flag)
    {
        isSign = true;
        flagOfIndex = flag;
    }
    void SignDispose()
    {
        Debug.Log(flagOfIndex);
        waitBox.SetActive(false);
        if(flagOfIndex == 1)
        {
            //提示用户注册失败
            if(Localization.language == "English")
                SignUpTip.text = "Registration Failed";
            else if(Localization.language == "简体中文")
                SignUpTip.text = "注册失败";
        }
        else if(flagOfIndex ==0)
        {
            //成功，紧接着到达登录界面1
            PlayerPrefs.SetString("user_password",register_password.text);
            PlayerPrefs.SetString("user_name",register_name.text);
            PlayerPrefs.SetString("agentCode", agentCode.text);
            
            SignUpTip.text = string.Empty;

            SetScaleToOne(LoginBox);
            SetScaleToZero(SignInBox);
            
            user_Name.text = PlayerPrefs.GetString("user_name");
            user_Name.GetComponentInParent<UIInput>().value = PlayerPrefs.GetString("user_name");
            agentCode.text = PlayerPrefs.GetString("agentCode");
            agentCode.GetComponentInParent<UIInput>().value = PlayerPrefs.GetString("agentCode");

            user_password.value = PlayerPrefs.GetString("user_password");

            register_name.transform.parent.GetComponent<UIInput>().value = "";
            register_password.transform.parent.GetComponent<UIInput>().value= "";
            register_password.text = "";
            register_name.text = "";
        }
        else if(flagOfIndex ==2)
        {
            //重名
            if(Localization.language == "English")
                SignUpTip.text = "Duplication Of Name";
            else if(Localization.language == "简体中文")
                SignUpTip.text = "用户名已被注册";
        }

    }
    void LoginDispose()
    {
        waitBox.SetActive(false);
        //登录成功
        if (flagOfIndex == 0)
        {
            //记住用户名密码
            PlayerPrefs.SetString("user_password", user_password.value);
            PlayerPrefs.SetString("user_name", user_Name.text);
            PlayerPrefs.SetString("agentCode", agentCode.text);
            SetScaleToZero(LoginBox);
            SetCardEnable();
            //取消提示
            LoginTip.gameObject.SetActive(false);

        }
        else if (flagOfIndex == 1)
        {
            //提示出错
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error1";
        }
        else if (flagOfIndex == 2)
        {
            //提示出错
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error2";
        }
        else if (flagOfIndex == 3)
        {
            //提示出错
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error3";
            
        }
        else if (flagOfIndex == 6)
        {
            //提示出错
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error6";
        }
        else
        {
            //提示出错
            LoginTip.gameObject.SetActive(true);
            LoginTip.GetComponent<UILocalize>().key = "Error5";
        }
        LoginTip.text = Localization.Get(LoginTip.GetComponent<UILocalize>().key);
    }
    void DetectLegalUser()
    {
        //
    }
    //设置卡牌可不可点击
    IEnumerator SetCardState(bool state)
    {
//        Debug.Log(state);
        if(Application.loadedLevelName == "Lobby")
        {
            GameObject curObj = GameObject.Find("/FlowRoot");
            BoxCollider[] bcs = curObj.GetComponentsInChildren<BoxCollider>();
            foreach(BoxCollider bc in bcs)
            {
                bc.enabled = state;
            }
        }
        yield return null;
    }
    //later method to login
    //type in 2014 11 25
    /*
    public void playGame()
    {
        StartCoroutine(setObjFalse(waitBox));
        
        string name = PlayerPrefs.GetString("user_name");
        string password = PlayerPrefs.GetString("user_password");
        Debug.Log("name:"+name+";password:"+password);
        GetComponent<NetworkConnect>().Login(name,password);
        GetComponent<NetworkConnect>().LoginWithToken("111111");
        
    }
    */
    //change the user
    public void LoginOut()
    {
        GameObject.Find("UI Root/Camera/Panel/Credit/Name").GetComponent<UILabel>().text = string.Empty;
        GameObject.Find("UI Root/Camera/Panel/Credit/Account").GetComponent<UILabel>().text = string.Empty;
        user_Name.text = string.Empty;
        user_password.value = string.Empty;
        agentCode.text = string.Empty;
        PlayerPrefs.SetString("user_name",string.Empty);
        PlayerPrefs.SetString("user_password",string.Empty);
        PlayerPrefs.SetString("agentCode", string.Empty);
        GetComponent<NetworkConnect>().AutoBrokenNetwork();
        SetScaleToOne(LoginBox);
    }
    public void SetCardEnable()
    {
        if(flagOfIndex ==0)
            StartCoroutine(SetCardState(true));
    }
    public void SetScaleToZero(GameObject obj)
    {
        TweenAlpha.Begin(shadowBackgrond,0.2f,0);
        TweenScale ts = TweenScale.Begin(obj,0.3f,Vector3.zero);
        ts.method = UITweener.Method.EaseOut;        
    }
    public void SetScaleToOne(GameObject obj)
    {
        obj.SetActive(true);
        StartCoroutine(SetCardState(false));
        TweenAlpha.Begin(shadowBackgrond,0.2f,1);
        TweenScale ts = TweenScale.Begin(obj,0.3f,Vector3.one);
        ts.method = UITweener.Method.EaseIn;
    }
    //确定登录
    public void Login()
    {
        StartCoroutine(SureLogin());
    }
    IEnumerator SureLogin()
    {
        waitBox.SetActive(true);
        string name = user_Name.text;
        string password = user_password.value;
        Debug.Log(name + " ; " + password);
        string aCode = agentCode.text;
        GetComponent<NetworkConnect>().GAME_UPDATE_LOGIN(aCode, name, password);
        yield return null;
    }
    //注册
    public void SignUp()
    {
        if(register_password.text==confirm_password.text)
        {
            StartCoroutine(setObjFalse(waitBox));
            string name = register_name.text;
            string password = register_password.text;
            GetComponent<NetworkConnect>().SignIn(name,password);
        }
        else
        {
            if(Localization.language == "English")
                SignUpTip.text = "The two passwords you typed do not match.";
            else if(Localization.language == "简体中文")
                SignUpTip.text = "两次输入的密码不一致";
        }
    }
    //注册
    public void SignUpByOpenWebSite()
    {
        Application.OpenURL(VersionController.signUrl);
    }
    //退出
    public void Quit()
    {
        Debug.Log("Call the Quit Function!");
        GetComponent<NetworkConnect>().ExitGame();
        StartCoroutine(AppliactionQuit_IE());
        
    }
    IEnumerator AppliactionQuit_IE()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
    IEnumerator SetObjectActiveLater(GameObject obj)
    {
        yield return new WaitForSeconds(0.2f);
        obj.SetActive(false);
        
    }
    IEnumerator setObjFalse(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        obj.SetActive(false);
    }
    void OnEnable()
    {
        NetworkConnect.login_Callback += LoginResult;
        NetworkConnect.sign_Callback += SignResult;
        NetworkConnect.login_CallbackByInterface += LoginResultByInterface;
    }
    void OnDisable()
    {
        NetworkConnect.login_Callback -= LoginResult;
        NetworkConnect.sign_Callback -= SignResult;
        NetworkConnect.login_CallbackByInterface -= LoginResultByInterface;

    }
}

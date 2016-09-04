using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void FreeEffect();
public delegate void FreeEndEffect(double freewin);
public delegate void WildEffect();
public delegate void SmallEffect();
public delegate void JackpotEffect(int val,double money);
public delegate void ScatterEffect(int countOfScatter);
public delegate void WinEffect(double money);
public delegate void GameStateDelegate(GameState gs);
public delegate void AutoSpinEndTag(bool value);
public enum GameState
{
    stop =0,
    start = 1
}
[RequireComponent(typeof(LineSelect))]
public class manager : MonoBehaviour
{
    //Constant 触发中奖特效的临界值
    public const float win_threshold = 8f;
    //最多显示中奖线条数
    public const int lines_show_MaxCount = 3;
    //Delegate Event
    public static FreeEffect freeEffect;
    public static FreeEndEffect freeEndEffect;
    public static WildEffect wildEffect;
    public static JackpotEffect jackEffect;
    public static SmallEffect smallEffect;
    public static ScatterEffect scatterEffect;
    public static WinEffect winEffect;
    public static GameStateDelegate gameStateDelegate;
    public static AutoSpinEndTag autoSpinEndTag;
    public GameState gameState = GameState.stop;
    public static bool isDraw = false;
    public static bool FreeStart = false;
    public static bool smallgame = false;
    public static bool autoSpin = false;
    public static bool jackpot = false;
    public static int GAME_ID = 1;
    public static bool startGame;
    //wheel state
    public static bool[] canmove = new bool[5];
    //is recieve data?
    public static bool isRecieveData;
    //while skipLineAnim = true,skip line Animation
    public static bool skipLineAnim;
    //icon of 15
    public static int[] IconNode = new int[16];
    public static double user_Account = 0;
    public static double cur_User_Account = 0;
    public static string user_name = null;
    public static double win = 0;
    public static double freeWin = 0;
    //animation icons
    public static int[] showIconEffectIcon;
    private int formVaule;
    public static double betBaseMoney = 0;
    public static int autoSpinTimess = 0;
    public static float GameSpeed;
    public UILabel credits;
    public UILabel betLabel;
    public UILabel lineLabel;
    public UILabel betMaxLabel;
    public UILabel WinLabel; 
    public UILabel FreeTimeLabel;
    public GameObject alphaDark;

    public AudioClip[] normalPrizeClip;
    stopSelf[] sp;
    //type form double
    public static Dictionary<int,GameDealNode> nodeList = new Dictionary<int, GameDealNode>();
    public static Dictionary<int,GameObject> object15 = new Dictionary<int,GameObject>();
    private NetworkConnect network;

    void Awake()
    {
        skipLineAnim = false;
        isDraw = false;
        isRecieveData = false;
        autoSpinTimess = 0;
        startGame = false;
        credits.text = Conversion.MoneyUnitConver(user_Account);
        cur_User_Account = user_Account;
    }

    void Start()
    {
        network = GameObject.Find("/GameManager").GetComponent<NetworkConnect>();
        sp = transform.GetComponentsInChildren<stopSelf>();
        for (int i = 0; i<5; i++)
            canmove [i] = false;
    }
    //start or stop 
    public void reset()
    {
        StartCoroutine("IEreset");
    }
    IEnumerator IEreset()
    {
        if (autoSpinTimess > 0)
            StartCoroutine("NextTime");
        else
        {
            //一系列的数值变动
            betBaseMoney = double.Parse(betMaxLabel.text);
            user_Account -= betBaseMoney;
            cur_User_Account = user_Account;
            credits.text = Conversion.MoneyUnitConver(user_Account);
            if (!startGame)
            {
                //游戏开始，如果正在播放先动画，则跳过
                gameStateDelegate(GameState.start);
                if (isDraw)
                    skipLineAnim = true;

                startGame = true;
                RunWheelFunction();
                //transition
                int betLine = Conversion.Trangelines(int.Parse(lineLabel.text));
                int betMoney = Conversion.TrangeBet(betLabel.text);
                network.GAME_DEAL(betMoney, betLine);
                // wait~
                float runTime =0; 
                while (true)
                {
                    runTime+=Time.deltaTime;
                    /*
                    if(runTime>30)
                    {   //滚轮持续滚动时间超过15秒，则设为掉线
                        //掉线
                        Debug.Log("超时滚动");
                        network.BrokenNetwork();
                    }
                    */
                    yield return null;
                    if (!IsNowMoving())//有问题----过很久才能下一把
                    {
                        ShowLinemiddle();
                        break;
                    }
                }
            }
        }
    }
    //auto spin method
    IEnumerator NextTime()
    {
        Time.timeScale = 1.0f;
        autoSpin = true;
        autoSpinEndTag(false);
        UILabel autoSpinLabel = GameObject.FindGameObjectWithTag("autoSpinLabel").GetComponent<UILabel>();
        while (autoSpinTimess>0)
        {
            if(!DoTheMoneyEnough())
            {
                autoSpinTimess = -1;
                break;
            }
            //game start
            gameStateDelegate(GameState.start);
            startGame = true;
            //deduction money
            user_Account = user_Account - double.Parse(betMaxLabel.text);
            betBaseMoney = double.Parse(betMaxLabel.text);
            cur_User_Account = user_Account;
            credits.text = Conversion.MoneyUnitConver(user_Account);
            //show auto spin time left
            autoSpinTimess--;
            autoSpinLabel.text = autoSpinTimess.ToString();
            RunWheelFunction();
            //transition
            int betLine = Conversion.Trangelines(int.Parse(lineLabel.text));
            int betMoney = Conversion.TrangeBet(betLabel.text);
            //bet method
            network.GAME_DEAL(betMoney, betLine);

            //wait~
            float runTime = 0;
            while (true)
            {
                /*
                runTime+=Time.deltaTime;
                if(runTime>15)
                {   //滚轮持续滚动时间超过15秒，则设为掉线
                    //掉线
                    network.BrokenNetwork();
                }
                */
                yield return null;
                if (!IsNowMoving())
                {
                    ShowLinemiddle();
                    yield return null;
                    break;
                }
            }
            while (smallgame||jackpot||FreeStart||isDraw)
                yield return new WaitForSeconds(.2f);
            Debug.Log("NEXT HAND");
        }
        //resume button state
        gameStateDelegate(GameState.stop);
        //call delegate， autospin
        autoSpin = false;
        autoSpinEndTag(true);
    }
    public void ShowLinemiddle()
    {
        StartCoroutine("ShowLine");
    }
    //show line effect
    IEnumerator ShowLine()
    {
        isDraw = true;
        Time.timeScale = 1.0f;
        ArrayList arrayLines = new ArrayList();
        ArrayList arrayClips = new ArrayList();
        ArrayList arrayPaids = new ArrayList();
		
        for (int i=0; i<manager.nodeList.Count; i++)
        {
            GameDealNode node;
            manager.nodeList.TryGetValue(i, out node);
            arrayLines.Add(node.TYPE);
            arrayClips.Add(node.FORM);
            arrayPaids.Add(node.PAID);
        }
        for (int i = 0; i<arrayLines.Count; i++)
        {
            //show the win this round if the prize is line
            if ((int)arrayLines[i] > 5 || (int)arrayLines[i] == 3 || (int)arrayLines[i] == 2)
            {
                WinLabel.text = ((double)arrayPaids[i] + double.Parse(WinLabel.text)).ToString();
                //if the current state is freegame...
                if (FreeStart) freeWin += (double)arrayPaids[i];
                else freeWin = 0;
            }
        }
        //in autoSpin or in free spin,your should limit count of show lines.
        int countLimit = (autoSpinTimess > 0||FreeStart) ? (arrayLines.Count > lines_show_MaxCount ? lines_show_MaxCount : arrayLines.Count) : arrayLines.Count;
        if (!FreeStart) AddMoney(double.Parse(WinLabel.text), countLimit);
        //free small or normal
        bool canNextTime = true;
        //遍历线，如果没有中免费游戏或小游戏或jackpot游戏，则按钮亮起，可进行下一把操作
        for (int i = 0; i < countLimit; i++)
            canNextTime = ((int)arrayLines [i] != 5) & ((int)arrayLines [i] != 4)&((int)arrayLines [i] != 1) & canNextTime;
        //该轮不是在免费游戏或自动游戏中，且canNextTime == true,而且中奖金额触发不了特效。
        if (canNextTime && !FreeStart && autoSpinTimess == 0 && double.Parse(WinLabel.text) < win_threshold * betBaseMoney) gameStateDelegate(GameState.stop);
        //if isn't a special prize，judge if the win money is enough to play a winEffect
        if (arrayLines.Count > 0 && (int)arrayLines [0] > 5 && double.Parse(WinLabel.text) >= win_threshold * betBaseMoney)
        {
            winEffect(double.Parse(WinLabel.text));
            yield return new WaitForSeconds(6f);
            if (!FreeStart)
                gameStateDelegate(GameState.stop);
        }
        //开始动画线的绘制
        for (int i = 0; i<countLimit; i++)
        {
            int value = (int)arrayLines [i];
            formVaule = (int)arrayClips [i];
            win = (double)arrayPaids [i];
            //Normal line
            if (value > 5)
            {
                while (IsNowMoving())
                {
                    yield return null;
                    if (skipLineAnim)
                        break;
                }
                if (!skipLineAnim)
                {
                    GetComponent<AudioSource>().PlayOneShot(normalPrizeClip [Random.Range(0, normalPrizeClip.Length - 1)]);
                    StartCoroutine(DrawLine(value - 5, formVaule));
                    for (int j=0; j<8; j++)
                        yield return new WaitForSeconds(0.25f);
                    //delete line
                    DeleteLine(value - 5);
                }
            }
			//Free
			else if (value == 5)
            {
                FreeTimeLabel.text = formVaule.ToString();
                freeEffect();
                FreeStart = true;
                //find specific icon
                foreach (GameObject oo in object15.Values)
                {
                    try
                    {
                        if (oo.name == "FREE")
                            oo.GetComponent<EffectManager>().OpenEffect();
                    } catch (MissingReferenceException me)
                    {
                        Debug.Log(me.ToString());
                    }
					
                }
                //表演动画
                for (int j=0; j<8; j++)
                    yield return new WaitForSeconds(0.6f);
                StartCoroutine("StartFreeGame");
                break;
            }
			//small game
			else if (value == 4)
            {
                smallgame = true;
                smallEffect();
                //traverse  Object15，找出SMALL
                foreach (GameObject oo in object15.Values)
                {
                    if (oo.name == "SMALL")
                        oo.GetComponent<EffectManager>().OpenEffect();
                }
                //wait~
                for (int j=0; j<8; j++)
                    yield return new WaitForSeconds(0.25f);
                //enter small game
                GameObject.FindWithTag("SgCamera").GetComponent<SmallGameChange>().ChangeToSg();
                SmallGame.valueOfTime = formVaule;
                while (true)
                {
                    //wait~
                    yield return null;
                    if (!smallgame)
                    {
                        gameStateDelegate(GameState.stop);
                        break;
                    }
                }
                break;
            }
			//Scatter
			else if (value == 3)
            {
                scatterEffect(formVaule);
                foreach (GameObject oo in object15.Values)
                {
                    if (oo.name == "SCATTER")
                        oo.GetComponent<EffectManager>().OpenEffect();
                }
                //wait~
                for (int j=0; j<12; j++)
                    yield return new WaitForSeconds(0.25f);
                winEffect(double.Parse(WinLabel.text));
                yield return new WaitForSeconds(3f);

                if (!FreeStart) gameStateDelegate(GameState.stop);
            }
            //WILD
			else if (value == 2)
            {
                wildEffect();
                //find specific icon
                foreach (GameObject oo in object15.Values)
                {
                    if (oo.name == "WILD")
                    {
                        oo.GetComponent<EffectManager>().OpenEffect();
                        if (oo.GetComponent<Wild2DEffect>())
                            oo.GetComponent<Wild2DEffect>().ShowWildEffect();
                    }
                }
                //wait~
                for (int j=0; j<12; j++)
                    yield return new WaitForSeconds(0.25f);

                winEffect(double.Parse(WinLabel.text));
                yield return new WaitForSeconds(3f);
                if (!FreeStart) gameStateDelegate(GameState.stop);
            }
			//Jackpot
			else if (value == 1)
            {
                jackpot = true;
                jackEffect(formVaule, win);
                string tempName = (formVaule == 1 ?"MINIJACKPOT":(formVaule == 2 ? "MINORJACKPOT" : "MEGAJACKPOT"));
                //find specific icon
                foreach (GameObject oo in object15.Values)
                {
                    if (oo.name == tempName) oo.GetComponent<EffectManager>().OpenEffect();
                }
                //wait~
                for (int j=0; j<8; j++)
                    yield return new WaitForSeconds(0.25f);
                StartCoroutine(LoadJackpot(formVaule));
                while (true)
                {
                    //wait~
                    yield return null;
                    if (!jackpot)
                    {
                        gameStateDelegate(GameState.stop);
                        break;
                    }
                }
            }
        }
        //End draw line
        WinLabel.text = "0";
        isDraw = false;
        skipLineAnim = false;
        alphaDark.SetActive(false);
    }
    //Free game start function
    IEnumerator StartFreeGame()
    {
        while (isDraw)
            yield return null;
        int tempValue = formVaule;
        while (tempValue>0)
        {
            gameStateDelegate(GameState.start);
            Debug.Log("tempValue:" + tempValue);
            startGame = true;
//          Debug.Log("免费游戏");
            startFree();
            tempValue--;
            //free time left
            FreeTimeLabel.text = tempValue.ToString();
            // wait for wheel stop and play line effect
            float runTime =  0;
            while (true)
            {
                runTime+=Time.deltaTime;
                /*
                if(runTime>15)
                {   //滚轮持续滚动时间超过15秒，则设为掉线
                    //掉线
                    network.BrokenNetwork();
                }
                */
                yield return null;
                if (!IsNowMoving())
                {
                    ShowLinemiddle();
                    yield return null;
                    while (isDraw)
                        yield return null;
                    break;
                }
//                Debug.Log("Wait Wait Wait~");
            }
        }
        freeEndEffect(freeWin);
        //等待播放结算动画
        yield return new WaitForSeconds(2f);
        AddMoney(freeWin, 1);
        yield return new WaitForSeconds(2f);

        FreeStart = false;
        //resume the button state
        if (autoSpinTimess == 0) gameStateDelegate(GameState.stop);
        
    }
    //start free game
    void startFree()
    {
        RunWheelFunction();
        network.Free_Game();
    }
    IEnumerator DrawLine(int type, int icons)
    {
        alphaDark.SetActive(true);
        //draw line.....
        yield return null;
        //play icon effect
        SaveTheAnimPoint(type, icons);
        GameObject.FindGameObjectWithTag("Line").GetComponent<LineShow>().LineShowMethod(type, true);
    }
    void DeleteLine(int type)
    {
        //delete line....
        GameObject.FindGameObjectWithTag("Line").GetComponent<LineShow>().LineShowMethod(type, false);
    }
    //Set Animation
    void SaveTheAnimPoint(int index, int form)
    {
        showIconEffectIcon = Conversion.AnimationConversion(index, form);
        //showIconEffectIcon include the point will animation
        Dictionary<int,GameObject> Icons = object15;
        for (int i = 0; i<showIconEffectIcon.Length; i++)
        {
            foreach (KeyValuePair<int,GameObject> kp in Icons)
            {
                try
                {
                    if (showIconEffectIcon [i] == kp.Key)
                        kp.Value.GetComponent<EffectManager>().OpenEffect();
                } catch (MissingReferenceException me)
                {
                    Debug.LogWarning(me);
                }
            }
        }
    }
    //justify if the wheel state is moving
    public bool IsNowMoving()
    {
        bool isMoving = false;
        for (int i = 0; i<5; i++)
            isMoving = isMoving || canmove [i];
        return isMoving;
    }
    //add win money in account
    public void AddMoney(double money, int lineCount)
    {
        StartCoroutine(AddWinMoneyInToAccount(money, lineCount));
    }
    IEnumerator AddWinMoneyInToAccount(double money, int lineCount)
    {
        double moneyAfterDeduction = money * 0.95;
        float timeCount = Conversion.MoneyTranTime(moneyAfterDeduction);
        if (timeCount > lineCount * 2f)
            timeCount = lineCount * 2f;
        double moneyDelta = (moneyAfterDeduction / timeCount) * 0.02f;
        while (timeCount>0.02f)
        {
            timeCount -= 0.02f;
            yield return new WaitForSeconds(0.02f);
            credits.text = Conversion.MoneyUnitConver((Conversion.MoneyUnitConver(credits.text) + moneyDelta));
            cur_User_Account = cur_User_Account + moneyDelta;
        }
        cur_User_Account = user_Account;
        credits.text = Conversion.MoneyUnitConver(user_Account);
    }
    // Runs the wheel function.
    private void RunWheelFunction()
    {
        foreach (stopSelf ss in sp)
            ss.Isstop = false;
        for (int i = 0; i<5; i++)
            canmove [i] = true;
    }
    //金钱是否足够
    public bool DoTheMoneyEnough()
    {
        return user_Account >= Conversion.MoneyUnitConver(betMaxLabel.text);
    }
    public void BackToMenu()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneManager>().BackToMenu();
    }
    public IEnumerator Simulate()
    {
        if (!startGame)
        {
            gameStateDelegate(GameState.start);
            if (isDraw)
                skipLineAnim = true;

            startGame = true;
            RunWheelFunction();
            while (true)
            {
                yield return null;
                if (!IsNowMoving())
                {
                    ShowLinemiddle();
                    break;
                }
            }
        }
    }
    IEnumerator LoadJackpot(int formValue)
    {
        yield return new WaitForSeconds(3f);
        switch(formValue)
        {
            case 3:Instantiate(Resources.Load("Jackpot/MegaJackpot"),new Vector3(-888,0,0),Quaternion.identity);break;
            case 2:Instantiate(Resources.Load("Jackpot/MinorJackpot"),new Vector3(-888,0,0),Quaternion.identity);break;
            case 1:Instantiate(Resources.Load("Jackpot/MiniJackpot"),new Vector3(-888,0,0),Quaternion.identity);break;
        }
    }
    void OnDisable()
    {
        FreeStart = false;
        autoSpin = false;
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using LitJson;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public delegate void SG_5TURNOVER_Callback(int index, int tag, double prize);
public delegate void SG_Mammom_Callback(int tag, double prize);
public delegate void SG_Egyptian_L_Callback(string str);
public delegate void SG_Roulette12_Callback(int index1, int index2, double prize);
////小游戏_九宫格踩雷
public delegate void SMALL_NINETHUNDER(int tag, double prize);
public delegate void SG_Egyptian_R_Callback(string str, double prize);
public delegate void Login_Callback(int flag);
public delegate void Sign_Callback(int flag);
public delegate void Monopoly_Callback(int diceNum, int offsetMap, double prize);
public delegate void GuessTheSize_Callback1(double basePrize);
public delegate void GuessTheSize_Callback2(int tag);
public delegate void GAME_JACKPOT_MINI(string[] paidInfo);
public delegate void GAME_JACKPOT_MEGA(string[] paidInfo);
public delegate void GAME_JACKPOT_MINOR(string jackpot, string allPaid, int percent);
public delegate void Paytable_Callback(string content);

public delegate void Sg_AutoRequirePrize(double prize);
//更新 -- 游戏大奖金钱
public delegate void JackpotData(double mega, double mini, double minor);
//更新账户余额
public delegate void BalanceRefresh(double balance);
public delegate void SmallTotalPrize(double prize);

public class NetworkConnect : MonoBehaviour
{
    public TCPClient myTcp1;
    //    public TCPClient myTcp2;

    public static SG_5TURNOVER_Callback sg_5Turnover_Callback;
    public static SG_Mammom_Callback sg_Mammom_Callback;
    public static SMALL_NINETHUNDER sg_NineThunder_Callback;
    public static SG_Egyptian_L_Callback sg_Egyptian_L_Callback;
    public static SG_Egyptian_R_Callback sg_Egyptian_R_Callback;
    public static SG_Roulette12_Callback sg_roulette12_Callback;
    public static Monopoly_Callback monopoly_Callback;
    public static GuessTheSize_Callback1 guessTheSize_Callback1;
    public static GuessTheSize_Callback2 guessTheSize_Callback2;
    public static Login_Callback login_Callback;
    public static Login_Callback login_CallbackByInterface;
    public static Sign_Callback sign_Callback;
    public static JackpotData jackpotData;
    public static BalanceRefresh balanceRefresh;
    public static SmallTotalPrize smallTotalPrize;
    public static GAME_JACKPOT_MEGA game_jackpot_mega;
    public static GAME_JACKPOT_MINOR game_jackpot_minor;
    public static GAME_JACKPOT_MINI game_jackpot_mini;

    public static Paytable_Callback paytable_callback;
    public static Sg_AutoRequirePrize sg_AutoRequirePrize;
    bool jackpotRecieve;
    private string preRequire;
    [HideInInspector]
    public float iconLostTime = 1000;
    private float jackpotCallTime = 10;
    [HideInInspector]
    public int logTag = 0;//表示未登录
    public string check_key;
    public string loginServer;
    private int cursor = 0;
    private int sendCursor = 0;
    private string token = string.Empty;
    private string info = string.Empty;
    string debuginfo = string.Empty;
    public const int requestTimeMax = 3;
    public static string sessionId = string.Empty;
    public static int msgId = 0;
    public GameObject brokenNetworkBox;
    void Update()
    {
        iconLostTime += Time.deltaTime;
        jackpotCallTime += Time.deltaTime;
        //几秒就视为断线
        if (iconLostTime > 5 && iconLostTime < 10)
        {
            BrokenNetwork();
        }
    }
    public void SignIn(string name, string password)
    {
        sendCursor = 0;
        int byteLength = name.Length + password.Length + 20;
        byte[] newByte = new byte[byteLength];
        newByte = IntToByteParseMethod(newByte, byteLength, 5000, 0);
        newByte = StringToByteMethod(newByte, name, password);
        myTcp1.Send(newByte);
        Debug.Log("SignIn");
    }
    public void GAME_UPDATE_LOGIN(string agentCode, string name, string password)
    {
        StartCoroutine(GAME_UPDATE_LOGINIE(agentCode, name, password));
    }
    IEnumerator GAME_UPDATE_LOGINIE(string agentCode, string name, string password)
    {
        GET_INFORM_SEND getInfoSend = new GET_INFORM_SEND();
        getInfoSend.check_key = check_key;
        getInfoSend.short_name = agentCode;
        string agentInfo = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(getInfoSend);
            WWW www = new WWW(string.Format("http://{0}/get_inform", loginServer), Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                agentInfo = www.text;
                break;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
        if (agentInfo != string.Empty)
        {
            Debug.Log("Get Success:" + agentInfo);
            GET_INFORM_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GET_INFORM_RECEIVE>(agentInfo);
            int errorCode = msgJsonRecieve.error_code;
            if (errorCode == 0)
            {
                VersionController.agent = msgJsonRecieve.agent;
                VersionController.platform = msgJsonRecieve.platform;
                VersionController.tokenUrl = msgJsonRecieve.token_url;
                VersionController.gameUrl = string.Format("http://{0}/", msgJsonRecieve.game_url);
                VersionController.keyStr = msgJsonRecieve.key;

                LoginByInterface(name, password);
            }
            else
            {
                //代理编码错误=--
                login_Callback(6);
            }
        }
        else
        {
            //断线处理
            login_Callback(5);
        }
    }
    //令牌登录 测试token号码：111111,
    public void LoginWithToken(string token)
    {
        StartCoroutine(LoginWithToken_IE(token));
    }
    IEnumerator LoginWithToken_IE(string token)
    {
        msgId++;
        GAME_HALL_LOGIN_SEND send = new GAME_HALL_LOGIN_SEND();
        send.msgId = msgId;
        send.systemTag = VersionController.systemTag;
        send.platform = VersionController.platform;
        send.userToken = token; //token;
        send.agent = VersionController.agent;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            Debug.Log("SEND:"+ jsonDataPost);
            Debug.Log("服务器：" + VersionController.gameUrl);
            WWW www = new WWW(VersionController.gameUrl + "game_hall_login", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            GAME_HALL_LOGIN_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_HALL_LOGIN_RECEIVE>(resultJson);
            Debug.Log("5001");
            //0_fail 1_succ
            logTag = msgJsonRecieve.errorCode;
            sessionId = msgJsonRecieve.sessionId;
            manager.user_Account = msgJsonRecieve.balance;
            login_Callback(logTag);
            if (logTag == 0)
            {
                JackpotRequest();
                LineBetRequire();
            }


        }
        else
        {
            //断线通知
            login_Callback(5);
        }
    }
    //开放接口登录
    public void LoginByInterface(string name, string password)
    {
        StartCoroutine(LoginByInterface_IE(name, password));
    }
    //开放接口登录
    IEnumerator LoginByInterface_IE(string name, string password)
    {
        LoginWithToken(VersionController.tokenUrl);
        yield break;
        HttpLogin_SEND send = new HttpLogin_SEND();
        send.user_name = name;
        send.user_password = password;
        send.key = VersionController.keyStr;
        string resultJson = string.Empty;
        for (int i = 0; i < 1; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            Debug.Log(jsonDataPost);
            WWW www = new WWW(VersionController.tokenUrl, Encoding.UTF8.GetBytes(jsonDataPost));

            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            HttpLogin_RECIEVE msgJsonRecieve = JsonMapper.ToObject<HttpLogin_RECIEVE>(resultJson);
            int errorCode = msgJsonRecieve.error;
            string token = msgJsonRecieve.token;
            if (errorCode == 0)
                LoginWithToken(token);
            else
                login_Callback(errorCode);
        }
        else
        {
            //断线处理
            login_Callback(5);
        }
    }

    //选择游戏类别
    public void ChooseGameType(int GameID)
    {
        StartCoroutine(ChooseGameType_IE(GameID));
    }
    public IEnumerator ChooseGameType_IE(int GameID)
    {
        msgId++;
        Debug.Log("GameID:------------" + GameID);
        GAME_IN_GAME_SEND send = new GAME_IN_GAME_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        send.gameId = GameID;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_in_game", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            GAME_IN_GAME_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_IN_GAME_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
            int gameId = msgJsonRecieve.gameId;
            GAME_Balance_Refresh();
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //退出房间
    public void ExitRoom()
    {
        StartCoroutine(ExitRoom_IE());
    }
    public IEnumerator ExitRoom_IE()
    {
        msgId++;
        GAME_OUT_GAME_SEND send = new GAME_OUT_GAME_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_out_game", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            GAME_OUT_GAME_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_OUT_GAME_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
            int flag = msgJsonRecieve.flag;
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    public void ExitGame()
    {
        StartCoroutine(ExitGame_IE());
    }
    public IEnumerator ExitGame_IE()
    {
        msgId++;
        GAME_HALL_LOGOUT_SEND send = new GAME_HALL_LOGOUT_SEND();
        send.sessionId = sessionId;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            Debug.Log("Exit Game");
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_hall_logout", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 1) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 1)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            GAME_HALL_LOGOUT_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_HALL_LOGOUT_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
        }
        yield return null;
    }

    //游戏押注请求
    public void GAME_DEAL(int bet, int line)
    {
        StartCoroutine(GAME_DEAL_IE(bet,line));
    }
    public IEnumerator GAME_DEAL_IE(int bet, int line)
    {
        msgId++;
        GAME_SLOT_DEAL_SEND send = new GAME_SLOT_DEAL_SEND();
        GAME_SLOT_DEAL_RECEIVE msgJsonRecieve;
        send.sessionId = sessionId;
        send.msgId = msgId;
        send.bet = bet;
        send.line = line;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            Debug.Log(send.msgId+"-- GAME - DEAL - SEND -- " +Time.realtimeSinceStartup);
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_deal", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if(timeCount<5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log(send.msgId+"-- GAME - DEAL - RECIEVE-- " + Time.realtimeSinceStartup);
            Debug.Log("5004,success-------content----" + resultJson);
            msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_DEAL_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
            if (errorCode == 0)
            {
                iconLostTime = 1000;
                //清除上一期中奖的点
                manager.nodeList.Clear();
                manager.object15.Clear();
                //游戏金额
                manager.user_Account = msgJsonRecieve.balance;
                for (int i = 0; i < msgJsonRecieve.arrCard.Length; i++)
                {
                    manager.IconNode[i] = msgJsonRecieve.arrCard[i];
                }
                manager.IconNode[15] = 16;
                //赢取的数目
                //为gamedealnode赋值，共有16个长度
                for (int i = 0; i < msgJsonRecieve.arrPaid.Length; i++)
                {
                    Debug.Log("type:" + msgJsonRecieve.arrPaid[i].type + ";form:" + msgJsonRecieve.arrPaid[i].form + ";paid:" + msgJsonRecieve.arrPaid[i].paid);
                    GameDealNode node = new GameDealNode(msgJsonRecieve.arrPaid[i].type, msgJsonRecieve.arrPaid[i].form, msgJsonRecieve.arrPaid[i].paid);
                    //保存这一期游戏的中奖的点
                    manager.nodeList.Add(i, node);
                    manager.user_Account = msgJsonRecieve.balance;
                    //将以前的15图标记录消除
                    manager.object15.Clear();
                }
                //滚轮消息接收完成
                manager.isRecieveData = true;
            }else if(errorCode == 201)
            {
                //金额不足提示
                BrokenNetwork(201);
            }else
            {
                BrokenNetwork();
            }
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //小游戏略过
    public void GAME_SMALLPrizeRequire()
    {
        StartCoroutine(GAME_SMALLPrizeRequire_IE());
    }
    public IEnumerator GAME_SMALLPrizeRequire_IE()
    {
        msgId++;
        GAME_SLOT_SMALL_OVER_SEND send = new GAME_SLOT_SMALL_OVER_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_over", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5010 success-------content----" + resultJson);
            GAME_SLOT_SMALL_OVER_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_OVER_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
            double prizeSmall = msgJsonRecieve.prize;
            double balanceSmall = msgJsonRecieve.balance;

            smallTotalPrize(prizeSmall);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //小游戏_5翻牌(中国风、沉默武士)
    public void GAME_SMALL_5TurnOver(int index)
    {
        StartCoroutine(GAME_SMALL_5TurnOver_IE(index));
    }
    public IEnumerator GAME_SMALL_5TurnOver_IE(int index)
    {
        msgId++;
        GAME_SLOT_SMALL_TURNOVER_SEND send = new GAME_SLOT_SMALL_TURNOVER_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        send.index = index;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            Debug.Log("-----game_slot_small_turnover-----");
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_turnover", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5011 success-------content----" + resultJson);
            GAME_SLOT_SMALL_TURNOVER_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_TURNOVER_RECEIVE>(resultJson);
            sg_5Turnover_Callback(msgJsonRecieve.index, msgJsonRecieve.tag, msgJsonRecieve.prize);

        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //翻牌 请求金额 5023
    public void GAME_SMALL_GuessTheSize()
    {
        StartCoroutine(GAME_SMALL_GuessTheSize_IE());
    }
    public IEnumerator GAME_SMALL_GuessTheSize_IE()
    {
        msgId++;
        GAME_SLOT_SMALL_BIGORSMALLPRIZE_SEND send = new GAME_SLOT_SMALL_BIGORSMALLPRIZE_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_bigorsmallprize", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5023 success-------content----" + resultJson);
            GAME_SLOT_SMALL_BIGORSMALLPRIZE_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_BIGORSMALLPRIZE_RECEIVE>(resultJson);
            guessTheSize_Callback1(msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //更新账户余额 5024
    public void GAME_Balance_Refresh()
    {
        StartCoroutine(GAME_Balance_Refresh_IE());
    }
    public IEnumerator GAME_Balance_Refresh_IE()
    {
        msgId++;
        GAME_HALL_BALANCE_SEND send = new GAME_HALL_BALANCE_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_hall_balance", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5024 success-------content----" + resultJson);
            GAME_HALL_BALANCE_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_HALL_BALANCE_RECEIVE>(resultJson);
            try
            {
                manager.user_Account = msgJsonRecieve.balance;
                balanceRefresh(msgJsonRecieve.balance);
            }
            catch (NullReferenceException ne)
            {
                Debug.Log(ne);
            }
            finally
            {
                ;
            }
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //猜 哪一个 0 1
    public void GAME_SMALL_GessTheSize2(int index)
    {
        StartCoroutine(GAME_SMALL_GessTheSize2_IE(index));
    }
    public IEnumerator GAME_SMALL_GessTheSize2_IE(int index)
    {
        msgId++;
        GAME_SLOT_SMALL_BIGORSMALL_SEND send = new GAME_SLOT_SMALL_BIGORSMALL_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        send.tag = index;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_bigorsmall", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5012 success-------content----" + resultJson);
            GAME_SLOT_SMALL_BIGORSMALL_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_BIGORSMALL_RECEIVE>(resultJson);
            guessTheSize_Callback2(msgJsonRecieve.tag);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //5020
    public void GAME_Roulette12()
    {
        StartCoroutine(GAME_Roulette12_IE());
    }
    public IEnumerator GAME_Roulette12_IE()
    {
        msgId++;
        GAME_SLOT_SMALL_TWELVEROULETTE_SEND send = new GAME_SLOT_SMALL_TWELVEROULETTE_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_twelveroulette", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5020 success-------content----" + resultJson);
            GAME_SLOT_SMALL_TWELVEROULETTE_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_TWELVEROULETTE_RECEIVE>(resultJson);
            sg_roulette12_Callback(msgJsonRecieve.index1, msgJsonRecieve.index2, msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    //财神小游戏
    public void GAME_SMALL_Mammom(int index)
    {
        StartCoroutine(GAME_SMALL_Mammom_IE(index));

    }
    public IEnumerator GAME_SMALL_Mammom_IE(int index)
    {
        msgId++;
        GAME_SLOT_SMALL_SERIALTURNOVER_SEND send = new GAME_SLOT_SMALL_SERIALTURNOVER_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_serialturnover", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5014 success-------content----" + resultJson);
            GAME_SLOT_SMALL_SERIALTURNOVER_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_SERIALTURNOVER_RECEIVE>(resultJson);
            sg_Mammom_Callback(msgJsonRecieve.tag, msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;

    }
    //九宫格踩雷 5017
    public void GAME_SMALL_SpiderMan()
    {
        StartCoroutine(GAME_SMALL_SpiderMan_IE());

    }
    public IEnumerator GAME_SMALL_SpiderMan_IE()
    {
        msgId++;
        GAME_SLOT_SMALL_NINETHUNDER_SEND send = new GAME_SLOT_SMALL_NINETHUNDER_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_ninethunder", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5017 success-------content----" + resultJson);
            GAME_SLOT_SMALL_NINETHUNDER_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_NINETHUNDER_RECEIVE>(resultJson);

            sg_NineThunder_Callback(msgJsonRecieve.tag, msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    //埃及小游戏请求 - 双轮翻牌_第一个消息(5018),第二个消息(5019);
    public void GAME_SMALL_EGYPTIAN(int index)
    {
        Debug.Log("Index:"+index);
            if (index == 5018)
            {
                StartCoroutine(GAME_SMALL_EGYPTIAN_IE_5018(index));
            }
            else
            {
                StartCoroutine(GAME_SMALL_EGYPTIAN_IE_5019(index));
            }
    }
    public IEnumerator GAME_SMALL_EGYPTIAN_IE_5018(int index)
    {
        msgId++;
        GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_SEND send = new GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;
        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_doubleturnoverfirst", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5018 success-------content----" + resultJson);
            GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_RECEIVE>(resultJson);

            sg_Egyptian_L_Callback(msgJsonRecieve.card.ToString());
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    public IEnumerator GAME_SMALL_EGYPTIAN_IE_5019(int index)
    {

        msgId++;
        GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_SEND send = new GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_doubleturnoversecond", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5019 success-------content----" + resultJson);
            GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_RECEIVE>(resultJson);

            sg_Egyptian_R_Callback(msgJsonRecieve.card.ToString(), msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    //jackpot请求
    public void JackpotRequest()
    {
        StartCoroutine(JackpotRequest_IE());
    }
    public IEnumerator JackpotRequest_IE()
    {
        msgId++;
        GAME_JACKPOT_POOL_SEND send = new GAME_JACKPOT_POOL_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;
        Debug.Log("SEND:jackpot");
        while (true)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_jackpot_pool", Encoding.UTF8.GetBytes(jsonDataPost));
            while (!www.isDone) yield return null;
            yield return www;
            if (www.error == null)
            {
                resultJson = www.text;
            }
            if (resultJson != string.Empty)
            {
                Debug.Log("5200 success-------content----" + resultJson);
                GAME_JACKPOT_POOL_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_JACKPOT_POOL_RECEIVE>(resultJson);
                try
                {
                    if (msgJsonRecieve.errorCode == 0)
                        jackpotData(msgJsonRecieve.megaBalance, msgJsonRecieve.miniBalance, msgJsonRecieve.minorBalance);
                }
                catch (NullReferenceException ne)
                {
                    Debug.Log(ne);
                }
                finally
                {
                    ;
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }
    //小游戏-大富翁
    public void GAME_SMALL_Monopoly()
    {
        StartCoroutine(GAME_SMALL_Monopoly_IE());
    }
    public IEnumerator GAME_SMALL_Monopoly_IE()
    {
        msgId++;
        GAME_SLOT_SMALL_ZILLIONATIRE_SEND send = new GAME_SLOT_SMALL_ZILLIONATIRE_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_small_zillionaire", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5013 success-------content----" + resultJson);
            GAME_SLOT_SMALL_ZILLIONATIRE_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_SMALL_ZILLIONATIRE_RECEIVE>(resultJson);

            monopoly_Callback(msgJsonRecieve.diceNum, msgJsonRecieve.offsetMap, msgJsonRecieve.prize);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    // Jackpot 有欧系
    public void GAME_JACKPOT(int jackpotID)//1、2、3
    {
        if (jackpotID == 5202)
            StartCoroutine(GAME_JACKPOTMini_IE());
        else if (jackpotID == 5203)
            StartCoroutine(GAME_JACKPOTMinor_IE());
        else if (jackpotID == 5201)
            StartCoroutine(GAME_JACKPOTMega_IE());
    }
    public IEnumerator GAME_JACKPOTMini_IE()
    {
        msgId++;
        GAME_JACKPOT_MINI_SEND send = new GAME_JACKPOT_MINI_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_jackpot_mini", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5202 success-------content----" + resultJson);
            GAME_JACKPOT_MINI_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_JACKPOT_MINI_RECEIVE>(resultJson);

            string[] minorStrs = new string[5] { msgJsonRecieve.jackpot, msgJsonRecieve.allPaid, msgJsonRecieve.paid1, msgJsonRecieve.paid2, msgJsonRecieve.paid3 };
            game_jackpot_mini(minorStrs);
            GAME_Balance_Refresh();
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    public IEnumerator GAME_JACKPOTMinor_IE()
    {
        msgId++;
        GAME_JACKPOT_MINOR_SEND send = new GAME_JACKPOT_MINOR_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_jackpot_minor", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5203 success-------content----" + resultJson);
            GAME_JACKPOT_MINOR_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_JACKPOT_MINOR_RECEIVE>(resultJson);

            game_jackpot_minor(msgJsonRecieve.jackpot, msgJsonRecieve.allPaid, msgJsonRecieve.percent);
            GAME_Balance_Refresh();
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    public IEnumerator GAME_JACKPOTMega_IE()
    {
        msgId++;
        GAME_JACKPOT_MEGA_SEND send = new GAME_JACKPOT_MEGA_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_jackpot_mega", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5201 success-------content----" + resultJson);
            GAME_JACKPOT_MEGA_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_JACKPOT_MEGA_RECEIVE>(resultJson);


            string[] megaStrs = new string[7] { msgJsonRecieve.jackpot, msgJsonRecieve.allPaid, msgJsonRecieve.paid1, msgJsonRecieve.paid2, msgJsonRecieve.paid3, msgJsonRecieve.paid4, msgJsonRecieve.paid5 };
            game_jackpot_mega(megaStrs);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }


    //免费游戏请求
    public void Free_Game()
    {
        StartCoroutine(Free_Game_IE());
    }
    public IEnumerator Free_Game_IE()
    {
        msgId++;
        GAME_SLOT_FREEDEAL_SEND send = new GAME_SLOT_FREEDEAL_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        string resultJson = string.Empty;

        for (int i = 0; i < requestTimeMax; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            WWW www = new WWW(VersionController.gameUrl + "game_slot_freedeal", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
             }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("5005,success-------content----" + resultJson);
            GAME_SLOT_FREEDEAL_RECEIVE msgJsonRecieve = JsonMapper.ToObject<GAME_SLOT_FREEDEAL_RECEIVE>(resultJson);
            int errorCode = msgJsonRecieve.errorCode;
            if (errorCode == 0)
            {
                iconLostTime = 1000;
                //清除上一期中奖的点
                manager.nodeList.Clear();
                manager.object15.Clear();
                //游戏金额
                manager.user_Account = msgJsonRecieve.balance;
                for (int i = 0; i < msgJsonRecieve.arrCard.Length; i++)
                {
                    manager.IconNode[i] = msgJsonRecieve.arrCard[i];
                }
                //这个数为多出来的点，可忽略
                manager.IconNode[15] = 16;
                //赢取的数目
                //为gamedealnode赋值，共有16个长度
                for (int i = 0; i < msgJsonRecieve.arrPaid.Length; i++)
                {
                    Debug.Log("type:" + msgJsonRecieve.arrPaid[i].type + ";form:" + msgJsonRecieve.arrPaid[i].form + ";paid:" + msgJsonRecieve.arrPaid[i].paid);
                    GameDealNode node = new GameDealNode(msgJsonRecieve.arrPaid[i].type, msgJsonRecieve.arrPaid[i].form, msgJsonRecieve.arrPaid[i].paid);
                    //保存这一期游戏的中奖的点
                    manager.nodeList.Add(i, node);
                    //将以前的15图标记录消除
                    manager.object15.Clear();
                }
                //滚轮消息接收完成
                manager.isRecieveData = true;
            }
            else
            {
                BrokenNetwork();
            }
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
    }
    //paytable请求
    public void PayTableRequest()
    {
        StartCoroutine(PayTableRequest_IE());
    }

    public IEnumerator PayTableRequest_IE()
    {
        msgId++;
        CONFIG_GET_PAID_SEND send = new CONFIG_GET_PAID_SEND();
        send.sessionId = sessionId;
        send.msgId = msgId;
        send.gameId = manager.GAME_ID;
        string resultJson = string.Empty;
        Debug.Log("发出消息 config_get_paid----");
        float time = Time.realtimeSinceStartup;
        for (int i = 0; i < requestTimeMax; i++)
        {

            string jsonDataPost = JsonMapper.ToJson(send);

            Debug.Log("5301:" + jsonDataPost);

            WWW www = new WWW(VersionController.gameUrl + "config_get_paid", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        Debug.Log("收到消息，时间为："+ (Time.realtimeSinceStartup-time));
        if (resultJson != string.Empty)
        {
            Debug.Log("5301,success-------content----" + resultJson);
            CONFIG_GET_PAID_RECEIVE msgJsonRecieve = JsonMapper.ToObject<CONFIG_GET_PAID_RECEIVE>(resultJson);

            paytable_callback(msgJsonRecieve.configData);
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    public void LineBetRequire()
    {
        StartCoroutine(LineBetRequire_IE());
    }
    IEnumerator LineBetRequire_IE()
    {
        msgId++;
        BetMsg_SEND send = new BetMsg_SEND();
        send.sessionId = sessionId;
        string resultJson = string.Empty;
        Debug.Log("发出消息 config_get_paid----");
        float time = Time.realtimeSinceStartup;
        for (int i = 0; i < requestTimeMax; i++)
        {

            string jsonDataPost = JsonMapper.ToJson(send);

            Debug.Log("？？？:" + jsonDataPost);

            WWW www = new WWW(VersionController.gameUrl + "config_get_slotlevel", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }
            if (timeCount < 5)
            {
                yield return new WaitForSeconds(5 - timeCount);
            }
        }
        Debug.Log("收到消息，时间为：" + (Time.realtimeSinceStartup - time));
        if (resultJson != string.Empty)
        {
            Debug.Log("5301,success-------content----" + resultJson);
            BetMsg_RECEIVE msgJsonRecieve = JsonMapper.ToObject<BetMsg_RECEIVE>(resultJson);

            for (int i = 0; i < msgJsonRecieve.betLevel.Length; i++)
            {
                if (msgJsonRecieve.betLevel[i].betType == 1)
                {
                    LinesBetManager.MoneyCountDelta25[i%8] = double.Parse(msgJsonRecieve.betLevel[i].bet);
                }
                else if (msgJsonRecieve.betLevel[i].betType == 2)
                {
                    LinesBetManager.MoneyCountDelta50[i%8] = double.Parse(msgJsonRecieve.betLevel[i].bet);
                }
                else if (msgJsonRecieve.betLevel[i].betType == 3)
                {
                    LinesBetManager.MoneyCountDelta100[i%16] = double.Parse(msgJsonRecieve.betLevel[i].bet);
                }
                else if (msgJsonRecieve.betLevel[i].betType == 4)
                {
                    LinesBetManager.MoneyCountDelta15[i%24] = double.Parse(msgJsonRecieve.betLevel[i].bet);
                }
            }
        }
        else
        {
            //断线处理
            BrokenNetwork();
        }
        yield return null;
    }
    //游戏数据模拟
    public void Simulate(string data)
    {
        sendCursor = 0;
        byte[] newByte = new byte[66];
        newByte = IntToByteParseMethod(newByte, 66, 3011, 0);
        newByte = StringToByteMethod(newByte, data);
        myTcp1.Send(newByte);
    }
    public byte[] IntToByteParseMethod(byte[] newByte, params int[] content)
    {
        byte[] innerByte = newByte;
        for (int i = 0; i < content.Length; i++)
        {
            byte[] bs = System.BitConverter.GetBytes(content[i]);
            Array.Reverse(bs); ;
            bs.CopyTo(innerByte, sendCursor);
            sendCursor += 4;
        }
        return innerByte;
    }
    public byte[] StringToByteMethod(byte[] newByte, params string[] content)
    {
        byte[] innerByte = newByte;
        for (int i = 0; i < content.Length; i++)
        {
            byte[] bs0 = System.BitConverter.GetBytes(content[i].Length);
            Array.Reverse(bs0);
            bs0.CopyTo(innerByte, sendCursor);
            sendCursor += 4;

            byte[] bs1 = System.Text.Encoding.ASCII.GetBytes(content[i]);
            bs1.CopyTo(innerByte, sendCursor);
            sendCursor += content[i].Length;

        }
        return innerByte;
    }
    public byte[] LongToByteParseMethod(byte[] newByte, params long[] content)
    {
        byte[] innerByte = newByte;
        for (int i = 0; i < content.Length; i++)
        {
            byte[] bs = System.BitConverter.GetBytes(content[i]);
            Array.Reverse(bs);
            bs.CopyTo(innerByte, sendCursor);
            sendCursor += 8;
        }
        return innerByte;
    }
    public byte[] DoubleToByteParseMethod(byte[] newByte, params double[] content)
    {
        byte[] innerByte = newByte;
        for (int i = 0; i < content.Length; i++)
        {
            byte[] bs = BitConverter.GetBytes(content[i]);
            Array.Reverse(bs);
            bs.CopyTo(innerByte, sendCursor);
            sendCursor += 8;
        }
        return innerByte;
    }
    public byte[] ByteToByteParseMethod(byte[] newByte, params byte[] content)
    {
        byte[] innerByte = newByte;
        content.CopyTo(innerByte, sendCursor);
        sendCursor += content.Length;
        return innerByte;
    }

    //收取数据回调函数
    void OnMyInceptEvent(byte[] getByte)
    {
        //ReciveOfMessage(getByte);
        //       Debug.Log("recieve data^^^");
    }
    //处理收取de数据
    #region 旧消息处理方式
        /*
    void ReciveOfMessage(byte[] reciveBytes)
    {

        float tempTime = normalBrokenTime;
        normalBrokenTime = 1000;
        //初始化游标位置
        cursor = 0;
        //获取游戏的接收的类型
        int MsgType = IntParseMethod(reciveBytes);
        int tempInt = 0;
        //此间略过4个字节
        cursor = 8;
        switch (MsgType)
        {
            //注册
            case 5000:
                Debug.Log("5000");
                //0_fail 1_succ 2_duplicate name
                //        tempInt = IntParseMethod(reciveBytes);
                int flag = IntParseMethod(reciveBytes);
                string token = StringParseMethod(reciveBytes);
                Debug.Log("flag:" + flag);
                Debug.Log("token:" + token);
                if (flag == 1)
                    LoginWithToken(token);
                //        sign_Callback(tempInt);
                break;
            //登录游戏
            case 5001:
                Debug.Log("5001");
                //0_fail 1_succ
                logTag = IntParseMethod(reciveBytes);
                //manager.user_name = StringParseMethod(reciveBytes);
                string sessionId = StringParseMethod(reciveBytes);
                manager.user_Account = DoubleParseMethod(reciveBytes);
                Debug.Log(logTag);
                login_Callback(logTag);
                if (logTag > 0)
                    JackpotRequest();
                break;
            //选择游戏
            case 5002:
                Debug.Log("5002");
                // s-c    gameId  int     0 表示进入失败 >0表示进入成功
                int valueOfFlag2 = IntParseMethod(reciveBytes);
                if (valueOfFlag2 == 0)
                {
                    Debug.LogError("Fail in room");
                }
                break;
            //选择游戏
            case 5003:
                Debug.Log("5003");
                //默认选择中国风游戏
                break;
            //游戏投注
            case 5004:
                Debug.Log("5004");
                //                NGUIDebug.Log("Response 5004");

                iconLostTime = 1000;
                //清除上一期中奖的点
                manager.nodeList.Clear();
                manager.object15.Clear();
                //游戏金额
                manager.user_Account = DoubleParseMethod(reciveBytes);
                //游戏停止转动时15张卡牌点对应的图标号（int）
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                for (int tagNum = 0; tagNum < 15; tagNum++)
                {
                    dictionary.Add(tagNum, IntParseMethod(reciveBytes));
                }
                int iii = 0;
                foreach (KeyValuePair<int, int> dict in dictionary)
                {
                    //Debug.Log("key="+dict.Key+";value="+dict.Value);
                    //保存
                    manager.IconNode[iii] = dict.Value;
                    iii++;
                }
                //这个数为多出来的点，可忽略
                manager.IconNode[15] = 16;
                //赢取的数目
                int wonCount = IntParseMethod(reciveBytes);
                //为gamedealnode赋值，共有16个长度
                for (int i = 0; i < wonCount; i++)
                {
                    int type = 0;
                    int form = 0;
                    double paid = 0;
                    //赋值
                    type = IntParseMethod(reciveBytes);
                    form = IntParseMethod(reciveBytes);
                    paid = DoubleParseMethod(reciveBytes);

                    Debug.Log("type:" + type + ";form:" + form + ";paid:" + paid);
                    GameDealNode node = new GameDealNode(type, form, paid);
                    //保存这一期游戏的中奖的点
                    manager.nodeList.Add(i, node);
                    //将以前的15图标记录消除
                    manager.object15.Clear();
                }
                //滚轮消息接收完成
                manager.isRecieveData = true;
                break;
            case 5010:
                Debug.Log("5010");
                double prizeSmall = DoubleParseMethod(reciveBytes);
                double balanceSmall = DoubleParseMethod(reciveBytes);
                Debug.Log("prize:" + prizeSmall);
                Debug.Log("balance:" + balanceSmall);
                smallTotalPrize(prizeSmall);
                //smallTotalPrize(prizeSmall);
                break;
            //小游戏请求
            case 5011:
                Debug.Log("5011");
                int index = IntParseMethod(reciveBytes);
                int tag = IntParseMethod(reciveBytes);
                double prize = DoubleParseMethod(reciveBytes);
                sg_5Turnover_Callback(index, tag, prize);
                break;
            case 5012:
                Debug.Log("5012");
                int tagAL = IntParseMethod(reciveBytes);
                guessTheSize_Callback2(tagAL);
                break;
            case 5013:
                Debug.Log("5013");
                int diceNum = IntParseMethod(reciveBytes);
                int offsetMap = IntParseMethod(reciveBytes);
                double prize40 = DoubleParseMethod(reciveBytes);
                monopoly_Callback(diceNum, offsetMap, prize40);
                break;
            case 5014:
                Debug.Log("5014");
                int tag2 = IntParseMethod(reciveBytes);
                double prize2 = DoubleParseMethod(reciveBytes);
                sg_Mammom_Callback(tag2, prize2);
                break;
            case 5017:
                Debug.Log("5017");
                int tag3 = IntParseMethod(reciveBytes);
                double prize3 = DoubleParseMethod(reciveBytes);
                sg_NineThunder_Callback(tag3, prize3);

                break;
            case 5018:
                Debug.Log("5018");
                sg_Egyptian_L_Callback(CharParseMethod(reciveBytes).ToString());
                break;
            case 5019:
                Debug.Log("5019");
                byte indexOfRight = CharParseMethod(reciveBytes);
                double prizecz = DoubleParseMethod(reciveBytes);
                sg_Egyptian_R_Callback(indexOfRight.ToString(), prizecz);
                break;
            case 5020:
                Debug.Log("5020");
                int index1 = IntParseMethod(reciveBytes);
                int index2 = IntParseMethod(reciveBytes);
                double prize4 = DoubleParseMethod(reciveBytes);
                sg_roulette12_Callback(index1, index2, prize4);
                break;
            case 5023:
                Debug.Log("5023");
                double prizeAL = DoubleParseMethod(reciveBytes);
                guessTheSize_Callback1(prizeAL);
                break;
            case 5024:
                Debug.Log("5024");
                double balance = DoubleParseMethod(reciveBytes);
                try
                {
                    balanceRefresh(balance);
                }
                catch (NullReferenceException ne)
                {
                    Debug.Log(ne);
                }
                finally
                {
                    ;
                }
                break;
            case 5031:
                Debug.Log("5031");
                int errorId = IntParseMethod(reciveBytes);
                string description = StringParseMethod(reciveBytes);
                //设置断线
                normalBrokenTime = 12;
                Debug.LogError("错误信息:" + description);
                break;
            case 1001:
                Debug.Log("1001");
                int loginMsg = IntParseMethod(reciveBytes);
                jackpotRecieve = true;
                Debug.Log("loginMsg:" + loginMsg);
                break;
            case 5200:
                Debug.Log("5200");
                normalBrokenTime = tempTime;
                double megaBalance = DoubleParseMethod(reciveBytes);
                double miniBalance = DoubleParseMethod(reciveBytes);
                double minorBalance = DoubleParseMethod(reciveBytes);
                //Debug.Log ("Mega:" + megaBalance + ";Mini:" + miniBalance + ";Minor:" + minorBalance);
                try
                {
                    jackpotData(megaBalance, miniBalance, minorBalance);
                }
                catch (NullReferenceException ne)
                {
                    Debug.Log(ne);
                }
                finally
                {
                    ;
                }
                //重新计时
                jackpotCallTime = 0;
                break;
            case 5201:
                Debug.Log("5201");
                //奖池总金额字段长度
                string jackpotAll = StringParseMethod(reciveBytes);
                Debug.Log("jackpotAll:" + jackpotAll);
                //获奖总金额
                string allPaid = StringParseMethod(reciveBytes);
                Debug.Log("allPaid:" + allPaid);
                //Paid1
                string paid1 = StringParseMethod(reciveBytes);
                Debug.Log("paid1:" + paid1);
                //Paid2
                string paid2 = StringParseMethod(reciveBytes);
                Debug.Log("paid2:" + paid2);
                //Paid3
                string paid3 = StringParseMethod(reciveBytes);
                Debug.Log("paid3:" + paid3);
                //Paid4
                string paid4 = StringParseMethod(reciveBytes);
                Debug.Log("paid4:" + paid4);
                //Paid5
                string paid5 = StringParseMethod(reciveBytes);
                Debug.Log("paid5:" + paid5);
                string[] megaStrs = new string[7] { jackpotAll, allPaid, paid1, paid2, paid3, paid4, paid5 };
                game_jackpot_mega(megaStrs);
                break;
            case 5202:
                //奖池总金额字段长度
                string jackpotAll2 = StringParseMethod(reciveBytes);
                Debug.Log("jackpotAll:" + jackpotAll2);
                //获奖总金额
                string allPaid2 = StringParseMethod(reciveBytes);
                Debug.Log("allPaid:" + allPaid2);
                //Paid1
                string paid12 = StringParseMethod(reciveBytes);
                Debug.Log("paid1:" + paid12);
                //Paid2
                string paid22 = StringParseMethod(reciveBytes);
                Debug.Log("paid2:" + paid22);
                //Paid3
                string paid32 = StringParseMethod(reciveBytes);
                Debug.Log("paid3:" + paid32);
                string[] minorStrs = new string[5] { jackpotAll2, allPaid2, paid12, paid22, paid32 };
                game_jackpot_mini(minorStrs);
                break;
            case 5203:
                Debug.Log("5203");
                string jackpotAll3 = StringParseMethod(reciveBytes);
                Debug.Log("jackpotAll:" + jackpotAll3);
                //获奖总金额
                string allPaid3 = StringParseMethod(reciveBytes);
                Debug.Log("allPaid:" + allPaid3);
                int percent = IntParseMethod(reciveBytes);
                game_jackpot_minor(jackpotAll3, allPaid3, percent);
                break;
            case 5301:
                Debug.Log("5301");
                //               NGUIDebug.Log("Request Paytable Success!");
                int gameID = IntParseMethod(reciveBytes);
                int gameType = IntParseMethod(reciveBytes);
                int gameChildType = IntParseMethod(reciveBytes);
                string configData = StringParseMethod(reciveBytes);

                paytable_callback(configData);
                break;

            default:
                break;
        }
    }
    */
    #endregion

    public byte CharParseMethod(byte[] reciveBytes)
    {
        byte[] tempChar = new byte[1];
        System.Array.Copy(reciveBytes, cursor, tempChar, 0, 1);
        cursor += 1;
        return tempChar[0];
    }
    public int IntParseMethod(byte[] reciveBytes)
    {
        byte[] tempBytes = new byte[4];
        System.Array.Copy(reciveBytes, cursor, tempBytes, 0, 4);
        cursor += 4;
        string typeStr = "";
        int tempInt = 0;
        foreach (byte bt in tempBytes)
        {
            typeStr = bt.ToString();
            tempInt = tempInt * 256 + int.Parse(typeStr);
        }
        return tempInt;
    }
    public double DoubleParseMethod(byte[] reciveBytes)
    {
        byte[] tempBytes = new byte[8];
        System.Array.Copy(reciveBytes, cursor, tempBytes, 0, 8);
        cursor += 8;
        Array.Reverse(tempBytes);
        return System.BitConverter.ToDouble(tempBytes, 0);
    }
    public string StringParseMethod(byte[] reciveBytes)
    {
        byte[] tempByte = new byte[IntParseMethod(reciveBytes)];
        System.Array.Copy(reciveBytes, cursor, tempByte, 0, tempByte.Length);
        cursor += tempByte.Length;
        string tempString = System.Text.Encoding.ASCII.GetString(tempByte);
        return tempString;
    }
    public long LongParseMethod(byte[] recieveBytes)
    {
        byte[] tempBytes = new byte[8];
        System.Array.Copy(recieveBytes, cursor, tempBytes, 0, 8);
        cursor += 8;
        Array.Reverse(tempBytes);
        return System.BitConverter.ToInt64(tempBytes, 0);
    }
    //Jackpot收取数据回调函数
    void OnMyInceptJackpotEvent(byte[] getByte)
    {
        //ReciveOfMessage(getByte);
        Debug.Log("jackpot msg recieve~");
    }

    #region  测试游戏连接代码 out
    IEnumerator ReConnect()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (!isConnect())
            {
                //                Debug.Log("re Connect");
                myTcp1.Closed();
                myTcp1.SocketConnect();
            }
        }
    }

    public bool isConnect()
    {
        if (myTcp1 == null)
        {
            return false;
        }
        else
        {
            if (myTcp1.clientSocket == null)
            {
                return false;
            }
            else
            {
                return myTcp1.clientSocket.Connected;
            }
        }
    }
    public void AutoBrokenNetwork()
    {
    }
    void Connect()
    {
        myTcp1.SocketConnect();
        if (isConnect())
        {
            CancelInvoke("Connect");
        }
    }
    //如果向服务器发送消息5秒钟后没收到回发消息就判断为断网，用户需要重新登录
    public void BrokenNetwork(int errorCode = 0)
    {
        StartCoroutine(BrokenNetwork_IE(errorCode));
    }
    public IEnumerator BrokenNetwork_IE(int errorCode = 0)
    {
        TweenScale.Begin(brokenNetworkBox, 0.2f, Vector3.one);
        Debug.Log("errorCode:" + errorCode);
        if (errorCode == 201)
        {
            brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().key = "Error201";
            if (Localization.language == "English")
                brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "Insufficient Balance!";
            else
                brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "余额不足！";
        }else
        {
            brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().key = "Lobby_LoseConnectTip";
            if (Localization.language == "English")
                brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "LOST CONNECTION!!!";
            else
                brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "断开连接！！！";

        }
        yield return new WaitForSeconds(1.5f);

        brokenNetworkBox.transform.localScale = Vector3.zero;
        ExitGame();
        if (SceneManager.curSceneStr != "Lobby")
        {
            //显示您已断网，然后回到主界面
            SceneManager.loseConnection = true;
            StartCoroutine(BackMenu());
        }
    }
    public IEnumerator ShowInsufficientBalanceTip()
    {
        brokenNetworkBox.transform.localScale = Vector3.one;
        brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().key = "Error201";
        if (Localization.language == "English")
            brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "Insufficient Balance!";
        else
            brokenNetworkBox.transform.FindChild("text").GetComponent<UILocalize>().value = "余额不足！";
        yield return new WaitForSeconds(1f);

        brokenNetworkBox.transform.localScale = Vector3.zero;


    }

    IEnumerator BackMenu()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneManager>().BackToMenu();
        yield return null;
    }
    #endregion
    /*
    void OnGUI()
    {
        GUI.TextField(new Rect(Screen.width-500,0,500,20),"Socket Connect State:"+myTcp1.clientSocket.Connected);
        GUI.TextField(new Rect(Screen.width-500,20,500,20),"Read Size:"+myTcp1.charSize);
        
    }

    /*
    void ConnectServer()
    {
      //  myTcp1 = new TCPClient("58.96.187.33",14001);
      //  myTcp1 = new TCPClient("192.168.1.199",15001);
    //    myTcp1 = new TCPClient("192.168.1.195",15001);
        myTcp1 = new TCPClient("203.88.171.232",14001);
        myTcp1.recievet += OnMyInceptEvent;
        /*
        myTcp2 = new TCPClient("192.168.1.199",15011);
        myTcp2.recievet += OnMyInceptJackpotEvent;

    }

    */

    //Http Post
}
public class MessageJson
{
    public int intValue;
    public long longValue;
    public string stringValue;
    public byte byteValue;
    public double doubleValue;
    public int[] arrCard;
    public ArrNode[] arrNode;

    public class ArrNode
    {
        public int type;
        public int form;
        public double paid;
    }
}
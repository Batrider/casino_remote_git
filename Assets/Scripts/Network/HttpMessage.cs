using UnityEngine;
using System.Collections;

public class HttpMessages
{

}
public class HttpLogin_SEND:HttpMessages
{
    public string user_name;
    public string user_password;
    public string key;
}
public class HttpLogin_RECIEVE:HttpMessages
{
    public string token;
    public int error;
    public string description;
}
/// <summary>
/// 5000 获取Token
/// </summary>
public class GAME_GET_TOKEN_SEND : HttpMessages
{
    public string userName;
    public string userPwd;
    public string key;
}
public class GAME_GET_TOKEN_RECEIVE : HttpMessages
{
    public string error;
    public string token;
    public string description;
}
/// <summary>
/// 5001 登录游戏
/// </summary>
public class GAME_HALL_LOGIN_SEND : HttpMessages
{
    public int msgId;
    public string systemTag;
    public string platform;
    public string userToken;
    public string agent;
}
public class GAME_HALL_LOGIN_RECEIVE : HttpMessages
{
    public int errorCode;
    public string sessionId;
    public double balance;
}
public class GAME_HALL_LOGOUT_SEND:HttpMessages
{
    public string sessionId;
}
public class GAME_HALL_LOGOUT_RECEIVE : HttpMessages
{
    public int errorCode;
}
/// <summary>
/// 5002 进入游戏
/// </summary>
public class GAME_IN_GAME_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
    public int gameId;
}
public class GAME_IN_GAME_RECEIVE : HttpMessages
{
    public int errorCode;
    public int gameId;
}
/// <summary>
/// 5003 退出账户
/// </summary>
public class GAME_OUT_GAME_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_OUT_GAME_RECEIVE : HttpMessages
{
    public int errorCode;
    public int flag;
}
/// <summary>
/// 5004 游戏投注
/// </summary>
public class GAME_SLOT_DEAL_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
    public int bet;
    public int line;
}
public class GAME_SLOT_DEAL_RECEIVE : HttpMessages
{
    public int errorCode;
    public double balance;
    public int[] arrCard;
    public ArrPaid[] arrPaid;
    public class ArrPaid
    {
        public int type;
        public int form;
        public double paid;
    }
}
/// <summary>
/// 5005 免费游戏
/// </summary>
public class GAME_SLOT_FREEDEAL_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_FREEDEAL_RECEIVE : HttpMessages
{
    public int errorCode;
    public double balance;
    public int[] arrCard;
    public GAME_SLOT_DEAL_RECEIVE.ArrPaid[] arrPaid;
}
/// <summary>
/// 5010 小游戏立马结束消息
/// </summary>
public class GAME_SLOT_SMALL_OVER_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_OVER_RECEIVE : HttpMessages
{
    public int errorCode;
    public double prize;
    public double balance;
}
/// <summary>
/// 5011 小游戏——翻牌
/// </summary>
public class GAME_SLOT_SMALL_TURNOVER_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
    public int index;
}
public class GAME_SLOT_SMALL_TURNOVER_RECEIVE : HttpMessages
{
    public int errorCode;
    public int index;
    public int tag;
    public double prize;
}
/// <summary>
/// 5012 小游戏——猜大小
/// </summary>
public class GAME_SLOT_SMALL_BIGORSMALL_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
    public int tag;
}
public class GAME_SLOT_SMALL_BIGORSMALL_RECEIVE : HttpMessages
{
    public int errorCode;
    public int tag;
    public double prize;
}

/// <summary>
/// 5013 小游戏——大富翁
/// </summary>
public class GAME_SLOT_SMALL_ZILLIONATIRE_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_ZILLIONATIRE_RECEIVE : HttpMessages
{
    public int errorCode;
    public int diceNum;
    public int offsetMap;
    public double prize;
}
/// <summary>
/// 5014 小游戏——连环翻牌
/// </summary>
public class GAME_SLOT_SMALL_SERIALTURNOVER_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_SERIALTURNOVER_RECEIVE : HttpMessages
{
    public int errorCode;
    public int tag;
    public double prize;
}
/// <summary>
/// 5017 小游戏——九宫格踩雷
/// </summary>
public class GAME_SLOT_SMALL_NINETHUNDER_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_NINETHUNDER_RECEIVE : HttpMessages
{
    public int errorCode;
    public int tag;
    public double prize;
}
/// <summary>
/// 5018 小游戏——双轮翻牌 1
/// </summary>
public class GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_DOUBLETURNOVERFIRST_RECEIVE : HttpMessages
{
    public int errorCode;
    public byte card;
}
/// <summary>
/// 5019 小游戏——双轮翻牌 2
/// </summary>
public class GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_DOUBLETURNOVERSECOND_RECEIVE : HttpMessages
{
    public int errorCode;
    public byte card;
    public double prize;
}
/// <summary>
/// 5020 小游戏——十二轮盘
/// </summary>
public class GAME_SLOT_SMALL_TWELVEROULETTE_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_TWELVEROULETTE_RECEIVE : HttpMessages
{
    public int errorCode;
    public int index1;
    public int index2;
    public double prize;
}
/// <summary>
/// 5021 小游戏——十八轮盘
/// </summary>
public class GAME_SLOT_SMALL_EIGHTEENROULETTE_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_EIGHTEENROULETTE_RECEIVE : HttpMessages
{
    public int errorCode;
    public int index;
    public double prize;
}
/// <summary>
/// 5023 
/// </summary>
public class GAME_SLOT_SMALL_BIGORSMALLPRIZE_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_SLOT_SMALL_BIGORSMALLPRIZE_RECEIVE : HttpMessages
{
    public int errorCode;
    public double prize;
}
/// <summary>
/// 5024 更新账户余额
/// </summary>
public class GAME_HALL_BALANCE_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_HALL_BALANCE_RECEIVE : HttpMessages
{
    public int errorCode;
    public double balance;
}
/// <summary>
/// 5200 大奖奖池的显示
/// </summary>
public class GAME_JACKPOT_POOL_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_JACKPOT_POOL_RECEIVE : HttpMessages
{
    public int errorCode;
    public double megaBalance;
    public double miniBalance;
    public double minorBalance;
}
/// <summary>
/// 5201 jackpot mega
/// </summary>
public class GAME_JACKPOT_MEGA_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_JACKPOT_MEGA_RECEIVE : HttpMessages
{
    public int errorCode;
    public string jackpot;
    public string allPaid;
    public string paid1;
    public string paid2;
    public string paid3;
    public string paid4;
    public string paid5;
}
/// <summary>
/// 5202 jackpot mini
/// </summary>
public class GAME_JACKPOT_MINI_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_JACKPOT_MINI_RECEIVE : HttpMessages
{
    public int errorCode;
    public string jackpot;
    public string allPaid;
    public string paid1;
    public string paid2;
    public string paid3;
}
/// <summary>
/// 5203 jackpot minor
/// </summary>
public class GAME_JACKPOT_MINOR_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
}
public class GAME_JACKPOT_MINOR_RECEIVE : HttpMessages
{
    public int errorCode;
    public string jackpot;
    public string allPaid;
    public int percent;
}
/// <summary>
/// 5301 PayTable
/// </summary>
public class CONFIG_GET_PAID_SEND : HttpMessages
{
    public string sessionId;
    public int msgId;
    public int gameId;
}
public class CONFIG_GET_PAID_RECEIVE : HttpMessages
{
    public int errorCode;
    public int gameId;
    public int gameType;
    public int gameChildType;
    public string configData;
}

public class BetMsg_SEND : HttpMessages
{
    public string sessionId;
}
public class BetMsg_RECEIVE : HttpMessages
{
    public int errorCode;
    public BetLevel[] betLevel;
    public LineLevel[] lineLevel;
    
    public class BetLevel
    {
        public int betType;
        public int level;
        public string bet;
    }
    public class LineLevel
    {
        public int betType;
        public int level;
        public int line;
    }
}
public class ArchiveDataForm : HttpMessages
{
    public string companyName;
    public string productName;
    public string bundleIdentifier;
    public string bundleVersion;
    public string shortBundleVersion;
    public string platform;
    public string ftpUrl;
    public string tokenUrl;
    public string gameUrl;
    public string versionUrl;
    public string signUrl;
    public string apkUrl;
    public string apkName;
    public string keyStr;
    public string agent;
}
public class GET_INFORM_SEND : HttpMessages
{
    public string check_key;
    public string short_name;
}
public class GET_INFORM_RECEIVE : HttpMessages
{
    public int error_code;
    public string platform;
    public string agent;
    public string key;
    public string token_url;
    public string game_url;
}

/*                                          根据索引加载关卡
* 
                                            0表示中国风
                                            1表示古埃及
                                            2表示……

                                            中国风的ID = 1
                                            皇冠CEO = 2
                                            SEXY女忧 = 3
                                            埃及的 = 4
                                            哥特 = 5
                                            异形 = 6
                                            男勇士 = 7
                                            女武士 = 8
                                            女战士 = 9
                                            蜘蛛侠 = 10
                                            足球 = 11
                                            星际大战 = 12
                                            阿拉丁 = 13
                                            财神 = 14
                                            沉默武士 = 15
                                            财富宝藏 = 16
                                            恶龙传说 = 17

编号  英文名称    中文说明
1   IPAD    
2   IPHONE  
3   PC  
4   BIKE    
5   MEGAJACKPOT 
6   MINIJACKPOT 
7   MINORJACKPOT    
8   WILD    百搭
9   FREE    免费
10  SMALL   小游戏
11  SCATTER 散图
12  LINEBEGIN   
12  A   
13  K   
14  Q   
15  J   
16  10  
17  BASE06  基础牌6
18  BASE07  基础牌7
19  BASE08  基础牌8
20  LINEEND 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;

public class Conversion: MonoBehaviour
{
    public static bool isTransCredits = true;
    static int[] showIconEffectIcon;
    //根据图标索引返回图标名称
    public static string SelectIcon(int index)
    {
        switch (index)
        {
            case 1:
                return "IPAD";
            case 2:
                return "IPHONE";
            case 3:
                return "PC";
            case 4:
                return "BIKE";
            case 5:
                return "MEGAJACKPOT";
            case 6:
                return "MINIJACKPOT";
            case 7:
                return "MINORJACKPOT";
            case 8:
                return "WILD";
            case 9:
                return "FREE";
            case 10:
                return "SMALL";
            case 11:
                return "SCATTER";
            case 12:
                return "A";
            case 13:
                return "K";
            case 14:
                return "Q";
            case 15:
                return "J";
            case 16:
                return "10";
            case 17:
                return "BASE06";
            case 18:
                return "BASE07";
            case 19:
                return "BASE08";
            default :
                return "";
        }
    }
    public static int[] AnimationConversion(int index, int form)
    {
        int[] iconIndex = new int[5];
        //返回线上连上的5个点
        SecurityParser sp = new SecurityParser();
        string xmlPath = "XMLData/GameSlot";

        Object xml = Resources.Load(xmlPath);
        sp.LoadXml(xml.ToString());
        System.Security.SecurityElement se = sp.ToXml();
        foreach (System.Security.SecurityElement child in se.Children)
        {
            if (int.Parse(child.Attribute("id")) == index)
            {
                iconIndex [0] = int.Parse(child.Attribute("offset1")) - 1;
                iconIndex [1] = int.Parse(child.Attribute("offset2")) - 1;
                iconIndex [2] = int.Parse(child.Attribute("offset3")) - 1;
                iconIndex [3] = int.Parse(child.Attribute("offset4")) - 1;
                iconIndex [4] = int.Parse(child.Attribute("offset5")) - 1;
            }
        }
        //根据form来返回需要播放特效的点
        //左 3，左4，右3，右4，全部
        switch (form)
        {
            case 1:
                showIconEffectIcon = new int[3];
                for (int i = 0; i<3; i++)
                {
                    showIconEffectIcon [i] = iconIndex [i];
                }
                break;
            case 2:
                showIconEffectIcon = new int[3];
                for (int i = 0; i<3; i++)
                {
                    showIconEffectIcon [i] = iconIndex [i + 2];
                }
                break;
            case 3:
                showIconEffectIcon = new int[4];
                for (int i = 0; i<4; i++)
                {
                    showIconEffectIcon [i] = iconIndex [i];
                }
                break;
            case 4:
                showIconEffectIcon = new int[4];
                for (int i =0; i<4; i++)
                {
                    showIconEffectIcon [i] = iconIndex [i + 1];
                }
                break;
            case 5:
                showIconEffectIcon = new int[5];
                for (int i =0; i<5; i++)
                {
                    showIconEffectIcon [i] = iconIndex [i];
                }
                break;
            default:
                break;
        }

        return showIconEffectIcon;
    }

    //等级与线数的转换
    public static int Trangelines(int lines)
    {
        if (SceneManager.lineCount < 20)
        {
            switch (lines)
            {
                case 3:
                    return 1;
                case 6:
                    return 2;
                case 9:
                    return 3;
                case 12:
                    return 4;
                case 15:
                    return 5;
                default:
                    return 0;
            }
        } else if (SceneManager.lineCount < 50)
        {
            switch (lines)
            {
                case 5:
                    return 1;
                case 10:
                    return 2;
                case 15:
                    return 3;
                case 20:
                    return 4;
                case 25:
                    return 5;
                default:
                    return 0;
            }
        } else
        {
            switch (lines)
            {
                case 10:
                    return 1;
                case 20:
                    return 2;
                case 30:
                    return 3;
                case 40:
                    return 4;
                case 50:
                    return 5;
                case 60:
                    return 6;
                case 70:
                    return 7;
                case 80:
                    return 8;
                case 90:
                    return 9;
                case 100:
                    return 10;
                default:
                    return 0;
            }
        }
    }
    //等级与押注的转换
    public static int TrangeBet(string betMoney)
    {
        return LinesBetManager.currentIndex+1;
        /*
        switch (betMoney)
        {
            case "1":
                return 1;
            case "5":
                return 2;
            case "10":
                return 3;
            case "20":
                return 4;
            case "50":
                return 5;
            case "100":
                return 6;
            case "200":
                return 7;
            case "300":
                return 8;
            default:
                return 0;
        }
        */
    }
    //金钱换算
    public static int MoneyTranTime(double money)
    {
        if (money > 1000000)
            return 8;
        else if (money > 100000)
            return 6;
        else if (money > 10000)
            return 4;
        else if (money > 1000)
            return 3;
        else if (money > 100)
            return 2;
        else
            return 1;
    }


    //金钱单位换算
    public static double MoneyUnitConver(string money)
    {
        double credits;
        if (money.Contains("k"))
            credits = double.Parse(money.Split('k') [0]) * 1000d;
        else if (money.Contains("m"))
            credits = double.Parse(money.Split('m') [0]) * 1000000d;
        else
            credits = double.Parse(money);
        return credits;
    }
    public static string MoneyUnitConver(double money)
    {
        if (isTransCredits)
        {
            string moneyStr = string.Empty;
            /*
            if (money >= 1000000d)
                moneyStr = (money / 1000000d).ToString("f2") + "m";
            else if (money >= 1000d)
                moneyStr = (money / 1000d).ToString("f2") + "k";
            else     
                moneyStr = money.ToString("f2");
                */
            if(money >= 1000000d)
                moneyStr = (money/1000d).ToString("f2") +"k";
            else if(money >= 10000000000d)
                moneyStr = (money/1000000d).ToString("f2") +"m";
            else
                moneyStr = money.ToString("f2");

            return moneyStr;
        } else
        {
            return money.ToString("f2");
        }
    }
    public static string SceneNameIDConver(int id)
    {
        GameInformation ginfo;
        SceneManager.gInfos.TryGetValue(id, out ginfo);
//        Debug.Log(ginfo.NAME);
        return ginfo.NAME;
    }
    public static int SceneNameIDConver(string name)
    {
        GameInformation ginfo;
        foreach(KeyValuePair<int,GameInformation> gi in SceneManager.gInfos)
        {
            if(gi.Value.NAME == name)
            {
                return gi.Key;
            }
        }
        return 0;
    }


}

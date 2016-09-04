using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Collections.Generic;
public class LinesBetManager : MonoBehaviour {

    public AudioClip[] betAudio;
    public AudioClip[] lineAudio;
    public UILabel betLabel;
    public UILabel lineLabel;
    public UILabel betMaxLabel;
    private Object xml;
    private int LineCountDelta;
    public static double[] MoneyCountDelta15 = new double[8];
    public static double[] MoneyCountDelta25 = new double[8];
    public static double[] MoneyCountDelta50 = new double[8];
    public static double[] MoneyCountDelta100 = new double[8];

    public double[] MoneyCountDelta = new double[8];

    //当前档数
    public static int currentIndex = 0;

    public Dictionary<int,int> LineGear = new Dictionary<int, int>();
    private AudioSource asSource;


    void Start()
    {
        currentIndex = 0;
        asSource = GameObject.Find("/UI Root").GetComponent<AudioSource>();
        ReadTheXml();
        switch (SceneManager.lineCount)
        {
            case 15:
                MoneyCountDelta = MoneyCountDelta15; break;
            case 25:
                MoneyCountDelta = MoneyCountDelta25; break;
            case 50:
                MoneyCountDelta = MoneyCountDelta50; break;
            case 100:
                MoneyCountDelta = MoneyCountDelta100; break;
            default: break;
        }
        lineLabel.text = SceneManager.lineCount.ToString();
        betLabel.text = MoneyCountDelta[currentIndex].ToString("f2");
        betMaxLabel.text = ((int.Parse(lineLabel.text)*double.Parse(betLabel.text))).ToString();
        
    }
    //新改革加线按钮方法
    public void BetlineMethod()
    {
        if (GameObject.FindGameObjectWithTag("Help"))
            GameObject.FindGameObjectWithTag("Help").GetComponent<UIPanel>().alpha = 0;
        lineLabel.text = (int.Parse(lineLabel.text)+LineCountDelta).ToString();
        if(int.Parse(lineLabel.text)>SceneManager.lineCount)
        {
            lineLabel.text=LineCountDelta.ToString();
        }
        betMaxLabel.text = ((int.Parse(lineLabel.text)* double.Parse(betLabel.text))).ToString();
        asSource.PlayOneShot(lineAudio[Conversion.Trangelines(int.Parse(lineLabel.text))-1]);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<LineSelect>().MenuLineShowMethod(int.Parse(lineLabel.text),LineCountDelta);
    }
    public void BetMoneyMethod()
    {
        betLabel.text = addBetMoneyConversion(betLabel.text);
        betMaxLabel.text = ((int.Parse(lineLabel.text)* double.Parse(betLabel.text))).ToString();
        asSource.PlayOneShot(betAudio[Conversion.TrangeBet(betLabel.text)-1]);
    }
    //BetMax
    public void betMaxMathod()
    {
        if (GameObject.FindGameObjectWithTag("Help"))
            GameObject.FindGameObjectWithTag("Help").GetComponent<UIPanel>().alpha = 0;
        currentIndex = MoneyCountDelta.Length-1;
        betLabel.text = MoneyCountDelta[currentIndex].ToString();
        lineLabel.text=SceneManager.lineCount.ToString();
        betMaxLabel.text = ((int.Parse(lineLabel.text)* double.Parse(betLabel.text))).ToString();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<LineSelect>().MenuLineShowMethod(int.Parse(lineLabel.text),LineCountDelta);
    }
    //加钱
    public string addBetMoneyConversion(string betPerline)
    {
        currentIndex++;
        if(currentIndex >= MoneyCountDelta.Length)
        {
            currentIndex = 0;
        }
        return MoneyCountDelta[currentIndex].ToString();
    }
    void ReadTheXml()
    {
        string xmlpath = "XMLData/linesBet";
        xml = Resources.Load(xmlpath);
        SecurityParser sp = new SecurityParser();
        sp.LoadXml(xml.ToString());
        System.Security.SecurityElement se = sp.ToXml();
        foreach (System.Security.SecurityElement child in se.Children)
        {
            if(SceneManager.lineCount == int.Parse(child.Attribute("gear")))
            {
                int i = 0;
                LineGear.Add(int.Parse(child.Attribute("gear")),int.Parse(child.Attribute("delta")));
                foreach(System.Security.SecurityElement cc in child.Children)
                {
                    MoneyCountDelta[i] =double.Parse(cc.Text);
                    i++;
                }

            }
        }
        LineGear.TryGetValue(SceneManager.lineCount,out LineCountDelta);
    }
    /**
    //加线
    public void addBetlineMethod()
    {
        if (GameObject.FindGameObjectWithTag("Help"))
            GameObject.FindGameObjectWithTag("Help").GetComponent<UIPanel>().alpha = 0;
        lineLabel.text = (int.Parse(lineLabel.text)+LineCountDelta).ToString();
        if(int.Parse(lineLabel.text)>=SceneManager.lineCount)
        {
            lineLabel.text = SceneManager.lineCount.ToString();
        }
        betMaxLabel.text = ""+(int.Parse(lineLabel.text)*float.Parse(betLabel.text));
        if(lineLabel.text == "10"&&betLabel.text == "0.01")
        {
            betMaxLabel.text = "0.1";
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<LineSelect>().MenuLineShowMethod(int.Parse(lineLabel.text),LineCountDelta);
    }
    //减线
    public void reduceBetlineMethod()
    {
        if (GameObject.FindGameObjectWithTag("Help"))
            GameObject.FindGameObjectWithTag("Help").GetComponent<UIPanel>().alpha = 0;
        lineLabel.text = (int.Parse(lineLabel.text)-LineCountDelta).ToString();
        if(int.Parse(lineLabel.text)<=LineCountDelta)
        {
            lineLabel.text=LineCountDelta.ToString();
        }
        betMaxLabel.text = ""+(int.Parse(lineLabel.text)*float.Parse(betLabel.text));
        if(lineLabel.text == "10"&&betLabel.text == "0.01")
        {
            betMaxLabel.text = "0.1";
        }
        //主界面线数选择方法
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<LineSelect>().MenuLineShowMethod(int.Parse(lineLabel.text),LineCountDelta);
    }

    //减钱
    public void reduceBetMoneyMethod()
    {
        betLabel.text = reduceBetMoneyConversion(betLabel.text);
        betMaxLabel.text = ""+(int.Parse(lineLabel.text)*float.Parse(betLabel.text));
        if(lineLabel.text == "10"&&betLabel.text == "0.01")
        {
            betMaxLabel.text = "0.1";
        }
    }
    //加钱
    public void addBetMoneyMethod()
    {
        betLabel.text = addBetMoneyConversion(betLabel.text);
        betMaxLabel.text = ""+(int.Parse(lineLabel.text)*float.Parse(betLabel.text));
        if(lineLabel.text == "10"&&betLabel.text == "0.01")
        {
            betMaxLabel.text = "0.1";
        }
    }
    //减钱
    public string reduceBetMoneyConversion(string betPerline)
    {
        if (betPerline == "300")
            betPerline = "200";
        else if (betPerline == "200")
            betPerline = "100";
        else if (betPerline == "100")
            betPerline = "50";
        else if (betPerline == "0")
            betPerline = "20";
        else if (betPerline == "20")
            betPerline = "10";
        else if (betPerline == "10")
            betPerline = "5";
        else if (betPerline == "5")
            betPerline = "1";
        
        return betPerline;
    }
       */

}

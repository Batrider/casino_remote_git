using UnityEngine;
using System.Collections;

public class JackpotRecive : MonoBehaviour {
   
    public bool matrixing;
    public UILabel MegaLabel;
    public UILabel MiniLabel;
    public UILabel MinorLabel;
    static double megaJackpot = 0;
    static double miniJackpot = 0;
    static double minorJackpot = 0;
    float timeInterval = 0f;
    void Update()
    {
        timeInterval += Time.deltaTime;
        if (timeInterval > 0.5f)
        {
            timeInterval = 0;
            if(matrixing)
            {
                MegaLabel.text =Conversion.MoneyUnitConver(megaJackpot);
                MiniLabel.text =Conversion.MoneyUnitConver(miniJackpot);
                MinorLabel.text =Conversion.MoneyUnitConver(minorJackpot);
            }
            else
            {
                MegaLabel.text = megaJackpot.ToString();
                MiniLabel.text = miniJackpot.ToString();
                MinorLabel.text = minorJackpot.ToString();
            }
        }
    }
    public void RefreshJackpot(double mega,double mini,double minor)
    {
        megaJackpot = mega;
        miniJackpot = mini;
        minorJackpot = minor;
    }
    void OnEnable()
    {
        NetworkConnect.jackpotData += RefreshJackpot;
    }
    void OnDisable()
    {
        NetworkConnect.jackpotData -= RefreshJackpot;  
    }
}

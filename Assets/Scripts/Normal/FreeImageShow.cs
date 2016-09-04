using UnityEngine;
using System.Collections;

public class FreeImageShow : MonoBehaviour {
    public GameObject[] jackpot;
    public GameObject freeImage;
    public UILabel freeLabel;
    public void ImageShow()
    {
        TweenAlpha.Begin(freeImage, .3f, 1);
        foreach (GameObject go in jackpot)
        {
            TweenAlpha.Begin(go,.3f,0);
        }
        StartCoroutine(WaitToResume());
    }
    IEnumerator WaitToResume()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if(int.Parse(freeLabel.text)==0)
            {
                yield return new WaitForSeconds(3f);
                TweenAlpha.Begin(freeImage, .3f, 0);
                foreach (GameObject go in jackpot)
                {
                    TweenAlpha.Begin(go,.3f,1);
                }
                break;
            }
        }
    }
    void OnEnable()
    {
        manager.freeEffect += ImageShow;
    }
    void OnDisable()
    {
        manager.freeEffect -= ImageShow;
    }
}

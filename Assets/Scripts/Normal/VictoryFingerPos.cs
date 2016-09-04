using UnityEngine;
using System.Collections;

public class VictoryFingerPos : MonoBehaviour {
    string tipPlayerPrefabs;

    void Start()
    {
        if (PlayerPrefs.GetInt(tipPlayerPrefabs) == 0)
        {
            this.transform.parent = GameObject.FindGameObjectWithTag("ship").transform;
            this.transform.localPosition = new Vector3(30,150,0);
            TweenPosition tp = TweenPosition.Begin(gameObject,1,new Vector3(30,120,0));
            tp.style = UITweener.Style.PingPong;
            tp.from = new Vector3(30,150,0);
                                                  
        }
    }
}

using UnityEngine;
using System.Collections;

public class ChangeSprite : MonoBehaviour {
    public string[] spriteName;
    public float  frequency = 10;
    private UISprite us;
    void Start()
    {
        us = GetComponent<UISprite>();
        StartCoroutine(StartCount());
    }
    IEnumerator StartCount()
    {
        int index = 0;
        float currentTime = 0;
        while(true)
        {
            currentTime+=Time.deltaTime;
            if(currentTime>frequency)
            {
                currentTime = 0;
                index ++;
                if(index>spriteName.Length-1)
                    index = 0;
                us.spriteName = spriteName[index];
            }
            yield return null;
                
        }



    }
}

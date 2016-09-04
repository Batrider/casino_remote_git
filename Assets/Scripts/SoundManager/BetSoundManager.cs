using UnityEngine;
using System.Collections;

public class BetSoundManager : MonoBehaviour {

    public AudioClip[] betAudio;
    int index;
    UIPlaySound us;
    void Start()
    {
        index = 0;
        us = GetComponent<UIPlaySound>();
        us.audioClip = betAudio[index];
    }
    public void ChangeAudio()
    {
        index++;
        us.audioClip = betAudio[index];
    }


}

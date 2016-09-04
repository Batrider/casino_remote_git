using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public UISlider uslMusic;
    public UISlider uslVolume;
    void Awake()
    {
        SetValue();
    }

    public void SetVolumeVal(float val)
    {
        Debug.Log(val);
        PlayerData.Volume = val;
        NGUITools.soundVolume = val;
    }
    public void SetMusicVal(float val)
    {
        Debug.Log(val);
        PlayerData.Music = val;
        Camera.main.audio.volume = val;
    }
    public void SetValue()
    {
        uslMusic.value = PlayerData.Music;
        uslVolume.value = PlayerData.Volume;
    }
}

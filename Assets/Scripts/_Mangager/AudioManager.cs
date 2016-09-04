using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip specialStopClip;
    public AudioClip normalStopClip;
    private AudioSource ase;
    private AudioSource smallAudio;
    private AudioSource mainAudio;
    private AudioSource wheelAudio;
    private AudioSource wheelStop;
    float maxMusic;
    void Start()
    {
        NGUITools.soundVolume = PlayerData.Volume;
        maxMusic = PlayerData.Music;
        ase = GetComponent<AudioSource>();
        mainAudio = GameObject.Find("/UI Root/Camera").GetComponent<AudioSource>();
        wheelAudio = GameObject.Find("/UI Root/Camera/Scroll View").GetComponent<AudioSource>();
        wheelStop = GameObject.Find("/UI Root").GetComponent<AudioSource>();
        smallAudio = GameObject.Find("/SmallGame/Camera/SmallGame").GetComponent<AudioSource>();

        wheelStop.volume = PlayerData.Volume;
        mainAudio.volume = PlayerData.Music;
        
    }
    public void WheelStopMusic(bool normal)
    {
        if (normal)
            ase.PlayOneShot(normalStopClip);
        else
            ase.PlayOneShot(specialStopClip);
    }
    void Update()
    {
        WheelRuningMusic();
    }
    void WheelRuningMusic()
    {
        if (manager.startGame)
        {
            smallAudio.volume = 0.02f * maxMusic;
            mainAudio.volume = 0.5f * maxMusic;
            if (!wheelAudio.isPlaying)
            {
                wheelAudio.Play();
            }
        } else if (!manager.canmove [2])
        {
            wheelAudio.Stop();
            SmallGameMusic();
        }
    }
    void SmallGameMusic()
    {
        if (!manager.jackpot)
        {
            if (manager.smallgame && smallAudio.volume < 0.99f * maxMusic)
            {
                smallAudio.volume = Mathf.Lerp(smallAudio.volume, 1 * maxMusic, Time.deltaTime);
                mainAudio.volume = Mathf.Lerp(mainAudio.volume, 0, Time.deltaTime);
            } else if (!manager.smallgame && smallAudio.volume > 0.01 * maxMusic)
            {
                smallAudio.volume = Mathf.Lerp(smallAudio.volume, 0, Time.deltaTime);
                mainAudio.volume = Mathf.Lerp(mainAudio.volume, 1 * maxMusic, Time.deltaTime);
            } else if (!manager.smallgame && mainAudio.volume < 0.99f * maxMusic)
            {
                mainAudio.volume = Mathf.Lerp(mainAudio.volume, 1 * maxMusic, Time.deltaTime);
            }
        } else
        {
            mainAudio.volume = 0;
            smallAudio.volume = 0;
        }
    }
}

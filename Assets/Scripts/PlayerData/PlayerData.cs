using UnityEngine;
using System.Collections;

public class PlayerData{
    public static float Volume
    {
        set
        {
            PlayerPrefs.SetFloat("Volume",value);
        }
        get
        {
            return PlayerPrefs.GetFloat("Volume",1);
        }
    }
    public static float Music
    {
        set
        {
            PlayerPrefs.SetFloat("Music",value);
        }
        get
        {
            return PlayerPrefs.GetFloat("Music",1);
        }
    }
}

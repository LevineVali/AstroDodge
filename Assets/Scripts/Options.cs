using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Options
{
    public float SoundVolume;
    public float MusicVolume;
    public bool SoundMute;
    public bool MusicMute;


    public Options(Options _o)
    {
        SoundVolume = _o.SoundVolume;
        MusicVolume = _o.MusicVolume;
        SoundMute = _o.SoundMute;
        MusicMute = _o.MusicMute;
    }

    /// <summary>
    /// Load Default values
    /// </summary>
    public Options()
    {
        SoundVolume = 1;
        MusicVolume = 1;
        SoundMute = false;
        MusicMute = false;
    }
}

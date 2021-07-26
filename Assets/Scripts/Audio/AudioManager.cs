using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Window of Options
    /// </summary>
    public GameObject OptionsWindow;

    /// <summary>
    /// playercontroller
    /// </summary>
    public PlayerController pc;

    [Header("Audios")]
    public Sound[] Music;
    public Sound[] Sound;

    [Header("Options")]
    public Slider MusicV;
    public Slider SoundV;
    public Image MusicIcon;
    public Image SoundIcon;

    public Sprite SoundSprite;
    public Sprite SoundMuteSprite;
    public Sprite MusicSprite;
    public Sprite MusicMuteSprite;

    private Options options;

    private float oldMusicV;
    private float oldSoundV;

    private float helper;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < Music.Length; i++)
        {
            Music[i].Source = gameObject.AddComponent<AudioSource>();

            Music[i].Source.clip = Music[i].Clip;
            Music[i].Source.volume = 1f;
            Music[i].Source.pitch = Music[i].Pitch;
            Music[i].Source.loop = Music[i].Loop;
            Music[i].RealName = Music[i].Source.name;
        }

        for (int i = 0; i < Sound.Length; i++)
        {
            Sound[i].Source = gameObject.AddComponent<AudioSource>();

            Sound[i].Source.clip = Sound[i].Clip;
            Sound[i].Source.volume = 1f;
            Sound[i].Source.pitch = Sound[i].Pitch;
            Sound[i].Source.loop = Sound[i].Loop;
            Sound[i].RealName = Sound[i].Source.name;
        }
    }

    void Start()
    {
        options = SaveSystem.LoadOptions();

        MusicV.value = options.MusicVolume;
        SoundV.value = options.SoundVolume;

        for (int i = 0; i < Music.Length; i++)
        {
            if (options.MusicMute)
            {
                Music[i].Source.volume = 0;
            }
            else
            {
                Music[i].Source.volume = MusicV.value * Music[i].Volume;
            }
        }
        for (int i = 0; i < Sound.Length; i++)
        {
            if (options.SoundMute)
            {
                Sound[i].Source.volume = 0;
            }
            else
            {
                Sound[i].Source.volume = SoundV.value * Sound[i].Volume * pc.soundValueModifire;
            }
        }

        if (options.MusicMute)
        {
            MusicIcon.sprite = MusicMuteSprite;
        }
        else
        {
            MusicIcon.sprite = MusicSprite;
        }

        if (options.SoundMute)
        {
            SoundIcon.sprite = SoundMuteSprite;
        }
        else
        {
            SoundIcon.sprite = SoundSprite;
        }

        helper = pc.soundValueModifire;
    }

    private void Update()
    {
        if (OptionsWindow.activeSelf == true)
        {
            SetVolume();

            if (MusicV.value != oldMusicV)
            {
                // set new setting
                options.MusicVolume = MusicV.value;
                // save settings
                SaveSystem.SaveOptions(options);
                // set old value for checking
                oldMusicV = MusicV.value;
            }

            if (SoundV.value != oldSoundV)
            {
                // set new setting
                options.SoundVolume = SoundV.value;
                // save settings
                SaveSystem.SaveOptions(options);
                // set old value for checking
                oldSoundV = SoundV.value;
            }
        }

        if (pc.soundValueModifire != helper && !options.SoundMute)
        {
            for (int i = 0; i < Sound.Length; i++)
            {
                Sound[i].Source.volume = SoundV.value * Sound[i].Volume * pc.soundValueModifire;
            }
            helper = pc.soundValueModifire;
        }
    }

    public void Play(string _name)
    {
        Sound s = Array.Find(Music, sound => sound.RealName == _name);

        if (s == null)
        {
            s = Array.Find(Sound, sound => sound.RealName == _name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + _name + " not found!");
                return;
            }
        }
        s.Source.Play();
    }

    public void FadeOut(AudioSource audioSource, SoundTyp _typ, float FadeTime)
    {
        int index = 0;
        switch (_typ)
        {
            case SoundTyp.MUSIC:
                for (int i = 0; i < Music.Length; i++)
                {
                    if (Music[i].RealName == audioSource.name)
                    {
                        index = i;
                        break;
                    }
                }

                if (audioSource.volume > 0)
                {
                    audioSource.volume -= MusicV.value * Music[index].Volume * Time.deltaTime / FadeTime;
                    break;
                }

                audioSource.Stop();
                audioSource.volume = MusicV.value * Music[index].Volume;
                break;
            case SoundTyp.SOUND:
                for (int i = 0; i < Music.Length; i++)
                {
                    if (Sound[i].RealName == audioSource.name)
                    {
                        index = i;
                        break;
                    }
                }

                if (audioSource.volume > 0)
                {
                    audioSource.volume -= SoundV.value * Sound[index].Volume * Time.deltaTime / FadeTime;
                    break;
                }

                audioSource.Stop();
                audioSource.volume = SoundV.value * Sound[index].Volume;
                break;
        }
    }

    public void MuteSound()
    {
        options.SoundMute = !options.SoundMute;

        if (options.SoundMute)
        {
            SoundIcon.sprite = SoundMuteSprite;
        }
        else
        {
            SoundIcon.sprite = SoundSprite;
        }

        SaveSystem.SaveOptions(options);
    }

    public void MuteMusic()
    {
        options.MusicMute = !options.MusicMute;

        if (options.MusicMute)
        {
            MusicIcon.sprite = MusicMuteSprite;
        }
        else
        {
            MusicIcon.sprite = MusicSprite;
        }

        SaveSystem.SaveOptions(options);
    }

    public enum SoundTyp
    {
        MUSIC,
        SOUND
    }

    public void SetVolume()
    {
        for (int i = 0; i < Music.Length; i++)
        {
            if (options.MusicMute)
            {
                Music[i].Source.volume = 0;
            }
            else
            {
                Music[i].Source.volume = MusicV.value * Music[i].Volume;
            }
            Music[i].Source.pitch = Music[i].Pitch;
        }

        for (int i = 0; i < Sound.Length; i++)
        {
            if (options.SoundMute)
            {
                Sound[i].Source.volume = 0;
            }
            else
            {
                Sound[i].Source.volume = SoundV.value * Sound[i].Volume * pc.soundValueModifire;
            }
            Sound[i].Source.pitch = Sound[i].Pitch;
        }
    }
}

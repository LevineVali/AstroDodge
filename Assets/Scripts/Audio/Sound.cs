using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    /// <summary>
    /// Name of the element for editor
    /// </summary>
    public string Name;

    [HideInInspector]
    /// <summary>
    /// Name of this Sound
    /// </summary>
    public string RealName;

    /// <summary>
    /// Clip of this Sound
    /// </summary>
    public AudioClip Clip;

    /// <summary>
    /// Volume of this Sound
    /// </summary>
    [Range(0f, 1f)]
    public float Volume;

    /// <summary>
    /// Pitch of this Sound
    /// </summary>
    [Range(.1f, 3f)]
    public float Pitch;

    /// <summary>
    /// Loop this sound
    /// </summary>
    public bool Loop;

    /// <summary>
    /// Audiosource of this Sound
    /// </summary>
    [HideInInspector]
    public AudioSource Source;
}
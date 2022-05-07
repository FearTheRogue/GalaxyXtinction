using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script, along with the 'AudioManager' class
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=6OT43pvUyfY
/// 
/// This script has been modified with the AudioMixerGroup, spacialBlend properties.
/// 
/// This custom class is used in the Inspector, where the properties are assigned.
/// 
/// </summary>

[System.Serializable]
public class Sound
{
    public string name;

    public AudioMixerGroup group;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;

    [Range(0f,1f)]
    public float spatialBlend;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

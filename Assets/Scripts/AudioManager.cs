using UnityEngine.Audio;
using UnityEngine;
using System;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script, along with the 'Sound' class
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=6OT43pvUyfY
/// 
/// This script has been modified with the PlayOnce(), Stop(), and IsClipPlaying() methods.
/// 
/// Script handles the audio in the project, using an array of 'sounds'.
/// Each 'sound' property is updated with the value in the Inspector.
/// 
/// </summary>

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;

    public Sound[] sounds;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Assigning values from Inspector to the Audio Source component
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.group;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    private void Start()
    {
        // Retrieves a value to the audio mixer from PlayerPrefs if exists
        if (PlayerPrefs.HasKey("MasterVol"))
            mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        if (PlayerPrefs.HasKey("MusicVol"))
            mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        if (PlayerPrefs.HasKey("SFXVol"))
            mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));

        Play("Main Music");
    }

    // Plays the audio clip specified by the name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Play();
    }

    // Plays the audio clip once specified by the name
    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.PlayOneShot(s.clip);
    }

    // Stops the audio clip specified by the name
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Stop();
    }

    // Checks is the current audio clip is playing, and return either true or false
    // Specified by the clip name
    public bool IsClipPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s.source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

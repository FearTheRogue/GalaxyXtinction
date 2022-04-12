using UnityEngine.Audio;
using UnityEngine;
using System;

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
        if (PlayerPrefs.HasKey("MasterVol"))
            mixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        if (PlayerPrefs.HasKey("MusicVol"))
            mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        if (PlayerPrefs.HasKey("SFXVol"))
            mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));

        Play("Main Music");
    }

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

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Stop();
    }

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

using System;
using ScriptableObjects;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Songs songs;
    private Sound[] sounds;
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        sounds = new Sound[songs.songs.Length];
        
        for (var i = 0; i < songs.songs.Length; i++)
        {
            var song = songs.songs[i];
            sounds[i] = new Sound
            {
                name = song.songName,
                clip = song.audioClipNormal,
                volume = song.volume,
                pitch = song.pitch,
                loop = song.loop,
                playOnAwake = song.playOnAwake,
                source = gameObject.AddComponent<AudioSource>()
            };
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
            sounds[i].source.playOnAwake = sounds[i].playOnAwake;
        }

    }

    public AudioClip GetAudioClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            {
                print("Sound" + name + "not found");
                return null;
            }
        }

        return s.clip;
    }

    public void PlayVFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            {
                print("Sound" + name + "not found");
                return;
            }
        }

        s.source.Play();
    }

    public void StopVFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
        {
            {
                print("Sound" + name + "not found");
                return;
            }
        }
        
        s.source.Stop();
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            {
                print("Sound" + name + "not found");
                return;
            }
        }

        s.source.Play();
    }
    public void StopMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null)
        {
            {
                print("Sound" + name + "not found");
                return;
            }
        }
        
        s.source.Stop();
    }

    public void StopAllMusic()
    {
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }
}

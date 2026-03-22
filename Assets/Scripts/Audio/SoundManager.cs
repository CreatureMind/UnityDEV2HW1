using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour
{
    [FormerlySerializedAs("songs")] [SerializeField] private SongWraperSO songWraperSo;
    [FormerlySerializedAs("SFXs")][SerializeField] private SFXWraperSO vfxClips;
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

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
        
        // Initialize both songs and SFX in the same array
        int totalSounds = songWraperSo.songs.Length + vfxClips.SFX.Length;
        sounds = new Sound[totalSounds];
        
        // Initialize songs first
        for (var i = 0; i < songWraperSo.songs.Length; i++)
        {
            var song = songWraperSo.songs[i];
            sounds[i] = new Sound
            {
                name = song.songName,
                clip = song.audioClipNormal,
                volume = song.volume,
                pitch = song.pitch,
                loop = song.loop,
                playOnAwake = song.playOnAwake,
                mixerGroup = song.mixerGroup,
                source = gameObject.AddComponent<AudioSource>()
            };
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
            sounds[i].source.playOnAwake = sounds[i].playOnAwake;
            sounds[i].source.outputAudioMixerGroup = sounds[i].mixerGroup;
        }
        
        // Initialize SFX after songs
        int sfxStartIndex = songWraperSo.songs.Length;
        for (var i = 0; i < vfxClips.SFX.Length; i++)
        {
            var sfx = vfxClips.SFX[i];
            int soundIndex = sfxStartIndex + i;
            sounds[soundIndex] = new Sound
            {
                name = sfx.soundName,
                clip = sfx.audioClipNormal,
                volume = sfx.volume,
                pitch = sfx.pitch,
                loop = sfx.loop,
                playOnAwake = sfx.playOnAwake,
                mixerGroup = sfx.mixerGroup,
                source = gameObject.AddComponent<AudioSource>()
            };
            sounds[soundIndex].source.clip = sounds[soundIndex].clip;
            sounds[soundIndex].source.volume = sounds[soundIndex].volume;
            sounds[soundIndex].source.pitch = sounds[soundIndex].pitch;
            sounds[soundIndex].source.loop = sounds[soundIndex].loop;
            sounds[soundIndex].source.playOnAwake = sounds[soundIndex].playOnAwake;
            sounds[soundIndex].source.outputAudioMixerGroup = sounds[soundIndex].mixerGroup;
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
        if (s.source.outputAudioMixerGroup == null)
        {
            s.source.outputAudioMixerGroup = sfxMixerGroup;
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
    if (s.source.outputAudioMixerGroup == null)
    {
        s.source.outputAudioMixerGroup = musicMixerGroup;
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

    public void StopAllSounds()
    {
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }
}
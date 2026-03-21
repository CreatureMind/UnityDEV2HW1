using ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Song", menuName = "DDR/Song")]
public class SongSO : ScriptableObject
{
    public string songID;
    public string songName;
    public Sprite preview;
    public AudioClip audioClipNormal;
    public AudioClip audioClipMeme;
    [Range(0f,2f)]
    public float volume = 0.4f;
    [Range(.1f,3f)]
    public float pitch = 1f;
    public bool loop;
    public bool playOnAwake = true;
    [Range(60,300)]
    public int bpm;
    public DdrPatternSO[] patterns;
    public AudioMixerGroup mixerGroup;
}

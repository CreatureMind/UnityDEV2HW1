using ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SFX", menuName = "DDR/SFX")]
public class SoundEffectSO : ScriptableObject
{
    public string soundName;
    public AudioClip audioClipNormal;
    public AudioMixerGroup mixerGroup;
    [Range(0f,2f)]
    public float volume = 0.4f;
    [Range(.1f,3f)]
    public float pitch = 1f;
    public bool loop;
    public bool playOnAwake = true;
}

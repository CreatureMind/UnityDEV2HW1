using UnityEngine;
using UnityEngine.Audio;

public class SoundMixer : MonoBehaviour
{
    public static SoundMixer Instance { get; private set; }

    [SerializeField] private AudioMixer mainMixer;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMasterVolume(float value) => SetVolume("MasterVol", value);
    public void SetMusicVolume(float value) => SetVolume("MusicVol", value);
    public void SetSFXVolume(float value) => SetVolume("SFXVol", value);

    private void SetVolume(string parameterName, float value)
    {
        // Converts linear 0-1 to logarithmic decibels
        // We use 0.0001f to avoid Log10(0) which is undefined/error
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat(parameterName, dB);
    }
}
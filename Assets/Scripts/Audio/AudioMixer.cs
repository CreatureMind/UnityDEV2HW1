using UnityEngine;
using UnityEngine.Audio;

public class SoundMixer : MonoBehaviour
{
    public static SoundMixer Instance { get; private set; }
    
    [Header("Mixer Parameters")]
    [SerializeField] private string masterVol = "MasterVol";
    [SerializeField] private string musicVol = "MusicVol";
    [SerializeField] private string sfxVol = "SFXVol";
    [SerializeField] private AudioMixer mainMixer;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMasterVolume(float value) => SetVolume(masterVol, value);
    public void SetMusicVolume(float value) => SetVolume(musicVol, value);
    public void SetSFXVolume(float value) => SetVolume(sfxVol, value);

    private void SetVolume(string parameterName, float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat(parameterName, dB);
    }
}
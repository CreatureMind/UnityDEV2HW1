using System;
using Arrows;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects;

public class SongProgress : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image innerMarker;
    [SerializeField] private float cycleDuration;
    
    private float _songLength;
    private float _progress;
    private float _progressPerBeat;
    
    private const float SIXTY_SECONDS = 60f;
    
    void OnEnable()
    {
        DifficultyPopupManager.OnDifficultySelected += LoadSongData;
        ArrowsSpawner.OnBeat += AddProgress;
    }

    void OnDisable()
    {
        DifficultyPopupManager.OnDifficultySelected -= LoadSongData;
        ArrowsSpawner.OnBeat -= AddProgress;
    }

    private void Start()
    {
        if (innerMarker)
        {
            StartRainbowCycle();
        }
        else
        {
            Debug.LogError("No image found");
        }
    }

    private void LoadSongData(SongSO songSo, bool meme, int difficulty)
    {
        var currentPattern = songSo.patterns[difficulty];
        
        _songLength = !meme ? songSo.audioClipNormal.length : songSo.audioClipMeme.length;
        _songLength = _songLength - currentPattern.delay - currentPattern.delay;
        var totalBeats = _songLength * songSo.bpm / SIXTY_SECONDS;
        _progressPerBeat = 1f / totalBeats;
    }

    private void AddProgress()
    {
        _progress += _progressPerBeat;
        slider.value = _progress;
    }
    
    private void StartRainbowCycle()
    {
        DOVirtual.Float(0f, 1f, cycleDuration, UpdateColor)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void UpdateColor(float hueValue)
    {
        var rainbowColor = Color.HSVToRGB(hueValue, 1f, 1f);
        
        if (!innerMarker) return;
        innerMarker.color = rainbowColor;
    }
}

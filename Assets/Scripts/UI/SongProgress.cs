using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects;

public class SongProgress : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image innerMarker;
    [SerializeField] private float cycleDuration;
    
    private float songLength;
    private float progress;
    private float progressPerBeat;
    
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
        
        songLength = !meme ? songSo.audioClipNormal.length : songSo.audioClipMeme.length;
        songLength = songLength - currentPattern.delay - currentPattern.delay;
        var totalBeats = songLength * songSo.bpm / 60f;
        progressPerBeat = 1f / totalBeats;
    }

    private void AddProgress()
    {
        progress += progressPerBeat;
        slider.value = progress;
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

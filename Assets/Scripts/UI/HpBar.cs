using System;
using Arrows;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    [SerializeField] private float hpBarValue;
    [SerializeField] private float breatheAmount;
    [SerializeField] private float breatheSpeed;
    
    [SerializeField] private Image fillImage;
    [SerializeField] private float cycleDuration;
    
    private float gainAmount;
    private float penaltyAmount;
    private float offset;
    
    void OnEnable()
    {
        DifficultyPopupManager.OnDifficultySelected += LoadSongData;
        ArrowGoal.OnHit += AddLife;
        ArrowGoal.OnMiss += RemoveLife;
    }

    void OnDisable()
    {
        DifficultyPopupManager.OnDifficultySelected -= LoadSongData;
        ArrowGoal.OnHit -= AddLife;
        ArrowGoal.OnMiss -= RemoveLife;
    }
    
    private void LoadSongData(SongSO song, bool meme, int difficulty)
    {
        hpBarValue = 0.5f;

        var gain = 100 / song.patterns[difficulty].gain;
        var penalty = 100 / song.patterns[difficulty].penalty;
        gainAmount = 1 / gain;
        penaltyAmount = 1 / penalty;
    }
    
    private void Start()
    {
        DOTween.To(() => offset, x => offset = x, breatheAmount, breatheSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        
        if (fillImage)
        {
            StartRainbowCycle();
        }
        else
        {
            Debug.LogError("No image found");
        }
    }

    private void Update()
    {
        hpBarValue = Mathf.Clamp01(hpBarValue);
        slider.value = Mathf.Clamp(hpBarValue + offset, 0f, 1f);

        if (hpBarValue <= 0)
        {
            SoundManager.instance.StopAllSounds();
            SoundManager.instance.PlayVFX("VinylScrach");
            UI_Manager.Instance.SwapMenu(MenuType.DifficultySelectionMenu);
            SoundManager.instance.PlayVFX("BarAmbiance");
            SoundManager.instance.PlayVFX("BarMusic");
        }
    }   

    private void AddLife()
    {
        DOTween.To(() => hpBarValue, x => hpBarValue = x, hpBarValue + gainAmount, 0.2f);
    }
    
    private void RemoveLife()
    {
        DOTween.To(() => hpBarValue, x => hpBarValue = x, hpBarValue - penaltyAmount, 0.2f);
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
        
        if (!fillImage) return;
        fillImage.color = rainbowColor;
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class ScoreRangeData
{
    [Header("Options")]
    [Tooltip("This data will effect any scores hit with the point value between this one and the next on the list, if there isnt a next one, the range is unlimited")]
    public int anyAbove;
    public SuccessTypes hitType;
    [Header("Visual Effects")]
    public CamShakeData shakeData;
    public TextEffectData scoreTextEffectData;
}

public class ScoreManager : Singleton<ScoreManager>
{
    protected override bool DontDestoryOnLoad => false;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _comboText;

    [SerializeField]
    private List<ScoreRangeData> _scoreData;
    [SerializeField, Tooltip("If no data is recieved for a score this is the default it would pick")]
    private ScoreRangeData _falloffData;

    [SerializeField]
    private TextEffectData _comboStreakEffect;
    [SerializeField]
    private TextEffectData _comboLostEffect;

    [SerializeField]
    private int _mostPreciseScore;

    public UnityEvent<int> OnScoreChangedEvent;
    public UnityEvent<int> OnComboChangedEvent;

    private int _currentScore;

    private bool isPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPressed)
        {
            isPressed = true;
            AddScore(3);
        }
        else
        {
            isPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isPressed)
        {
            isPressed = true;
            AddScore(0);
        }
        else
        {
            isPressed = false;
        }
    }

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            OnScoreChanged(value);
        }
    }

    private int _currentCombo;
    public int CurrentCombo
    {
        get => _currentCombo;
        set
        {
            OnComboChanged(value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="precision">num between 0-1, 0 being a miss and 1 being a direct hit</param>
    public void SendThreshold(float precision)
    {
        AddScore((int)(_mostPreciseScore * precision));
    }

    public void AddScore(int amount)
    {
        if (amount > 0)
            CurrentCombo++;
        else if (CurrentCombo != 0)
            CurrentCombo = 0;

        CurrentScore += amount + CurrentCombo;
        
        RunScoreEffects(amount);
    }

    protected override void OnSingletonCreated()
    {
        _scoreData.Sort((a, b) => a.anyAbove > b.anyAbove ? 1 : -1);
    }

    public ScoreRangeData GetDataForThreshold(float precision)
    {
        return GetDataForScore((int)(_mostPreciseScore * precision));
    }

    public ScoreRangeData GetDataForScore(int score)
    {
        if (_scoreData.Count == 0) return _falloffData;

        for (int i = 0; i < _scoreData.Count; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex == _scoreData.Count) return _scoreData[i].anyAbove >= score ? _scoreData[i] : _falloffData;

            if (score >= _scoreData[i].anyAbove && score < _scoreData[nextIndex].anyAbove)
                return _scoreData[i];
        }

        return _falloffData;
    }

    private void OnScoreChanged(int score)
    {
        _currentScore = score;

        _scoreText.text = _currentScore.ToString();
    }

    private void RunScoreEffects(int scoreAdded)
    {        
        OnScoreChangedEvent?.Invoke(_currentScore);

        var currScoreData = GetDataForScore(scoreAdded);
        if (currScoreData == null) return;

        CameraShaker.Instance.Shake(currScoreData.shakeData);

        _scoreText.PlayTextEffect(currScoreData.scoreTextEffectData);
    }

    private void OnComboChanged(int value)
    {
        _currentCombo = value;
        
        _comboText.text = _currentCombo.ToString();
        _comboText.PlayTextEffect(_currentCombo == 0 ? _comboLostEffect : _comboStreakEffect);

        OnComboChangedEvent.Invoke(_currentCombo);
    }
}

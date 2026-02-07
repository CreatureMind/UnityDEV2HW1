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
    private InputActionReference _addScoreAction;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _comboText;

    [SerializeField]
    private List<ScoreRangeData> _scoreData;
    [SerializeField, Tooltip("If no data is recieved for a score this is the default it would pick")]
    private ScoreRangeData _falloffData;

    [SerializeField]
    private int _scoreToAdd = 1;

    [SerializeField]
    private TextEffectData _comboStreakEffect;
    [SerializeField]
    private TextEffectData _comboLostEffect;

    public UnityEvent<int> OnScoreChangedEvent;

    private int _currentScore;
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

    public void AddScore(int amount)
    {
        if (amount > 0)
            CurrentCombo++;
        else if (CurrentCombo != 0)
            CurrentCombo = 0;

        var currScoreData = GetDataForScore(amount);
        if (currScoreData == null) return;

        CameraShaker.Instance.Shake(currScoreData.shakeData);

        CurrentScore += amount + CurrentCombo;

        RunScoreEffects(amount);
    }

    protected override void OnSingletonCreated()
    {
        _addScoreAction.action.performed += _ => AddScore(_scoreToAdd);

        _scoreData.Sort((a, b) => a.anyAbove > b.anyAbove ? 1 : -1);
    }

    public ScoreRangeData GetDataForScore(int score)
    {
        if (_scoreData.Count == 0) return _falloffData;

        for (int i = 0; i < _scoreData.Count; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex == _scoreData.Count) return _scoreData[i].anyAbove <= score ? _scoreData[i] : _falloffData;

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

        _scoreText.PlayTextEffect(currScoreData.scoreTextEffectData);
    }

    private void OnComboChanged(int value)
    {
        _currentCombo = value;
        
        _comboText.text = _currentCombo.ToString();
        _comboText.PlayTextEffect(_currentCombo == 0 ? _comboLostEffect : _comboStreakEffect);
    }
}

using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UI.Base;

public class InGameUI : BaseMenu
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreNumText;
    
    [Header("Combo")]
    [SerializeField] private GameObject comboParent;
    [SerializeField] private Transform comboImage;
    [SerializeField] private Image numberPrefab;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private Transform numbersParent;
    [SerializeField] private float DOPunchScaleFactor;
    [SerializeField] private float DOPunchDuration;

    [Header("Compliments")]
    [SerializeField] private List<Compliments> compliments;
    
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    private readonly List<Image> _spawnedDigits = new();
    private Dictionary<Transform, Vector3> _originalScales = new();
    private bool isPaused;
    
    public static event Action OnMenuClosed;
    public static event Action OnMenuOpened;
    

    void OnEnable()
    {
        ScoreManager.Instance.OnScoreChangedEvent.AddListener(ChangeScore);
        ScoreManager.Instance.OnHitEvent.AddListener(ChangeCompliment);
        ScoreManager.Instance.OnComboChangedEvent.AddListener(ChangeCombo);
    }

    void OnDisable()
    {
        ScoreManager.Instance?.OnScoreChangedEvent.RemoveListener(ChangeScore);
        ScoreManager.Instance?.OnHitEvent.RemoveListener(ChangeCompliment);
        ScoreManager.Instance?.OnComboChangedEvent.RemoveListener(ChangeCombo);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        
        foreach (var comp in compliments)
        {
            _originalScales[comp.image] = comp.image.localScale;
        }
        
    }

    private void ChangeScore(int score)
    {
        scoreNumText.text = score.ToString();
    }

    private void ChangeCompliment(int score)
    {
        var scoreDate = ScoreManager.Instance.GetDataForScore(score);
        if (scoreDate == null) return;

        scoreNumText.PlayTextEffect(scoreDate.scoreTextEffectData);
        CameraShaker.Instance.Shake(scoreDate.shakeData);
        
        foreach (var comp in compliments)
        {
            var isMatch = comp.type == scoreDate.hitType;
            
            if (isMatch)
            {
                comp.image.gameObject.SetActive(true);

                comp.image.DOKill();
                comp.image.localScale = _originalScales[comp.image];
                comp.image.DOPunchScale(
                    Vector3.one * DOPunchScaleFactor,
                    scoreDate.shakeData.duration,
                    scoreDate.shakeData.vibrato
                );
            }
            else
            {
                comp.image.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeCombo(int combo)
    {
        comboParent.SetActive(combo != 0);

        var comboText = combo.ToString();
        
        while (_spawnedDigits.Count < comboText.Length)
        {
            var newDigit = Instantiate(numberPrefab, numbersParent, false);
            _spawnedDigits.Add(newDigit);
        }
        
        for (var i = 0; i < _spawnedDigits.Count; i++)
        {
            if (i < comboText.Length)
            {
                _spawnedDigits[i].gameObject.SetActive(true);
                _spawnedDigits[i].gameObject.transform.DOKill();
                _spawnedDigits[i].gameObject.transform.localScale = Vector3.one;
                _spawnedDigits[i].gameObject.transform.DOPunchScale(Vector3.one  * DOPunchScaleFactor, DOPunchDuration);
                var digitIndex = comboText[i] - '0'; 
                _spawnedDigits[i].sprite = numbers[digitIndex];
            }
            else
            {
                _spawnedDigits[i].gameObject.SetActive(false);
            }
        }
    }

    public override void ShowMenu()
    {
        canvasGroup.alpha = 1;
        if (isPaused)
        {
            isPaused = false;
        }
        else
        {
            OnMenuOpened?.Invoke();
            virtualCamera.Priority = 100;
        }
    }

    public override void HideMenu()
    {
        if (isPaused)
        {
            canvasGroup.alpha = 0.5f;
        }
        else
        {
            canvasGroup.alpha = 0f;
            OnMenuClosed?.Invoke();
            virtualCamera.Priority = 0;
        }
    }

    public override void EscapePressed()
    {
        isPaused = true;
        UI_Manager.Instance.SwapMenu(MenuType.PauseMenu);
    }
}

[System.Serializable]
public struct Compliments
{
    public SuccessTypes type;
    public Transform image;
}
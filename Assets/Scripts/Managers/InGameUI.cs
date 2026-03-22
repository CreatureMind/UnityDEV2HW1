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
    public static InGameUI Instance;
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
        ScoreManager.OnScoreChangedEvent += ChangeScore;
        ScoreManager.OnHitEvent += ChangeCompliment;
        ScoreManager.OnComboChangedEvent += ChangeCombo;
    }

    void OnDisable()
    {
        ScoreManager.OnScoreChangedEvent -= ChangeScore;
        ScoreManager.OnHitEvent -= ChangeCompliment;
        ScoreManager.OnComboChangedEvent -= ChangeCombo;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
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
    
    public void ResetInGameUI()
    {
        canvasGroup.alpha = 0f;
        OnMenuClosed?.Invoke();
        virtualCamera.Priority = 0;
    }

    public override void ShowMenu()
    {
        canvasGroup.alpha = 1;
        OnMenuOpened?.Invoke();
        virtualCamera.Priority = 100;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public override void HideMenu()
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public override void EscapePressed()
    {
        UI_Manager.Instance.SwapMenu(MenuType.PauseMenu);
    }

    public override void ForceStop()
    {
        ResetInGameUI();
    }
}

[System.Serializable]
public struct Compliments
{
    public SuccessTypes type;
    public Transform image;
}
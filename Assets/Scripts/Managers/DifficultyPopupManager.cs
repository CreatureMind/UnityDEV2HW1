using System;
using UI.Base;
using UnityEngine;
using DG.Tweening;
using UI;
using UnityEngine.UI;
using UI.Vinyl;


public class DifficultyPopupManager : BaseMenu
{
    [Header("General")]
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Buttons")]
    [SerializeField] private Toggle isMemeToggle;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button quitButton;
    
    [Header("Scene Loading")]
    [SerializeField] private int sceneToLoad;
    
    private SongSO _selectedSong;
    public static event Action<SongSO, bool, int> OnDifficultySelected;
    
    private void Awake()
    {
        Vinyl.OnSelectedSong += RegisterSong;
    }
    
    private void Start()
    {
        easyButton.onClick.AddListener(() => DifficultySelected(0));
        mediumButton.onClick.AddListener(() => DifficultySelected(1));
        hardButton.onClick.AddListener(() => DifficultySelected(2));
        quitButton.onClick.AddListener(EscapePressed);
        
        //HideMenu();
    }
    
    private void DifficultySelected(int difficulty)
    {
        var isMeme = isMemeToggle.isOn;
        rectTransform.ShakeAndHide(canvasGroup, HideMenu);
        OnDifficultySelected?.Invoke(_selectedSong, isMeme, difficulty);
        UI_Manager.Instance.SwapMenu(MenuType.InGameMenu);
    }
    
    private void RegisterSong(SongSO songSo)
    {
        _selectedSong = songSo;
    }

    public override void ShowMenu()
    {
        canvasGroup.alpha = 1; 
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(Vector3.one, 0.4f).SetUpdate(true).SetEase(Ease.InOutSine).OnComplete(() => Time.timeScale = 0);
    }

    public override void HideMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        Time.timeScale = 1;
    }

    public override void EscapePressed()
    {
        rectTransform.ShakeAndHide(canvasGroup, HideMenu);
        UI_Manager.Instance.SwapMenu(MenuType.SongSelectionMenu);
    }
}

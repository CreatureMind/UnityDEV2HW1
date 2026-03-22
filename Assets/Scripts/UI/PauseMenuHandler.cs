using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UI.Base;

namespace UI
{
    public class PauseMenuHandler: BaseMenu
    {
        [Header("Transform")]
        [SerializeField] private RectTransform menuTransform;
        
        [Header("Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        
        [Header("Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sFXVolumeSlider;
        
        [Header("Counter")]
        [SerializeField] private Image counterImage;
        [SerializeField] private Sprite[] numberSprites;
        
        [Header("Tween Settings")]
        [SerializeField] private float doShakeDuration = 0.2f;
        [SerializeField] private float doShakeStrength = 0.15f;
        [SerializeField] private int doShakeVibrato = 40;
        [SerializeField] private float doScaleDuration = 0.4f;

        private void Awake()
        {
            masterVolumeSlider.value = musicVolumeSlider.value = musicVolumeSlider.value = 1;
            resumeButton.onClick.AddListener(ResumeOnClick);
            mainMenuButton.onClick.AddListener(MainMenuOnClick);
        }

        private void Start()
        {
            
            masterVolumeSlider.onValueChanged.AddListener((value) => SoundMixer.instance.SetMasterVolume(value));
            musicVolumeSlider.onValueChanged.AddListener((value) => SoundMixer.instance.SetMusicVolume(value));
            sFXVolumeSlider.onValueChanged.AddListener((value) => SoundMixer.instance.SetSFXVolume(value));  
            
            
            HideMenu();
        }
        
        private void ResumeOnClick()
        {
            SoundManager.instance.PlayVFX("Punch");
            if(UI_Manager.Instance.LastMenuType == MenuType.MainMenu)
            {
                MainMenuOnClick();
                HideMenu();
            }
            else
            {
                StartCoroutine(CountDown());
            }
        }
        
        private void MainMenuOnClick()
        {
            SoundManager.instance.PlayVFX("Punch");
            Time.timeScale = 1;
            menuTransform.DOShakePosition(doShakeDuration, doShakeStrength, doShakeVibrato).SetUpdate(true).OnComplete(() => UI_Manager.Instance.SwapMenu(MenuType.MainMenu));
            
            if (UI_Manager.Instance.LastMenuType != MenuType.MainMenu)
            {
                UI_Manager.Instance.SwapMenu(MenuType.MainMenu);
            }
        }

        public override void ShowMenu()
        {
            Time.timeScale = 0;
            canvasGroup.alpha = 1; 
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            menuTransform.localScale = Vector3.zero;
            menuTransform.DOScale(Vector3.one, doScaleDuration).SetUpdate(true).SetEase(Ease.OutBack);
        }

        public override void HideMenu()
        {
            canvasGroup.alpha = 0; 
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            counterImage.gameObject.SetActive(false);
        }

        public override void EscapePressed()
        {
            SoundManager.instance.PlayVFX("DiscordLeaveSound");
            ResumeOnClick();
        }

        public override void ForceStop()
        {
            
        }

        private IEnumerator CountDown()
        {
            HideMenu();
            counterImage.gameObject.SetActive(true);
            
            for (var i = numberSprites.Length; i > 0; i--)
            {
                counterImage.sprite = numberSprites[i - 1];
                counterImage.PlayPopShakeFade();
                yield return new WaitForSecondsRealtime(1);
            }
            
            counterImage.gameObject.SetActive(false);
            Time.timeScale = 1;
            
            UI_Manager.Instance.ReturnToPreviousMenu();
        }
    }
}
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

        private void Awake()
        {
            
            masterVolumeSlider.value = musicVolumeSlider.value = musicVolumeSlider.value = 1;
            resumeButton.onClick.AddListener(ResumeOnClick);
            mainMenuButton.onClick.AddListener(MainMenuOnClick);
        }

        private void Start()
        {
            /*
            add these listeners to the sliders once the sound manager is implemented
            masterVolumeSlider.onValueChanged.AddListener((value) => SoundManager.instance.SetMasterVolume(value));
            musicVolumeSlider.onValueChanged.AddListener((value) => SoundManager.instance.SetMusicVolume(value));
            sFXVolumeSlider.onValueChanged.AddListener((value) => SoundManager.instance.SetSFXVolume(value));  
            */
            
            HideMenu();
        }
        
        private void ResumeOnClick()
        {
            StartCoroutine(CountDown());
        }

        private void MainMenuOnClick()
        {
            Time.timeScale = 1;
            menuTransform.DOShakePosition(0.2f, 15, 40).SetUpdate(true).OnComplete(() => menuTransform.localScale = Vector3.zero);
        }

        public override void ShowMenu()
        {
            canvasGroup.alpha = 1; 
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            menuTransform.localScale = Vector3.zero;
            menuTransform.DOScale(Vector3.one, 0.4f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(()=> Time.timeScale = 0);
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
            ResumeOnClick();
        }

        private IEnumerator CountDown()
        {
            HideMenu();
            counterImage.gameObject.SetActive(true);
            
            for (int i = numberSprites.Length; i > 0; i--)
            {
                counterImage.sprite = numberSprites[i - 1];
                counterImage.PlayPopShakeFade(scaleDuration: 0.4f, shakeDuration: 0.2f, fadeDuration: 0.5f, shakeStrength: 0.15f, fromZero: true, useUnscaledTime: true, deactivateOnComplete: false);
                yield return new WaitForSecondsRealtime(1);
            }
            
            counterImage.gameObject.SetActive(false);
            Time.timeScale = 1;
            
            UI_Manager.Instance.SwapMenu(MenuType.MainMenu);
        }

    }
    
    
    
}
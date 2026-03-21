using UI.Base;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace UI
{
    public class NewHighScore : BaseMenu
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text newScoreText;
        
        [Header("Buttons")]
        [SerializeField] private Button quitButton;
        
        void OnEnable()
        {
            ArrowsSpawner.OnSongEnded += LoadScore;
        }

        void OnDisable()
        {
            ArrowsSpawner.OnSongEnded -= LoadScore;
        }
        
        private void Start()
        {
            quitButton.onClick.AddListener(EscapePressed); 
        }
        
        public void LoadScore(string songID, int songDifficulty)
        {
            // SaveManager.LoadSaveData();
            // var currentScore = ScoreManager.Instance.CurrentScore;
            // var savedScore = SaveManager.saveData.songsData.ContainsKey()
            // currentScoreText.text = currentScore.ToString();
            // newScoreText.text = newScore.ToString();
            // ScoreManager.Instance.SaveHighScoreFor(songID, songDifficulty);
        }
        
        public override void ShowMenu()
        {
            Time.timeScale = 0;
            canvasGroup.alpha = 1; 
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.4f).SetUpdate(true).SetEase(Ease.InOutSine);
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
            transform.ShakeAndHide(canvasGroup, HideMenu);
            UI_Manager.Instance.SwapMenu(MenuType.SongSelectionMenu);
        }
    }
}

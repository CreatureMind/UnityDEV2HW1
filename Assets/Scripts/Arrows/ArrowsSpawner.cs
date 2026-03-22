using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Arrows
{
    public class ArrowsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject arrowPrefab;
    
        [Header("Spawn Place")]
        [SerializeField] private Transform spawnLeft;
        [SerializeField] private Transform spawnDown;
        [SerializeField] private Transform spawnUp;
        [SerializeField] private Transform spawnRight;
    
        [SerializeField, Range(1,3)] private float scrollTime; //GD number in seconds
        [SerializeField] private Transform goalCollider;
    
        private DdrPatternSO _currentPatternSo;
        private string _songID;
        private int _songDifficulty;
        private string _songName;
        private float _songLength;
        private int _bpm;
        private ArrowStep[] _steps;
        private float _perPatternDelay;
        private float _distanceToGoal;
        private float _arrowSpeed;
        private float _arrowSpeedScaleFactor;
    
        private bool _isPaused = false;
    
        private const float SIXTY_SECONDS = 60f;

        public static event Action OnTrackStarted;
        public static event Action<string,int> OnSongEnded;
        public static event Action OnBeat;

        void OnEnable()
        {
            DifficultyPopupManager.OnDifficultySelected += LoadSongData;
            InGameUI.OnMenuClosed += SongEndedUnload;
        }

        private void SongEndedUnload()
        {
            StopAllCoroutines();
        }

        void OnDisable()
        {
            DifficultyPopupManager.OnDifficultySelected -= LoadSongData;
            InGameUI.OnMenuClosed -= SongEndedUnload;
        }

        private void Start()
        {
            _distanceToGoal = (transform.position - goalCollider.position).magnitude;
            _arrowSpeed = _distanceToGoal / scrollTime;
        }
    
        private void LoadSongData(SongSO songSo, bool meme, int difficulty)
        {
            OnTrackStarted?.Invoke();
            Debug.Log($"{songSo} + {meme} + {difficulty}");
            var currentPattern = songSo.patterns[difficulty];
            
            _songID = songSo.songID;
            _songDifficulty = difficulty;
            _songName = songSo.songName;
            _songLength = !meme ? songSo.audioClipNormal.length : songSo.audioClipMeme.length;
            _perPatternDelay = currentPattern.delay;
            _arrowSpeedScaleFactor = currentPattern.arrowSpeedScaleFactor;
            _bpm = songSo.bpm;
            _steps = currentPattern.steps;
        
            StartCoroutine(StartTrack());
        }

        private void SpawnArrow(ArrowStep step)
        {
            var directions =  step.GetDirections();

            foreach (var direction in directions)
            {
                var arrow = Instantiate(arrowPrefab).GetComponent<Arrow>();
                arrow.direction = direction;
            
                switch (direction)
                {
                    case Direction.Left:
                        arrow.transform.SetParent(spawnLeft,false);
                        break;
                    case Direction.Down:
                        arrow.transform.SetParent(spawnDown, false);
                        break;
                    case Direction.Right:
                        arrow.transform.SetParent(spawnRight,  false);
                        break;
                    case Direction.Up:
                        arrow.transform.SetParent(spawnUp, false);
                        break;
                }
            
                arrow.transform.localPosition = Vector3.zero;
                arrow.speed = _arrowSpeed;
            }
        }

        private IEnumerator StartTrack()
        {
            var songLengthInSeconds =  _songLength - _perPatternDelay - _perPatternDelay;
            var secondsPerBeat = SIXTY_SECONDS / _bpm;

            SoundManager.instance.PlayMusic(_songName);
            yield return new WaitForSeconds(_perPatternDelay);

            while (songLengthInSeconds > 0)
            {
                foreach (var step in _steps)
                {
                    if (songLengthInSeconds <= 0) break;
                    SpawnArrow(step);
                    yield return new WaitForSeconds(secondsPerBeat);
                    _arrowSpeed += _arrowSpeedScaleFactor;
                    songLengthInSeconds -= secondsPerBeat;
                    OnBeat?.Invoke();
                }
            }
        
            SoundManager.instance.StopMusic(_songName);
            yield return new WaitForSeconds(_perPatternDelay);

            //ScoreManager.Instance.SaveHighScoreFor(songID, songDifficulty);
        
            OnSongEnded?.Invoke(_songID, _songDifficulty);
        }
    }

    public enum Direction
    {
        Left,
        Down,
        Up,
        Right,
    }
}

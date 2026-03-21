using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

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
    private string songID;
    private int songDifficulty;
    private string songName;
    private float songLength;
    private int BPM;
    private ArrowStep[] steps;
    private float perPatternDelay;
    private float distanceToGoal;
    private float arrowSpeed;
    private float arrowSpeedScaleFactor;
    
    private bool isPaused = false;

    public static event Action OnSongEnded;
    public static event Action OnBeat;

    void OnEnable()
    {
        //DifficultyPopupManager.OnSelected += LoadSongData;
    }

    void OnDisable()
    {
        //DifficultyPopupManager.OnSelected -= LoadSongData;
    }

    private void Start()
    {
        distanceToGoal = (transform.position - goalCollider.position).magnitude;
        arrowSpeed = distanceToGoal / scrollTime;
    }
    
    private void LoadSongData(SongSO songSo, bool meme, int difficulty)
    {
        var currentPattern = songSo.patterns[difficulty];
        
        songID = songSo.songID;
        songDifficulty = difficulty;
        songName = songSo.songName;
        songLength = !meme ? songSo.audioClipNormal.length : songSo.audioClipMeme.length;
        perPatternDelay = currentPattern.delay;
        arrowSpeedScaleFactor = currentPattern.arrowSpeedScaleFactor;
        BPM = songSo.bpm;
        steps = currentPattern.steps;
        
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
            arrow.speed = arrowSpeed;
        }
    }

    private IEnumerator StartTrack()
    {
        var songLengthInSeconds =  songLength - perPatternDelay - perPatternDelay;
        var secondsPerBeat = 60f / BPM;

        SoundManager.instance.PlayMusic(songName);
        yield return new WaitForSeconds(perPatternDelay);

        while (songLengthInSeconds > 0)
        {
            foreach (var step in steps)
            {
                if (songLengthInSeconds <= 0) break;
                SpawnArrow(step);
                yield return new WaitForSeconds(secondsPerBeat);
                arrowSpeed += arrowSpeedScaleFactor;
                songLengthInSeconds -= secondsPerBeat;
                OnBeat?.Invoke();
            }
        }
        
        SoundManager.instance.StopMusic(songName);
        yield return new WaitForSeconds(perPatternDelay);

        ScoreManager.Instance.SaveHighScoreFor(songID, songDifficulty);
        
        OnSongEnded?.Invoke();
    }
}

public enum Direction
{
    Left,
    Down,
    Up,
    Right,
}

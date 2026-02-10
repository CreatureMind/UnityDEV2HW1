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
    
    private DdrPattern currentPattern;
    private AudioClip song;
    private int BPM;
    private ArrowStep[] steps;
    private float preSongDelay;
    private float distanceToGoal;
    private float arrowSpeed;
    private float arrowSpeedScaleFactor;

    public static event Action<DdrPattern> OnSongEnded;

    void OnEnable()
    {
        DifficultyPopupManager.OnSelected += LoadSongData;
    }

    void OnDisable()
    {
        DifficultyPopupManager.OnSelected -= LoadSongData;
    }

    private void LoadSongData(DdrPattern pattern)
    {
        currentPattern = pattern;
        
        distanceToGoal = (transform.position - goalCollider.position).magnitude;
        arrowSpeed = distanceToGoal / scrollTime;
        
        song = SoundManager.instance.GetAudioClip(pattern.songName);
        preSongDelay = pattern.delay;
        arrowSpeedScaleFactor = pattern.arrowSpeedScaleFactor;
        BPM = pattern.bpm;
        steps = pattern.steps;
        
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
        var songLengthInSeconds =  song.length - preSongDelay - preSongDelay;
        var secondsPerBeat = 60f / BPM;

        yield return new WaitForSeconds(preSongDelay);

        while (songLengthInSeconds > 0)
        {
            foreach (var step in steps)
            {
                if (songLengthInSeconds <= 0) break;
                SpawnArrow(step);
                yield return new WaitForSeconds(secondsPerBeat);
                arrowSpeed += arrowSpeedScaleFactor;
                songLengthInSeconds -= secondsPerBeat;
            }
        }
        
        yield return new WaitForSeconds(preSongDelay);
        
        OnSongEnded?.Invoke(currentPattern);
    }
}

public enum Direction
{
    Left,
    Down,
    Up,
    Right
}

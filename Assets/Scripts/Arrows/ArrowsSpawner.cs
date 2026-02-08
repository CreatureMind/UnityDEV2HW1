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
    
    [SerializeField] private DdrPattern spawnPattern;
    [SerializeField, Range(1,3)] private float scrollTime; //GD number in seconds
    [SerializeField] private Transform goalCollider;
    
    private AudioClip song;
    private int BPM;
    private ArrowStep[] steps;
    public float distanceToGoal;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distanceToGoal = (transform.position - goalCollider.position).magnitude;
        LoadSongData(spawnPattern);
        
        StartCoroutine(StartTrack());
    }

    private void LoadSongData(DdrPattern pattern)
    {
        song = SoundManager.instance.GetAudioClip(pattern.songName);
        BPM = pattern.bpm;
        steps = pattern.steps;
    }

    // Update is called once per frame
    void Update()
    {

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
            arrow.speed = distanceToGoal / scrollTime;
        }
    }

    private IEnumerator StartTrack()
    {
        var songLengthInSeconds =  song.length;
        var beatsPerSeconds = BPM / 60f;
        var secondsPerBeat = 60f / BPM;
        var secondsPerMeasure = secondsPerBeat * 4;
        var totalMeasures = (songLengthInSeconds * BPM) / 240;

        foreach (var step in steps)
        {
            SpawnArrow(step);
            yield return new WaitForSeconds(secondsPerBeat);
        }

    }
}

public enum Direction
{
    Left,
    Down,
    Up,
    Right
}

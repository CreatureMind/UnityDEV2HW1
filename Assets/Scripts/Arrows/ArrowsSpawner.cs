using ScriptableObjects;
using UnityEngine;

public class ArrowsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnLeft;
    [SerializeField] private Transform spawnDown;
    [SerializeField] private Transform spawnUp;
    [SerializeField] private Transform spawnRight;
    [SerializeField] private DdrPattern spawnPattern;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var step in spawnPattern.steps)
        {
            SpawnArrow(step);
        }
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

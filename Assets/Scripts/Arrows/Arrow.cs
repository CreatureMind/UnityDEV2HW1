using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public bool isSuccessful;
    [HideInInspector] public Direction direction;
    
    [SerializeField] private Image image;
    [SerializeField] private float cycleDuration;
    
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
        // Default is Up
        switch (direction)
        {
            case Direction.Left:
                transform.Rotate(Vector3.forward, 90);
                break;
            case Direction.Down:
                transform.Rotate(Vector3.forward, 180);
                break;
            case Direction.Right:
                transform.Rotate(Vector3.forward, -90);
                break;
            case Direction.Up:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (image)
        {
            StartRainbowCycle();
        }
        else
        {
            Debug.LogError("No image found");
        }
    }

    void Update()
    {
        if (transform.position.y > cam.pixelRect.yMax)
            Destroy(gameObject);
        
        transform.localPosition += Vector3.up * (speed * Time.deltaTime);
    }
    
    void StartRainbowCycle()
    {
        DOVirtual.Float(0f, 1f, cycleDuration, UpdateColor)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void UpdateColor(float hueValue)
    {
        var rainbowColor = Color.HSVToRGB(hueValue, 1f, 1f);
        
        if (!image) return;
        image.color = rainbowColor;
    }
}

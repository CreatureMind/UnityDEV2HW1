using System;
using UnityEngine;

namespace Arrows
{
    public class Arrow : MonoBehaviour
    {
        [HideInInspector] public float speed;
        [HideInInspector] public bool isSuccessful;
        [HideInInspector] public Direction direction;
    
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
        }

        void Update()
        {
            if (transform.position.y > cam.pixelRect.yMax)
                Destroy(gameObject);
        
            transform.localPosition += Vector3.up * (speed * Time.deltaTime);
        }
    }
}

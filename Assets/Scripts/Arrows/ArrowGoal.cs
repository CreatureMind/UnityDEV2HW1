using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arrows
{
    [RequireComponent(typeof(Collider2D))]
    public class ArrowGoal : MonoBehaviour
    {
        public static event Action OnMiss;
        private float threshold;
        private readonly List<ArrowMovement> arrows = new();
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                arrows.Add(collision.GetComponent<ArrowMovement>());
            }
        }

        public void OnPressLeft(InputAction.CallbackContext context)
        {
            Debug.Log("PressLeft");
            if (context.performed)
                CheckThreshold(Direction.Left);
        }
        
        public void OnPressDown(InputAction.CallbackContext context)
        {
            Debug.Log("PressDown");
            if (context.performed)
                CheckThreshold(Direction.Down);
        }
        
        public void OnPressUp(InputAction.CallbackContext context)
        {
            Debug.Log("PressUp");
            if (context.performed)
                CheckThreshold(Direction.Up);
        }
        
        public void OnPressRight(InputAction.CallbackContext context)
        {
            Debug.Log("PressRight");
            if (context.performed)
                CheckThreshold(Direction.Right);
        }
        
        private void CheckThreshold(Direction direction)
        {
            if (arrows.Count == 0)
            {
                ScoreManager.Instance.SendThreshold(0);
                OnMiss?.Invoke();
            }
                
            /*switch (direction)
            {
                case Direction.Left:
                    if (arrows.Contains())
            }*/
        }
    }
}

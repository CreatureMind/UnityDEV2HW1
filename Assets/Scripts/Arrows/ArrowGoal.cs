using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arrows
{
    [RequireComponent(typeof(Collider2D))]
    public class ArrowGoal : MonoBehaviour
    {
        [SerializeField] private float threshold;
        
        public static event Action OnMiss;
        private readonly List<Arrow> arrows = new();
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.CompareTag("Arrow"))
            {
                arrows.Add(collision.GetComponent<Arrow>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                arrows.Remove(collision.GetComponent<Arrow>());
                ScoreManager.Instance.SendThreshold(0);
                OnMiss?.Invoke();
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

            HashSet<Arrow> missedArrows = new HashSet<Arrow>();
            HashSet<Arrow> successfulArrows = new HashSet<Arrow>();
            foreach (var arrow in arrows)
            {
                if(direction != arrow.direction)
                    continue;
                
                var distance = arrow.transform.position.y - transform.position.y;
                var unmapped = Mathf.Abs(distance) / threshold;
                if (unmapped > 1)
                {
                    ScoreManager.Instance.SendThreshold(0);
                    OnMiss?.Invoke();
                    missedArrows.Add(arrow);
                }
                else
                {
                    Debug.Log(1 - unmapped);
                    ScoreManager.Instance.SendThreshold(1 - unmapped);
                    successfulArrows.Add(arrow);
                }
            }

            foreach (var arrow in missedArrows)
            {
                arrows.Remove(arrow);
            }
            
            foreach (var arrow in successfulArrows)
            {
                arrows.Remove(arrow);
                Destroy(arrow.gameObject);
            }
        }
    }
}

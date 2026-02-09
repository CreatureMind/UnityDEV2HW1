using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows
{
    [RequireComponent(typeof(Collider2D))]
    public class ArrowGoal : MonoBehaviour
    {
        [SerializeField] private float threshold;
        
        public static event Action OnMiss;
        private readonly List<Arrow> _arrows = new();

        void OnEnable()
        {
            InputHandler.OnPress += CheckThreshold;
        }

        void OnDisable()
        {
            InputHandler.OnPress -= CheckThreshold;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var currentArrow = collision.GetComponent<Arrow>();
            if (currentArrow)
            {
                _arrows.Add(currentArrow);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var currentArrow = collision.GetComponent<Arrow>();
            
            if (currentArrow)
            {
                if(currentArrow.isSuccessful)
                    return;
                
                _arrows.Remove(currentArrow);
                ScoreManager.Instance.SendThreshold(0);
                OnMiss?.Invoke();
            }
        }
        
        private void CheckThreshold(Direction direction)
        {
            if (_arrows.Count == 0)
            {
                ScoreManager.Instance.SendThreshold(0);
                OnMiss?.Invoke();
            }

            HashSet<Arrow> arrowToRemove = new();
            foreach (var arrow in _arrows)
            {
                if(direction != arrow.direction)
                    continue;
                
                var distance = arrow.transform.position.y - transform.position.y;
                var unmapped = Mathf.Abs(distance) / threshold;
                if (unmapped > 1)
                {
                    ScoreManager.Instance.SendThreshold(0);
                    OnMiss?.Invoke();
                    arrowToRemove.Add(arrow);
                    arrow.isSuccessful = false;
                }
                else
                {
                    ScoreManager.Instance.SendThreshold(1 - unmapped);
                    arrowToRemove.Add(arrow);
                    arrow.isSuccessful = true;
                }
            }

            foreach (var arrow in arrowToRemove)
            {
                _arrows.Remove(arrow);
                if(arrow.isSuccessful)
                    Destroy(arrow.gameObject);
            }
        }
    }
}

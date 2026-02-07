using System.Collections.Generic;
using UnityEngine;

namespace Arrows
{
    [RequireComponent(typeof(Collider2D))]
    public class ArrowGoal : MonoBehaviour
    {
        private float okThreshold;
        private float greatThreshold;
        private float perfectThreshold;
        private float marvelThreshold;
        private List<ArrowMovement> arrows;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                arrows.Add(collision.GetComponent<ArrowMovement>());
            }
        }
    }
}

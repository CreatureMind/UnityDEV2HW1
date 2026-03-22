using System.Collections.Generic;
using Arrows;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace ScriptableObjects
{
    [System.Serializable]
    public struct ArrowStep {
        public bool left, down, up, right;

        public HashSet<Direction> GetDirections()
        {
            var directions = new HashSet<Direction>();
            
            if (left)
                directions.Add(Direction.Left);
            if (down)
                directions.Add(Direction.Down);
            if (up)
                directions.Add(Direction.Up);
            if (right)
                directions.Add(Direction.Right);
            
            return directions;
        }
    }

    [CreateAssetMenu(fileName = "NewPattern", menuName = "DDR/Pattern")]
    public class DdrPatternSO : ScriptableObject
    {
        public float delay;
        [Range(1f, 50f)] public float gain;
        [Range(1f, 50f)] public float penalty;
        public float arrowSpeedScaleFactor;
        public ArrowStep[] steps;
    }
}
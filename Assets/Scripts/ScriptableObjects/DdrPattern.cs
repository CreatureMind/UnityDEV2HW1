using System.Collections.Generic;
using UnityEngine;

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
    public class DdrPattern : ScriptableObject
    {
        public string songName;
        public int bpm;
        public float delay;
        public float arrowSpeedScaleFactor;
        public ArrowStep[] steps;
    }
}
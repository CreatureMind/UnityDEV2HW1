using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    public struct ArrowStep {
        public bool left, down, up, right; // Simple DDR layout
    }

    [CreateAssetMenu(fileName = "NewPattern", menuName = "DDR/Pattern")]
    public class DdrPattern : ScriptableObject
    {
        public string songName;
        public int bpm;
        public ArrowStep[] steps;
    }
}
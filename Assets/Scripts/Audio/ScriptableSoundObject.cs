using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Sound Collection", menuName = "Audio/Sound Collection")]
public class ScriptableSoundObject : ScriptableObject
{
    [Header("Sound Settings")]
    public List<Sound> VFX = new List<Sound>();
    public List<Sound> Music = new List<Sound>();
    
    [Header("Global Audio Settings")]
    public float masterVolume = 1f;
    
    // Helper methods
    public Sound GetSound(string soundName, List<Sound> sounds)
    {
        return sounds.Find(s => s.name == soundName);
    }
    
    public bool HasSound(string soundName, List<Sound> sounds)
    {
        return sounds.Exists(s => s.name == soundName);
    }
}
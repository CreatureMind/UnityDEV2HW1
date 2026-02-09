using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class CamShakeData
{
    public float duration;
    public float strength = 3f;
    public int vibrato = 10;
    public float randomness = 90f;
    public bool fadeOut = true;
    public ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;
}

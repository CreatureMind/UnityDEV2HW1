using UnityEngine;

[System.Serializable]
public class TextEffectData
{
    public float enterTime;
    public float exitTime;
    [Header("Scaling")]
    public float scaleExtra;
    public float scaleMulti = 1;
    [Header("Coloring")]
    public bool useColor;
    public Color pulseColor = Color.white;
    [Header("Shake")]
    public bool useShake;
    public CamShakeData shakeData;
}

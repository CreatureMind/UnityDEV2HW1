using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Rainbow : MonoBehaviour
{
    [SerializeField] private float rainbowCycleDuration;
    [SerializeField] private Graphic visual;

    void Start()
    {
        DOVirtual.Float(0f, 1f, rainbowCycleDuration, UpdateColor)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void UpdateColor(float hueValue)
    {
        var rainbowColor = Color.HSVToRGB(hueValue, 1f, 1f);
        
        if (!visual) return;
        visual.color = rainbowColor;
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Rainbow : MonoBehaviour
{
    [SerializeField] private float rainbowCycleDuration;
    [SerializeField] private Graphic uiImage;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        DOVirtual.Float(0f, 1f, rainbowCycleDuration, UpdateColor)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void UpdateColor(float hueValue)
    {
        var rainbowColor = Color.HSVToRGB(hueValue, 1f, 1f);
        
        if (!uiImage && !spriteRenderer) return;
        if (spriteRenderer) spriteRenderer.color = rainbowColor;
        if (uiImage) uiImage.color = rainbowColor;
    }
}

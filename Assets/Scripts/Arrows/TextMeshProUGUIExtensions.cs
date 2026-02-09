using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class TMP_TextExtensions
{
    private static readonly Dictionary<TMP_Text, Vector3> BaseScales = new();
    private static readonly Dictionary<TMP_Text, Color> BaseColors = new();
    private static readonly Dictionary<TMP_Text, Vector3> BasePositions = new();

    public static void PlayTextEffect(this TMP_Text text, TextEffectData effectData)
    {
        var t = text.transform;

        if (!BaseScales.TryGetValue(text, out var baseScale))
        {
            BaseScales[text] = baseScale = t.localScale;
            BasePositions[text] = t.localPosition;
            BaseColors[text] = text.color;
        }

        t.DOKill(true);
        text.DOKill(true);

        float targetScale = baseScale.x * effectData.scaleMulti + effectData.scaleExtra;

        var seq = DOTween.Sequence();

        seq.Append(t.DOScale(targetScale, effectData.enterTime));
        seq.Append(t.DOScale(baseScale, effectData.exitTime));

        if (effectData.useColor)
        {
            seq.Join(text.DOColor(effectData.pulseColor, effectData.enterTime));
            seq.Append(text.DOColor(BaseColors[text], effectData.exitTime));
        }

        if (effectData.useShake)
        {
            t.localPosition = BasePositions[text];

            t.DOShakePosition(
                effectData.shakeData.duration,
                effectData.shakeData.strength,
                effectData.shakeData.vibrato,
                effectData.shakeData.randomness,
                true,
                effectData.shakeData.fadeOut,
                effectData.shakeData.randomnessMode
            );
        }

        seq.SetLink(text.gameObject);
    }
}

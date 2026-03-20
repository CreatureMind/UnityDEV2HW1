using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public static class ImageTweenExtensions
    {
        /// <summary>
        /// Plays a pop (scale to 1) then shake and fade-out sequence on the provided Image.
        /// Returns the created Sequence so the caller can reuse/control it.
        /// </summary>
        public static Sequence PlayPopShakeFade(this Image image,
                                               float scaleDuration = 0.4f,
                                               float shakeDuration = 0.2f,
                                               float fadeDuration = 0.5f,
                                               float shakeStrength = 0.15f,
                                               bool fromZero = true,
                                               bool useUnscaledTime = true,
                                               bool deactivateOnComplete = false,
                                               Action onComplete = null)
        {
            if (!image) return null;

            // Kill existing tweens targeting this image/rectTransform
            image.DOKill();
            var rt = image.rectTransform;
            rt.DOKill();

            // Prepare start state
            if (fromZero) rt.localScale = Vector3.zero;
            var col = image.color;
            col.a = 1f;
            image.color = col;
            image.gameObject.SetActive(true);

            // Build sequence: scale -> shake (with fade joined)
            var seq = DOTween.Sequence();
            seq.SetUpdate(useUnscaledTime);

            seq.Append(rt.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).SetUpdate(useUnscaledTime));
            // shake and fade concurrently
            seq.Append(rt.DOShakeScale(shakeDuration, shakeStrength, vibrato: 10, randomness: 90, fadeOut: true)
                         .SetUpdate(useUnscaledTime));
            seq.Join(image.DOFade(0f, fadeDuration).SetUpdate(useUnscaledTime));

            seq.OnComplete(() =>
            {
                if (deactivateOnComplete) image.gameObject.SetActive(false);
                onComplete?.Invoke();
            });

            seq.Play();
            return seq;
        }
    }
}
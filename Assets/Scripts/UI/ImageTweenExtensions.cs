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
        
        
        public static Sequence ShakeAndHide(this
            Transform target,
            CanvasGroup canvasGroup = null,
            Action onComplete = null,
            float duration = 0.2f,
            float strength = 15f,
            int vibrato = 40,
            float randomness = 90f,
            bool useUnscaledTime = true
            )
        {
            if (target == null) return null;

            // kill existing tweens to avoid conflicting animations
            target.DOKill();
            if (canvasGroup != null) canvasGroup.DOKill();

            // ensure visible before anim
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }

            var seq = DOTween.Sequence();
            seq.SetUpdate(useUnscaledTime);

            // shake position, optionally fade canvasGroup concurrently
            seq.Append(target.DOShakePosition(duration, strength, vibrato, randomness).SetUpdate(useUnscaledTime));
            if (canvasGroup != null)
                seq.Join(canvasGroup.DOFade(0f, duration).SetUpdate(useUnscaledTime));

            seq.OnComplete(() =>
            {
                target.localScale = Vector3.zero;
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.interactable = false;
                }
                onComplete?.Invoke();
            });

            seq.Play();
            return seq;
        }
    }
}
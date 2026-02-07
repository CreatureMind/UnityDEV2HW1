using System;
using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int layerIndex = 1;
    [SerializeField] private float initialWeight = 1f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float holdDuration = 0f;

    private Coroutine fadeCoroutine;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tomato")) return;
        TomatoHit();
    }
    
    [ContextMenu("Debug Tomato Hit")]
    private void TomatoHit()
    {
        if (animator == null) return;

        animator.SetLayerWeight(layerIndex, initialWeight);
        animator.SetTrigger("GotHit");

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        if (holdDuration > 0f) yield return new WaitForSeconds(holdDuration);
        yield return FadeTo(0f, fadeOutDuration);
        fadeCoroutine = null;
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        if (!animator)
            yield break;

        if (duration <= 0f)
        {
            animator.SetLayerWeight(layerIndex, target);
            yield break;
        }

        var start = animator.GetLayerWeight(layerIndex);
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float w = Mathf.Lerp(start, target, t);
            animator.SetLayerWeight(layerIndex, w);
            yield return null;
        }
        animator.SetLayerWeight(layerIndex, target);
    }

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
}

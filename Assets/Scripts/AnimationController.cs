using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Hit Reaction Settings")] [SerializeField]
    private int layerIndex = 1;

    [SerializeField] private float initialWeight = 1f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float holdDuration = 0f;

    #region Contsants

    private static readonly int IsNormalDancing = Animator.StringToHash("IsNormalDancing");
    private static readonly int NormalDancingBlend = Animator.StringToHash("NormalDancingBlend");
    private static readonly int IsCrazyDancing = Animator.StringToHash("IsCrazyDancing");
    private static readonly int IsDancing = Animator.StringToHash("IsDancing");
    
    private const int ComboNormalStart = 10;
    private const int ComboNormalEnd = 29;
    private const int ComboCrazyStart = 30; 

    #endregion
    
    

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        TomatoCollision.OnTomatoHit += TomatoHitPlayer;
        ScoreManager.Instance.OnComboChangedEvent.AddListener(OnComboChanged);
    }

    private void OnComboChanged(int combo)
    {
        switch (combo)
        {
            case 0:
                // Reset all dance states   
                animator.SetBool(IsNormalDancing, false);
                animator.SetFloat(NormalDancingBlend, 0);
                animator.SetBool(IsCrazyDancing, false);
                break;

            case 1:
                animator.SetBool(IsDancing, true);
                break;
            case ComboNormalStart:
                animator.SetBool(IsNormalDancing, true);
                break;
            case <ComboNormalEnd:
                var scalarForNormalizing = ComboNormalEnd - ComboNormalStart;
                var normalizedCombo = (combo - ComboNormalStart) / scalarForNormalizing; // Normalize combo to range [0, 1]
                animator.SetFloat(NormalDancingBlend, normalizedCombo);
                break;
            case ComboCrazyStart:
                animator.SetBool(IsCrazyDancing, true);
                break;
        }
    }


    [ContextMenu("Debug Tomato Hit")]
    private void TomatoHitPlayer()
    {
        if (!animator) return;

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
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            var weight = Mathf.Lerp(start, target, time);
            animator.SetLayerWeight(layerIndex, weight);
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
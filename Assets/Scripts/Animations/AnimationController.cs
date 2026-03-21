using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class AnimationController : MonoBehaviour
{
    [SerializeField] private SelectedCharacterParent charactersHolder;
    public SelectedCharacterParent CharactersHolder => charactersHolder;

    private Animator _anima;
    private Animator Anima {
        get
        {
            var characterObj = charactersHolder.SelectedCharacterObj;

            if (characterObj == null)
                return null;

            if (_anima != null && _anima.gameObject == characterObj) return _anima;

            _anima = characterObj.GetComponent<Animator>();
            return _anima;
        }
    }

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

    private void Start()
    {
        TomatoCollision.OnTomatoHit += TomatoHitPlayer;
        ScoreManager.Instance?.OnComboChangedEvent.AddListener(OnComboChanged);
    }

    private void OnComboChanged(int combo)
    {
        switch (combo)
        {
            case 0:
                // Reset all dance states   
                Anima.SetBool(IsNormalDancing, false);
                Anima.SetFloat(NormalDancingBlend, 0);
                Anima.SetBool(IsCrazyDancing, false);
                break;
            case 1:
                Anima.SetBool(IsDancing, true);
                break;
            case ComboNormalStart:
                Anima.SetBool(IsNormalDancing, true);
                break;
            case ComboCrazyStart:
                Anima.SetBool(IsCrazyDancing, true);
                break;
        }
        
        if (combo >= ComboNormalStart && combo < ComboCrazyStart)
        {
            float blend = (float)(combo - ComboNormalStart) / (ComboNormalEnd - ComboNormalStart);
            Anima.SetFloat(NormalDancingBlend, blend);
        }
    }


    [ContextMenu("Debug Tomato Hit")]
    private void TomatoHitPlayer()
    {
        if (!Anima) return;

        Anima.SetLayerWeight(layerIndex, initialWeight);
        Anima.SetTrigger("GotHit");

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
        if (!Anima)
            yield break;

        if (duration <= 0f)
        {
            Anima.SetLayerWeight(layerIndex, target);
            yield break;
        }

        var start = Anima.GetLayerWeight(layerIndex);
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            var weight = Mathf.Lerp(start, target, time);
            Anima.SetLayerWeight(layerIndex, weight);
            yield return null;
        }

        Anima.SetLayerWeight(layerIndex, target);
    }
}
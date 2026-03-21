using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct CharacterWithAnimators
{
    public Character characterOptions;
    public Animator characterAnimator;

    public override bool Equals(object obj)
    {
        if (obj is not CharacterWithAnimators character) return false;

        return characterOptions == character.characterOptions;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


public class AnimationController : MonoBehaviour
{
    [SerializeField] private CharacterWithAnimators[] characters;
    [SerializeField] private Character selectedCharacter;
    private Animator Anima {
        get
        {
            foreach (var character in characters)
            {
                if (character.Equals(selectedCharacter))
                    return character.characterAnimator;
            }

            return null;
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

    void Awake()
    {
        SaveManager.LoadSaveData();

        foreach (var character in characters)
        {
            if (character.characterOptions.characterID != SaveManager.saveData.selectedCharacterID)
                continue;

            selectedCharacter = character.characterOptions;
            break;
        }
    }

    private void Start()
    {
        TomatoCollision.OnTomatoHit += TomatoHitPlayer;
        ScoreManager.Instance?.OnComboChangedEvent.AddListener(OnComboChanged);

        DisableInvalidCharacters();
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        DisableInvalidCharacters();
    }

    public Character GetSelectedCharacter() => selectedCharacter;

    public Character[] GetCharacters() => characters.Select(x => x.characterOptions).ToArray();

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

    void DisableInvalidCharacters()
    {
        foreach (var character in characters)
        {
            character.characterAnimator.gameObject.SetActive(character.characterOptions == selectedCharacter);
        }
    }
}
using UnityEngine;
using System.Collections;

public class BartenderNPCAnimations : MonoBehaviour
{
    
    private Animator _animator;
    
    void Start()
    {
        SoundManager.instance.StopAllSounds();
        SoundManager.instance.PlayVFX("BarAmbiance");
        SoundManager.instance.PlayVFX("BarMusic");
        _animator = GetComponent<Animator>();
        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            if (animationTrigger.Length > 0)
            {
                string animationTrigger = null;

                _animator.SetTrigger(animationTrigger);

                Debug.Log($"NPC {gameObject.name} is now doing: {animationTrigger}");
            }
        }
    }
}

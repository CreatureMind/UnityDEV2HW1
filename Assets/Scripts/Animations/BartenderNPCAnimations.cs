using UnityEngine;
using System.Collections;

public class BartenderNPCAnimations : MonoBehaviour
{
    
    private Animator _animator;

    [Header("Settings")] public float minWaitTime = 5f;
    public float maxWaitTime = 15f;

        public string animationTrigger =  "Bartending" ;

    void Start()
    {
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

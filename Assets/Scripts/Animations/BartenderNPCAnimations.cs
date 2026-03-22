using UnityEngine;
using System.Collections;

public class BartenderNPCAnimations : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minWaitTime = 5f;
    [SerializeField] private float maxWaitTime = 15f;
    [SerializeField] private string animationTrigger =  "Bartending" ;

    private Animator _animator;
    
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

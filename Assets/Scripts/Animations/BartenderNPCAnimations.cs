using UnityEngine;
using System.Collections;

public class BartenderNPCAnimations : MonoBehaviour
{
    
    private Animator _animator;

    [Header("Settings")] public float minWaitTime = 5f;
    public float maxWaitTime = 15f;

    // Array of trigger names you set up in the Animator
        public string[] animationTrigger = { "Bartending" };

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        while (true)
        {
            // 1. Wait for a random duration
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 2. Pick a random animation from your list
            if (animationTrigger.Length > 0)
            {
                int randomIndex = Random.Range(0, animationTrigger.Length);
                string selectedTrigger = animationTrigger[randomIndex];

                // 3. Fire the trigger in the Animator
                _animator.SetTrigger(selectedTrigger);

                Debug.Log($"NPC {gameObject.name} is now doing: {selectedTrigger}");
            }
        }
    }
}

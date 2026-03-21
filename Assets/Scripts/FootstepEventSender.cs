using UnityEngine;
using UnityEngine.Events;

public class FootstepEventSender : MonoBehaviour
{
    [SerializeField] private UnityEvent<AnimationEvent> OnFootstepDetected;
    public void OnFootstep(AnimationEvent animationEvent)
    {
        OnFootstepDetected.Invoke(animationEvent);
    }
}

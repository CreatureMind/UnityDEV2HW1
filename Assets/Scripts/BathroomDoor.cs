using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BathroomDoor : MonoBehaviour
{
    [SerializeField] private TweenOptions<Vector3> openTween;
    [SerializeField] private TweenOptions<Vector3> closeTween;

    public event UnityAction OnDoorClosed;
    public event UnityAction OnDoorOpened;

    public void OpenDoor()
    {
        DOTween.Kill(gameObject);
        DOTween.Sequence()
            .Append(openTween.Apply(transform.DOLocalRotate))
            .AppendCallback(() => OnDoorOpened?.Invoke())
            .SetLink(gameObject);
    }
    public void CloseDoor()
    {
        DOTween.Kill(gameObject);
        DOTween.Sequence()
            .Append(closeTween.Apply(transform.DOLocalRotate))
            .AppendCallback(() => OnDoorClosed?.Invoke())
            .SetLink(gameObject);
    }
}

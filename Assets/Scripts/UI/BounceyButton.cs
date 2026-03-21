using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BounceyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, ICancelHandler
{
    private Vector3 _originalScale;
    [SerializeField] private TweenOptions<Vector3> _pressedTween;
    [SerializeField] private TweenOptions _exitPressTween;

    [SerializeField] private UnityEvent onClick;

    private bool isHolding = false;

    void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RunPressedTween();
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RunExitTween();
        if (isHolding)
            onClick.Invoke();
        isHolding = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RunExitTween();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHolding)
        {
            RunPressedTween();
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        isHolding = false;
        RunExitTween();
    }

    void RunExitTween()
    {
        transform.DOKill();
        _exitPressTween.Apply(transform.DOScale, _originalScale);
    }

    void RunPressedTween()
    {
        transform.DOKill();
        _pressedTween.Apply(transform.DOScale);
    }
}

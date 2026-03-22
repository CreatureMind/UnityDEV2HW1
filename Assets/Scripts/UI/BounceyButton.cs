using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BounceyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, ICancelHandler
{
    [SerializeField] private TweenOptions<Vector3> pressedTween;
    [SerializeField] private TweenOptions exitPressTween;
    [SerializeField] private UnityEvent onClick;

    private Vector3 _originalScale;
    private bool _isHolding = false;

    void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RunPressedTween();
        _isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RunExitTween();
        if (_isHolding)
            onClick.Invoke();
        _isHolding = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RunExitTween();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isHolding)
        {
            RunPressedTween();
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        _isHolding = false;
        RunExitTween();
    }

    void RunExitTween()
    {
        transform.DOKill();
        exitPressTween.Apply(transform.DOScale, _originalScale);
    }

    void RunPressedTween()
    {
        transform.DOKill();
        pressedTween.Apply(transform.DOScale);
    }
}

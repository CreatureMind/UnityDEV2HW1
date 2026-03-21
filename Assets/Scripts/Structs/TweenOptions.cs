using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TweenOptions
{
    public float duration;
    public Ease easing;
    public bool isRelative;

    public UnityEvent OnComplete;
    public UnityEvent OnKill;

    public Tweener Apply<T>(Func<T, float, bool, TweenerCore<T, T, VectorOptions>> tweenCreator, T value) => Setup(tweenCreator(value, duration, false));

    public Tweener Apply<T>(Func<T, float, RotateMode, TweenerCore<Quaternion, Vector3, QuaternionOptions>> tweenCreator, T value)
        => Setup(tweenCreator(value, duration, RotateMode.Fast));

    public Tweener Apply<T>(Func<T, float, Tweener> tweenCreator, T value) => Setup(tweenCreator(value, duration));

    private Tweener Setup(Tweener t)
    {
        return t.SetEase(easing)
            .SetRelative(isRelative)
            .OnComplete(() => OnComplete.Invoke())
            .OnKill(() => OnKill.Invoke());
    }
}


[System.Serializable]
public class TweenOptions<T> : TweenOptions
{
    [Space]
    public T destination;

    public Tweener Apply(Func<T, float, bool, TweenerCore<T, T, VectorOptions>> tweenCreator) => Apply(tweenCreator, destination);

    public Tweener Apply(Func<T, float, RotateMode, TweenerCore<Quaternion, Vector3, QuaternionOptions>> tweenCreator) => Apply(tweenCreator, destination);

    public Tweener Apply(Func<T, float, Tweener> tweenCreator) => Apply(tweenCreator, destination);
}
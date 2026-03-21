
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public static class TweenerCoreExtensions
{
    public static TweenerCore<T, V, TPlugOptions> SetTweenOptions<T, V, TPlugOptions>(this TweenerCore<T, V, TPlugOptions> tween, TweenOptions options) where TPlugOptions : struct, IPlugOptions
    {
        tween.SetEase(options.easing);
        tween.SetRelative(options.isRelative);
        return tween;
    }

    public static TweenerCore<T, V, TPlugOptions> SetTweenOptions<T, V, TPlugOptions>(this TweenerCore<T, V, TPlugOptions> tween, TweenOptions<V> options) where TPlugOptions : struct, IPlugOptions
    {
        tween.SetEase(options.easing);
        tween.SetRelative(options.isRelative);
        return tween;
    }
}

using DG.Tweening;
using UnityEngine;

public class CameraShaker : Singleton<CameraShaker>
{
    private Camera _shakeCamera;
    public Camera ShakeCamera
    {
        get
        {
            if (_shakeCamera == null)
                _shakeCamera = Camera.main;

            return _shakeCamera;
        }
        set => _shakeCamera = value;
    }

    public void Shake(CamShakeData shakeData)
    {
        if (ShakeCamera == null || shakeData.duration == 0) return;

        ShakeCamera.DOComplete();
        ShakeCamera.DOShakePosition(
            shakeData.duration,
            shakeData.strength,
            shakeData.vibrato,
            shakeData.randomness,
            shakeData.fadeOut,
            shakeData.randomnessMode
        );
    }
}

using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoSingleton<CameraShake>
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    private bool _shaking;

    protected override void Awake()
    {
        base.Awake();
        _perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_AmplitudeGain = 0f;
    }

    public void Shake(float intensity)
    {
        if (_shaking) return;

        _shaking = true;
        
        Transition(intensity, 0.1f)
            .OnComplete(() =>
            {
                Transition(0f, 0.1f);
                _shaking = false;
            });
    }

    private Tween Transition(float intensity, float duration)
    {
        return DOVirtual.Float(_perlin.m_AmplitudeGain, intensity, duration, (v) => _perlin.m_AmplitudeGain = v);
    }
}
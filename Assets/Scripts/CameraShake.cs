using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoSingleton<CameraShake>
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    [SerializeField, NoiseSettingsProperty]
    private NoiseSettings idleProfile;
    
    [SerializeField, NoiseSettingsProperty]
    private NoiseSettings shakeProfile;
    
    private CinemachineBasicMultiChannelPerlin _perlin;

    private bool _shaking;

    private float _defaultAmplitude;

    protected override void Awake()
    {
        base.Awake();
        _perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_NoiseProfile = idleProfile;
        _defaultAmplitude = _perlin.m_AmplitudeGain;
    }

    public void Shake(float intensity, float duration = 1f)
    {
        if (_shaking) return;

        _shaking = true;

        _perlin.m_NoiseProfile = shakeProfile;
        
        Transition(intensity, 0.1f)
            .OnComplete(() =>
            {
                Transition(_defaultAmplitude, 0.1f)
                    .SetDelay(duration);

                _perlin.m_NoiseProfile = idleProfile;
                
                _shaking = false;
            });
    }

    private Tween Transition(float intensity, float duration)
    {
        return DOVirtual.Float(_perlin.m_AmplitudeGain, intensity, duration, (v) => _perlin.m_AmplitudeGain = v);
    }
}
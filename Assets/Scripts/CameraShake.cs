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

    [SerializeField] private float shakeFrequency;
    
    private CinemachineBasicMultiChannelPerlin _perlin;

    private bool _shaking;

    private float _defaultAmplitude;
    private float _defaultFrequency;

    protected override void Awake()
    {
        base.Awake();
        _perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_NoiseProfile = idleProfile;
        _defaultAmplitude = _perlin.m_AmplitudeGain;
        _defaultFrequency = _perlin.m_FrequencyGain;
    }

    public void Shake(float intensity, float duration = 1f)
    {
        if (_shaking) return;

        duration = Mathf.Max(duration, 0.8f);
        
        _shaking = true;

        _perlin.m_NoiseProfile = shakeProfile;
        _perlin.m_FrequencyGain = shakeFrequency;
        
        Transition(intensity, 0.2f)
            .OnComplete(() =>
            {
                Transition(_defaultAmplitude, 0.2f)
                    .SetDelay(duration)
                    .OnComplete(() =>
                    {
                        _perlin.m_NoiseProfile = idleProfile;
                        _perlin.m_FrequencyGain = _defaultFrequency;
                
                        _shaking = false;

                    });
            });
    }

    private Tween Transition(float intensity, float duration)
    {
        return DOVirtual.Float(_perlin.m_AmplitudeGain, intensity, duration, (v) => _perlin.m_AmplitudeGain = v);
    }
}
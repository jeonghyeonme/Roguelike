using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineCamera ccam;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;

    [Header("Shake Settings")]
    public float defaultAmplitude = 2f;
    public float defaultFrequency = 2f;
    public float defaultDuration = 0.3f;

    void Awake()
    {
        ccam = GetComponent<CinemachineCamera>();
        if (ccam == null)
        {
            Debug.LogError("[CameraShake] CinemachineVirtualCamera가 없습니다.");
            return;
        }

        noise = ccam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogError("[CameraShake] MultiChannelPerlin 컴포넌트가 없습니다.");
        }
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                StopShake();
            }
        }
    }

    public void Shake()
    {
        Shake(defaultAmplitude, defaultFrequency, defaultDuration);
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        if (noise == null) return;

        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;
        shakeTimer = duration;

        Debug.Log($"[CameraShake] 흔들림 시작: 강도 {amplitude}, 주파수 {frequency}, 지속 {duration}");
    }

    private void StopShake()
    {
        if (noise == null) return;

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;

        Debug.Log("[CameraShake] 흔들림 종료됨");
    }
}
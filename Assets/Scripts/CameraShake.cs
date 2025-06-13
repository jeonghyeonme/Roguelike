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
            Debug.LogError("[CameraShake] CinemachineVirtualCamera�� �����ϴ�.");
            return;
        }

        noise = ccam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogError("[CameraShake] MultiChannelPerlin ������Ʈ�� �����ϴ�.");
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

        Debug.Log($"[CameraShake] ��鸲 ����: ���� {amplitude}, ���ļ� {frequency}, ���� {duration}");
    }

    private void StopShake()
    {
        if (noise == null) return;

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;

        Debug.Log("[CameraShake] ��鸲 �����");
    }
}
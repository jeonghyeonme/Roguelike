using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    private float shakeTimer;

    [Header("Shake Settings")]
    public float shakeIntensity = 3f;
    public float shakeDuration = 0.2f;

    void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
                StopShake();
        }
    }

    public void Shake()
    {
        var noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeIntensity;
            noise.m_FrequencyGain = 2f;
            shakeTimer = shakeDuration;
        }
    }

    private void StopShake()
    {
        var noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
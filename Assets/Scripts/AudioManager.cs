using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource uiClickSource;
    public AudioSource attackSource;
    public AudioSource swingSource;
    public AudioSource dashSource;
    public AudioSource hitSource;
    public AudioSource GameClearSource;
    public AudioSource GameOverSource;

    [Header("Audio Clips")]
    public AudioClip uiClickClip;
    public AudioClip slashClip;
    public AudioClip swingClip;
    public AudioClip dashClip;
    public AudioClip hitClip;
    public AudioClip GameClearClip;
    public AudioClip GameOverClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // UI Ŭ��
    public void PlayUIClick()
    {
        uiClickSource.PlayOneShot(uiClickClip);
    }

    // ���� �ֵθ���
    public void PlaySlash()
    {
        attackSource.PlayOneShot(slashClip);
    }

    public void PlaySwing()
    {
        hitSource.PlayOneShot(swingClip);
    }

    // ���
    public void PlayDash()
    {
        dashSource.PlayOneShot(dashClip);
    }

    // �ǰ�
    public void PlayHit()
    {
        hitSource.PlayOneShot(hitClip);
    }

    public void PlayGameClear()
    {
        hitSource.PlayOneShot(GameClearClip);
    }

    public void PlayGameOver()
    {
        hitSource.PlayOneShot(GameOverClip);
    }
}
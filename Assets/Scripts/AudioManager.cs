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

    // UI 클릭
    public void PlayUIClick()
    {
        uiClickSource.PlayOneShot(uiClickClip);
    }

    // 무기 휘두르기
    public void PlaySlash()
    {
        attackSource.PlayOneShot(slashClip);
    }

    public void PlaySwing()
    {
        hitSource.PlayOneShot(swingClip);
    }

    // 대시
    public void PlayDash()
    {
        dashSource.PlayOneShot(dashClip);
    }

    // 피격
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
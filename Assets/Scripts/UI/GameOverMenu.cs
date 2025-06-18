using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("Game Over 패널")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("배경 음악 AudioSource")]
    public AudioSource backgroundMusic;

    [Header("플레이어 조작 스크립트들")]
    public MonoBehaviour[] playerControlScripts;

    private bool isMenuShown = false;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            UnityEngine.Debug.LogWarning("[GameOverMenu] GameOverPanel이 연결되지 않았습니다.");

        isMenuShown = false;
    }

    public void Show()
    {
        if (isMenuShown || gameOverPanel == null) return;

        gameOverPanel.SetActive(true);
        backgroundMusic.Stop();
        AudioManager.Instance.PlayGameOver();
        Time.timeScale = 0f;
        isMenuShown = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (var script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlayUIClick();
        Time.timeScale = 1f;

        foreach (var script in playerControlScripts)
        {
            if (script != null)
                script.enabled = true;
        }

        // ❌ 커서 상태는 변경하지 않음 (메인 메뉴 씬에서 마우스 UI 계속 사용 가능)

        SceneManager.LoadScene("MainMenuScene");
    }
}
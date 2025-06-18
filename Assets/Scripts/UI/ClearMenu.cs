using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearMenu : MonoBehaviour
{
    [Header("클리어 메뉴 패널")]
    [Tooltip("클리어 시 활성화할 UI 패널")]
    public GameObject clearMenuPanel;

    [Header("배경 음악 AudioSource")]
    public AudioSource backgroundMusic;

    [Header("플레이어 조작 스크립트들")]
    [Tooltip("조작을 차단할 플레이어 제어 스크립트들")]
    public MonoBehaviour[] playerControlScripts;

    private bool isMenuShown = false;

    void Start()
    {
        if (clearMenuPanel != null)
        {
            clearMenuPanel.SetActive(false);
        }
        isMenuShown = false;
    }

    // ✅ 외부에서 호출: 클리어 발생 시 실행
    public void ShowClearMenu()
    {
        UnityEngine.Debug.Log("[ClearMenu] ShowClearMenu 호출됨");

        if (isMenuShown)
        {
            UnityEngine.Debug.LogWarning("[ClearMenu] 이미 메뉴가 떠 있음. 중복 호출 방지됨");
        }

        if (clearMenuPanel == null)
        {
            UnityEngine.Debug.LogError("[ClearMenu] clearMenuPanel이 null임!");
        }

        if (isMenuShown || clearMenuPanel == null) return;

        clearMenuPanel.SetActive(true);
        backgroundMusic.Stop();
        AudioManager.Instance.PlayGameClear();
        Time.timeScale = 0f;
        isMenuShown = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (var script in playerControlScripts)
        {
            if (script != null)
            {
                script.enabled = false;
                UnityEngine.Debug.Log($"[ClearMenu] Disabled: {script.GetType().Name}");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[ClearMenu] playerControlScripts에 null 있음");
            }
        }
    }


    // ✅ 버튼 연결: 메인 메뉴로 이동
    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlayUIClick();
        // 게임 속도 복구
        Time.timeScale = 1f;

        // 플레이어 조작 복구 (선택)
        foreach (var script in playerControlScripts)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }

        // 씬 전환
        SceneManager.LoadScene("MainMenuScene");
    }
}
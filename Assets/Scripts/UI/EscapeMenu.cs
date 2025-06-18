using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [Header("메뉴 패널")]
    public GameObject escapeMenu;

    [Header("플레이어 조작 스크립트들")]
    public MonoBehaviour[] playerControlScripts;

    private bool isMenuShown = false;

    void Start()
    {
        if (escapeMenu != null)
            escapeMenu.SetActive(false);

        isMenuShown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        if (escapeMenu == null) return;

        isMenuShown = !escapeMenu.activeSelf;
        escapeMenu.SetActive(isMenuShown);
        Time.timeScale = isMenuShown ? 0f : 1f;

        if (isMenuShown)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        foreach (var script in playerControlScripts)
        {
            if (script != null)
                script.enabled = !isMenuShown;
        }
    }

    // ▶ Resume 버튼에 연결
    public void ResumeGame()
    {
        AudioManager.Instance.PlayUIClick();

        if (escapeMenu != null)
            escapeMenu.SetActive(false);

        Time.timeScale = 1f;
        isMenuShown = false;

        // ❌ 커서 상태는 변경하지 않음 (사용자가 직접 UI를 계속 조작할 수 있도록)

        foreach (var script in playerControlScripts)
        {
            if (script != null)
                script.enabled = true;
        }
    }

    // ▶ Main Menu 버튼에 연결
    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlayUIClick();
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
}
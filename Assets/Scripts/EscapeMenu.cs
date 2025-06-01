using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [Header("메뉴 패널")]
    public GameObject escapeMenu;

    void Start()
    {
        if (escapeMenu != null)
            escapeMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeMenu != null)
            {
                bool isActive = escapeMenu.activeSelf;
                escapeMenu.SetActive(!isActive);
                Time.timeScale = isActive ? 1f : 0f;
            }
        }
    }

    // ▶ Resume 버튼에 연결
    public void ResumeGame()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // ▶ Main Menu 버튼에 연결
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
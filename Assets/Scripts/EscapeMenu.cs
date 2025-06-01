using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [Header("�޴� �г�")]
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

    // �� Resume ��ư�� ����
    public void ResumeGame()
    {
        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // �� Main Menu ��ư�� ����
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
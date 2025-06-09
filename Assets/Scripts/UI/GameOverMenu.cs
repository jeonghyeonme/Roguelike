using System.Diagnostics;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [Header("Game Over �г�")]
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // ���� �� ���α�
        }
        else
        {
            UnityEngine.Debug.LogWarning("[GameOverMenu] GameOverPanel�� ������� �ʾҽ��ϴ�.");
        }
    }

    public void Show()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // ���� �Ͻ����� (����)
        }
    }
}
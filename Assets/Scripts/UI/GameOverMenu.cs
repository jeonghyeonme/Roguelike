using System.Diagnostics;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [Header("Game Over 패널")]
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 시작 시 꺼두기
        }
        else
        {
            UnityEngine.Debug.LogWarning("[GameOverMenu] GameOverPanel이 연결되지 않았습니다.");
        }
    }

    public void Show()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지 (선택)
        }
    }
}
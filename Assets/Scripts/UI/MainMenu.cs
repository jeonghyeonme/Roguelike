using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    [Header("시작할 게임 씬 이름")]
    public string gameSceneName = "TestScene"; // 불러올 게임 씬 이름 지정

    // Start 버튼 클릭 시 호출
    public void StartGame()
    {
        UnityEngine.Debug.Log("게임 시작!");
        SceneManager.LoadScene(gameSceneName);
    }

    // Quit 버튼 클릭 시 호출
    public void QuitGame()
    {
        UnityEngine.Debug.Log("게임 종료 요청");
        UnityEngine.Application.Quit();
    }
}
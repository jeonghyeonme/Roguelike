using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    [Header("������ ���� �� �̸�")]
    public string gameSceneName = "TestScene"; // �ҷ��� ���� �� �̸� ����

    // Start ��ư Ŭ�� �� ȣ��
    public void StartGame()
    {
        UnityEngine.Debug.Log("���� ����!");
        SceneManager.LoadScene(gameSceneName);
    }

    // Quit ��ư Ŭ�� �� ȣ��
    public void QuitGame()
    {
        UnityEngine.Debug.Log("���� ���� ��û");
        UnityEngine.Application.Quit();
    }
}
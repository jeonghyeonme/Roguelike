using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.Find("PlayerSpawnPoint");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
        else
        {
            UnityEngine.Debug.LogWarning("Player �Ǵ� SpawnPoint�� ã�� �� �����ϴ�.");
        }
    }
}
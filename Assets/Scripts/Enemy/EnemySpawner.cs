using System.Diagnostics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 대상")]
    public GameObject[] enemyPrefabs;       // 소환할 적 프리팹 목록

    [Header("플레이어 정보")]
    public Transform playerTransform;       // 거리 체크 대상: 플레이어

    [Header("스폰 조건")]
    public float spawnDistance = 5f;        // 플레이어가 이 거리 이내로 오면 스폰
    public bool onlyOnce = true;            // 한 번만 스폰할 것인지 여부

    private bool hasSpawned = false;        // 중복 스폰 방지 플래그

    void Update()
    {
        if (playerTransform == null || enemyPrefabs.Length == 0)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        UnityEngine.Debug.Log($"[Spawner] Player distance: {distance}");

        if (distance <= spawnDistance && (!onlyOnce || !hasSpawned))
        {
            SpawnEnemy();
            hasSpawned = true;
        }
    }

    void SpawnEnemy()
    {
        int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[index], transform.position, Quaternion.identity);
        UnityEngine.Debug.Log($"[Spawned] {enemy.name} at {transform.position}");
    }
}
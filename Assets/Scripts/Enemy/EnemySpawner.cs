using System.Diagnostics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 대상")]
    public GameObject[] enemyPrefabs;

    [Header("플레이어")]
    public Transform playerTransform;

    [Header("스폰 조건")]
    public float spawnDistance = 5f;
    public bool onlyOnce = false;

    [Header("쿨다운")]
    public float cooldownTime = 3f;
    private float cooldownTimer = 0f;
    private bool hasSpawned = false;

    void Update()
    {
        if (playerTransform == null || enemyPrefabs.Length == 0)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        cooldownTimer += Time.deltaTime;

        UnityEngine.Debug.Log($"[Spawner: {name}] Distance: {distance}");

        if (distance <= spawnDistance && cooldownTimer >= cooldownTime)
        {
            SpawnEnemy();
            cooldownTimer = 0f;

            if (onlyOnce && !hasSpawned)
            {
                hasSpawned = true;
                enabled = false; // 스크립트 비활성화로 반복 방지
            }
        }
    }

    void SpawnEnemy()
    {
        int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[index], transform.position, Quaternion.identity);
        UnityEngine.Debug.Log($"[Spawner: {name}] Spawned {enemy.name}");
    }
}
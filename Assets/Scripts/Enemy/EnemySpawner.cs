using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹")]
    public List<GameObject> enemyPrefabs;

    [Header("플레이어")]
    public Transform playerTransform;

    [Header("스폰 조건")]
    public float spawnDistance = 5f;
    public float cooldownTime = 3f;

    [Header("UI")]
    public GameObject dungeonClearUI;
    public GameObject gameEndUI;

    private float cooldownTimer = 0f;
    private bool isClearTriggered = false;

    void Start()
    {
        foreach (var enemy in enemyPrefabs)
        {
            EnemySpawnManager.Instance.RegisterEnemy(enemy);
        }
    }

    void Update()
    {
        if (playerTransform == null || enemyPrefabs.Count == 0)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        cooldownTimer += Time.deltaTime;

        if (distance <= spawnDistance && cooldownTimer >= cooldownTime && !isClearTriggered)
        {
            TrySpawnEnemy();
            cooldownTimer = 0f;
        }
    }

    void TrySpawnEnemy()
    {
        List<GameObject> spawnable = new();
        foreach (var enemy in enemyPrefabs)
        {
            if (EnemySpawnManager.Instance.CanSpawn(enemy))
                spawnable.Add(enemy);
        }

        if (spawnable.Count == 0)
        {
            TriggerDungeonClear();
            return;
        }

        GameObject chosen = spawnable[UnityEngine.Random.Range(0, spawnable.Count)];
        Instantiate(chosen, transform.position, Quaternion.identity);
        EnemySpawnManager.Instance.RecordSpawn(chosen);

        UnityEngine.Debug.Log($"[Spawner:{name}] {chosen.name} 스폰 완료");
        EnemySpawnManager.Instance.LogRemainingSpawns();
    }

    void TriggerDungeonClear()
    {
        if (isClearTriggered) return;
        isClearTriggered = true;

        if (dungeonClearUI != null) dungeonClearUI.SetActive(true);
        if (gameEndUI != null) gameEndUI.SetActive(true);

        UnityEngine.Debug.Log("[Spawner] 던전 클리어 조건 만족 - 모든 적 스폰 완료");
    }
}
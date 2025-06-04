using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    public int defaultSpawnLimit = 5;

    private Dictionary<GameObject, int> spawnLimits = new();
    private Dictionary<GameObject, int> spawnCounts = new();

    [Header("UI")]
    public GameObject dungeonClearUI;

    private bool isClearTriggered = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // ✅ 클리어 조건 검사
        if (!isClearTriggered && IsAllSpawned() && AreAllEnemiesDead())
        {
            TriggerDungeonClear();
        }
    }

    public void RegisterEnemy(GameObject enemyPrefab)
    {
        if (!spawnLimits.ContainsKey(enemyPrefab))
        {
            spawnLimits[enemyPrefab] = defaultSpawnLimit;
            spawnCounts[enemyPrefab] = 0;
        }
    }

    public bool CanSpawn(GameObject enemyPrefab)
    {
        if (!spawnLimits.ContainsKey(enemyPrefab)) return false;
        return spawnCounts[enemyPrefab] < spawnLimits[enemyPrefab];
    }

    public void RecordSpawn(GameObject enemyPrefab)
    {
        if (spawnCounts.ContainsKey(enemyPrefab))
            spawnCounts[enemyPrefab]++;
    }

    public bool IsAllSpawned()
    {
        foreach (var kvp in spawnLimits)
        {
            if (spawnCounts[kvp.Key] < kvp.Value)
                return false;
        }
        return true;
    }

    public bool AreAllEnemiesDead()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private void TriggerDungeonClear()
    {
        isClearTriggered = true;
        UnityEngine.Debug.Log("[SpawnManager] 던전 클리어: 스폰 완료 + 적 없음");

        if (dungeonClearUI != null)
        {
            // ClearMenu 컴포넌트의 ShowClearMenu() 호출
            var clearMenu = dungeonClearUI.GetComponent<ClearMenu>();
            if (clearMenu != null)
                clearMenu.ShowClearMenu();
            else
                dungeonClearUI.SetActive(true); // fallback
        }
    }

    public void LogRemainingSpawns()
    {
        foreach (var kvp in spawnLimits)
        {
            int spawned = spawnCounts[kvp.Key];
            int limit = kvp.Value;
            UnityEngine.Debug.Log($"[SpawnManager] {kvp.Key.name} 남은 스폰 수: {limit - spawned}/{limit}");
        }
    }
}
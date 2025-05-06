using System.Diagnostics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UnityEngine.Debug.Log($"[Enemy] 피격! 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        UnityEngine.Debug.Log("[Enemy] 사망!");
        Destroy(gameObject);  // 테스트용: 적 오브젝트 삭제
    }
}
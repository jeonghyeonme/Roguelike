using System.Diagnostics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UnityEngine.Debug.Log($"[Player] 피격! 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        UnityEngine.Debug.Log("[Player] 사망!");
        // 추후: 사망 애니메이션 / 재시작 처리
    }
}
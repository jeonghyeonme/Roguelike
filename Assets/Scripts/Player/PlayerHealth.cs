using System.Diagnostics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 5;
    public GameObject gameManagerObject; // UI 오브젝트를 연결
    public int currentHealth;

    private DamageFeedback feedback;
    private Animator animator;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    private Collider2D col;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        feedback = GetComponent<DamageFeedback>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        AudioManager.Instance.PlayHit();
        UnityEngine.Debug.Log($"[Player] 피격! 현재 체력: {currentHealth}");

        if (feedback != null)
            feedback.TriggerFeedback(Vector2.zero);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        UnityEngine.Debug.Log("[Player] 사망!");

        // 이동 및 입력 중단
        if (movement != null)
            movement.OnDeath();

        // 무기 비활성화
        Transform weapon = transform.Find("WeaponPivot/Weapon");
        if (weapon != null)
            weapon.gameObject.SetActive(false);

        // Rigidbody / Collider 비활성화
        if (rb != null) rb.simulated = false;
        if (col != null) col.enabled = false;

        // 애니메이션 트리거
        if (animator != null)
            animator.SetTrigger("Die");
    }

    // 🔔 애니메이션 마지막 프레임에서 호출
    public void OnDeathAnimationEnd()
    {
        UnityEngine.Debug.Log("[Player] 애니메이션 종료, 게임오버 처리 시작");

        if (gameManagerObject != null)
        {
            GameOverMenu menu = gameManagerObject.GetComponent<GameOverMenu>();
            if (menu != null)
            {
                menu.Show();
            }
        }
    }
    public bool IsDead => isDead;
}
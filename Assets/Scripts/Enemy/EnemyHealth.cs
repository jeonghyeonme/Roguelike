using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    private int currentHealth;

    private DamageFeedback feedback;
    private Animator animator;
    private EnemyMovement movement;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        feedback = GetComponent<DamageFeedback>();
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        AudioManager.Instance.PlayHit();
        UnityEngine.Debug.Log($"[Enemy] 피격! 현재 체력: {currentHealth}");

        if (feedback != null)
        {
            Vector2 knockSource = transform.position + Vector3.left;
            feedback.TriggerFeedback(knockSource);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        UnityEngine.Debug.Log("[Enemy] 사망!");

        // 이동 정지
        if (movement != null)
            movement.OnDeath();

        // 무기 비활성화
        Transform weapon = transform.Find("WeaponPivot/Weapon");
        if (weapon != null)
            weapon.gameObject.SetActive(false);

        // Rigidbody / Collider 비활성화
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 애니메이션
        if (animator != null)
            animator.SetTrigger("Die");
    }

    // 🔔 애니메이션 마지막 프레임에서 호출될 메서드
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    public bool IsDead => isDead;
}
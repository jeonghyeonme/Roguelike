using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    [Header("Collider Reference")]
    public Collider2D hitboxCollider;

    [Header("Target Layer")]
    public LayerMask targetLayer;  // 예: Player만 포함

    void Awake()
    {
        if (hitboxCollider == null)
            hitboxCollider = GetComponent<Collider2D>();

        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitboxCollider.enabled) return;

        // ✅ Enemy → Enemy 공격 무시
        if (CompareTag("Enemy") && other.CompareTag("Enemy"))
            return;

        // ✅ LayerMask 검사
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            // 데미지 처리
            if (other.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
                UnityEngine.Debug.Log("Player just get DAMAGED.");
            }

            // 피격 피드백
            if (other.TryGetComponent(out DamageFeedback feedback))
            {
                feedback.TriggerFeedback(transform.position);
            }
        }
    }
}

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

        // LayerMask로 타겟 감지
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            // IDamageable 처리
            if (other.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
                UnityEngine.Debug.Log("Player just get DAMAGED.");
            }

            // DamageFeedback이 있다면 넉백/깜빡임
            if (other.TryGetComponent(out DamageFeedback feedback))
            {
                feedback.TriggerFeedback(transform.position); // 무기 위치 기준 넉백 방향
            }
        }
    }
}

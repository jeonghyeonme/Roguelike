using System.Diagnostics;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    [Header("Collider Reference")]
    public Collider2D hitboxCollider;  // 반드시 IsTrigger=true인 Collider 연결

    void Awake()
    {
        // 자동으로 연결되도록 처리
        if (hitboxCollider == null)
            hitboxCollider = GetComponent<Collider2D>();

        // 처음에는 비활성화
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    /// <summary>
    /// 애니메이션 이벤트에서 호출 (휘두르기 시작 시점)
    /// </summary>
    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    /// <summary>
    /// 애니메이션 이벤트에서 호출 (휘두르기 끝 시점)
    /// </summary>
    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitboxCollider.enabled) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                UnityEngine.Debug.Log("적에게 데미지를 주었습니다.");
            }
        }
    }
}
using UnityEngine;
using System.Collections;

public class DamageFeedback : MonoBehaviour
{
    [Header("Sprite Flash")]
    [SerializeField] private SpriteRenderer targetSprite;        // 플레이어 본체 SpriteRenderer
    [SerializeField] private SpriteRenderer weaponSprite;        // 무기 SpriteRenderer
    [SerializeField] private Material whiteFlashMaterial;        // 깜빡임용 흰색 머티리얼
    [SerializeField] private float flashDuration = 0.1f;

    private Material defaultMaterial;
    private Material weaponDefaultMaterial;

    [Header("Knockback")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackForce = 5f;

    [Header("Stun")]
    public float stunDuration = 0.1f;
    [HideInInspector] public bool isStunned = false;

    private Coroutine feedbackRoutine;

    void Awake()
    {
        if (targetSprite == null)
            targetSprite = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (targetSprite != null)
            defaultMaterial = targetSprite.material;

        if (weaponSprite != null)
            weaponDefaultMaterial = weaponSprite.material;
    }

    /// <summary>
    /// 피격 시 호출 (공격자의 위치 기준으로 넉백 방향 계산)
    /// </summary>
    /// <param name="sourcePosition">공격자 위치</param>
    public void TriggerFeedback(Vector2 sourcePosition)
    {
        UnityEngine.Debug.Log("[DamageFeedback] TriggerFeedback 호출됨");

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(DoFeedback(sourcePosition));
    }

    private IEnumerator DoFeedback(Vector2 sourcePosition)
    {
        isStunned = true;

        // ✅ 1. 하얗게 깜빡임 적용
        if (targetSprite != null && whiteFlashMaterial != null)
            targetSprite.material = whiteFlashMaterial;

        if (weaponSprite != null && whiteFlashMaterial != null)
            weaponSprite.material = whiteFlashMaterial;

        // ✅ 2. 넉백 즉시 적용
        if (rb != null)
        {
            Vector2 knockDir = (rb.position - sourcePosition).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            UnityEngine.Debug.Log($"[DamageFeedback] Knockback 방향: {knockDir}");
        }

        // ✅ 3. 깜빡임 시간 유지
        yield return new WaitForSeconds(flashDuration);

        // ✅ 4. 머티리얼 복원
        if (targetSprite != null && defaultMaterial != null)
            targetSprite.material = defaultMaterial;

        if (weaponSprite != null && weaponDefaultMaterial != null)
            weaponSprite.material = weaponDefaultMaterial;

        // ✅ 5. 추가 경직 시간 유지
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }
}
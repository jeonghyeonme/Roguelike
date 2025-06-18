using UnityEngine;

public class EnemyWeaponMovement : MonoBehaviour
{
    [Header("References")]
    public Transform enemyTransform;
    public Transform playerTransform;
    public Transform weaponSpriteTransform;
    public Animator weaponAnimator;

    private SpriteRenderer weaponSpriteRenderer;

    [Header("Offset")]
    public Vector3 rightOffset = new Vector3(0.5f, 0.5f, 0f);
    public Vector3 leftOffset = new Vector3(-0.5f, 0.5f, 0f);

    [Header("Rotation")]
    public float rotationSmoothSpeed = 15f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    private float lastAttackTime = -999f;

    private bool isPlayerLeft = false;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
            else
                UnityEngine.Debug.LogWarning("[EnemyWeaponMovement] Player not found in scene!");
        }

        if (weaponAnimator == null && weaponSpriteTransform != null)
            weaponAnimator = weaponSpriteTransform.GetComponent<Animator>();

        if (weaponSpriteTransform != null)
            weaponSpriteRenderer = weaponSpriteTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (enemyTransform == null || playerTransform == null || weaponSpriteTransform == null)
            return;

        // 좌우 방향 판단
        isPlayerLeft = playerTransform.position.x < enemyTransform.position.x;
        weaponAnimator.SetBool("IsLeft", isPlayerLeft);
        if (weaponSpriteRenderer != null)
            weaponSpriteRenderer.flipX = isPlayerLeft;

        // ✅ 공격 조건 체크
        float distanceToPlayer = Vector2.Distance(enemyTransform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            weaponAnimator.SetTrigger("Attack");
            AudioManager.Instance.PlaySwing();
            lastAttackTime = Time.time;
        }
    }

    void LateUpdate()
    {
        if (enemyTransform == null || playerTransform == null || weaponSpriteTransform == null)
            return;

        // 위치 보정
        Vector3 offset = isPlayerLeft ? leftOffset : rightOffset;
        transform.position = enemyTransform.position + offset;

        // 회전 처리
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (isPlayerLeft) angle += 180f;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, angle),
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
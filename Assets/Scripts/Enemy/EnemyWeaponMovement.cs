using UnityEngine;

public class EnemyWeaponMovement : MonoBehaviour
{
    [Header("References")]
    public Transform enemyTransform;         // 적 본체
    public Transform playerTransform;        // 플레이어 Transform
    public Transform weaponSpriteTransform;  // Sprite/애니메이션 적용용 무기
    public Animator weaponAnimator;

    private SpriteRenderer weaponSpriteRenderer;

    [Header("Offset")]
    public Vector3 rightOffset = new Vector3(0.5f, 0.5f, 0f);
    public Vector3 leftOffset = new Vector3(-0.5f, 0.5f, 0f);

    [Header("Rotation")]
    public float rotationSmoothSpeed = 15f;

    private bool isPlayerLeft = false;

    void Start()
    {
        // ✅ 씬 내 플레이어 찾기 (스포너 사용 시 필수)
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

        // Animator 방향 설정
        weaponAnimator.SetBool("IsLeft", isPlayerLeft);

        if (weaponSpriteRenderer != null)
            weaponSpriteRenderer.flipX = isPlayerLeft;
    }

    void LateUpdate()
    {
        if (enemyTransform == null || playerTransform == null || weaponSpriteTransform == null)
            return;

        // 무기 위치 설정
        Vector3 offset = isPlayerLeft ? leftOffset : rightOffset;
        transform.position = enemyTransform.position + offset;

        // 무기 회전 처리
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
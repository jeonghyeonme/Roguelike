using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Transform weaponSpriteTransform;
    public Camera mainCamera;
    public Animator weaponAnimator;

    private SpriteRenderer weaponSpriteRenderer;  // 🔁 flipX 처리용 SpriteRenderer

    [Header("Offset")]
    public Vector3 rightOffset = new Vector3(0.5f, -0.2f, 0f);
    public Vector3 leftOffset = new Vector3(-0.5f, -0.2f, 0f);

    [Header("Rotation")]
    public float rotationSmoothSpeed = 15f;

    [Header("Attack")]
    public float attackCooldown = 0.4f;
    private float lastAttackTime = -999f;

    private bool isMouseLeft = false;

    void Start()
    {
        if (weaponAnimator == null && weaponSpriteTransform != null)
            weaponAnimator = weaponSpriteTransform.GetComponent<Animator>();

        if (weaponSpriteTransform != null)
            weaponSpriteRenderer = weaponSpriteTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerTransform == null || mainCamera == null || weaponAnimator == null)
            return;

        // 마우스 위치 기반 방향 판단
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerTransform.position);
        isMouseLeft = mouseScreenPos.x < playerScreenPos.x;

        // 방향 전달
        weaponAnimator.SetBool("IsLeft", isMouseLeft);

        // 🔁 flip 처리 복구
        if (weaponSpriteRenderer != null)
            weaponSpriteRenderer.flipX = isMouseLeft;

        // 공격 입력 처리
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            weaponAnimator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null || mainCamera == null || weaponSpriteTransform == null)
            return;

        // 무기 위치 설정
        Vector3 offset = isMouseLeft ? leftOffset : rightOffset;
        transform.position = playerTransform.position + offset;

        // 회전 처리
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (isMouseLeft) angle += 180f;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, angle),
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
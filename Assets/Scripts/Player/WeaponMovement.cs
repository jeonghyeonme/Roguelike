using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;               // 플레이어 위치
    public Transform weaponSpriteTransform;         // 무기 Sprite가 붙은 자식 오브젝트
    public Camera mainCamera;

    [Header("Offset")]
    public Vector3 rightOffset = new Vector3(0.5f, -0.2f, 0f); // 오른쪽 손 위치
    public Vector3 leftOffset = new Vector3(-0.5f, -0.2f, 0f); // 왼쪽 손 위치

    [Header("Rotation")]
    public float rotationSmoothSpeed = 15f;         // 회전 부드러움 정도

    void LateUpdate()
    {
        if (playerTransform == null || mainCamera == null || weaponSpriteTransform == null)
            return;

        // 마우스 및 플레이어의 스크린 좌표
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerTransform.position);
        bool isMouseLeft = mouseScreenPos.x < playerScreenPos.x;

        // 무기 위치 업데이트
        Vector3 offset = isMouseLeft ? leftOffset : rightOffset;
        transform.position = playerTransform.position + offset;

        // 마우스 방향 계산
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 왼쪽일 경우 회전 각도 보정
        if (isMouseLeft)
        {
            angle += 180f;
        }

        // 회전 적용
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        weaponSpriteTransform.rotation = Quaternion.Lerp(
            weaponSpriteTransform.rotation,
            targetRotation,
            rotationSmoothSpeed * Time.deltaTime
        );

        // 좌우 flip (scale.x 반전)
        Vector3 scale = weaponSpriteTransform.localScale;
        scale.x = isMouseLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        weaponSpriteTransform.localScale = scale;
    }
}
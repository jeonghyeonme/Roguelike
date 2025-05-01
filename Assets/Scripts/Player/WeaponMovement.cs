using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;               // 플레이어 위치
    public Transform weaponSpriteTransform;         // 회전/스프라이트 담당 (자식 오브젝트)
    public Camera mainCamera;

    [Header("Position Offset")]
    public Vector3 rightOffset = new Vector3(0.5f, 0f, 0f);
    public Vector3 leftOffset = new Vector3(-0.5f, 0f, 0f);

    void LateUpdate()
    {
        if (playerTransform == null || mainCamera == null) return;

        // 1. 마우스와 플레이어의 스크린 좌표 비교
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(playerTransform.position);

        // 2. 무기 위치는 플레이어 기준 오프셋 적용 (왼쪽/오른쪽)
        if (mouseScreenPos.x < playerScreenPos.x)
        {
            transform.position = playerTransform.position + leftOffset;
        }
        else
        {
            transform.position = playerTransform.position + rightOffset;
        }

        // 3. 마우스 각도 계산
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector3 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. 무기 회전 처리
        if (mouseScreenPos.x < playerScreenPos.x)
        {
            weaponSpriteTransform.rotation = Quaternion.Euler(0f, 180f, -angle);
        }
        else
        {
            weaponSpriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("스태미나 바 이미지")]
    [SerializeField] private Image spBarImage;

    [Header("플레이어 이동")]
    [SerializeField] private PlayerMovement playerMovement;

    void Update()
    {
        if (playerMovement == null || spBarImage == null) return;

        float cooldown = playerMovement.dashCooldown;
        float elapsed = Time.time - playerMovement.LastDashTime;
        float ratio = elapsed / cooldown;
        spBarImage.fillAmount = Mathf.Clamp01(ratio);
    }
}
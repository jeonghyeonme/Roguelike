using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("���¹̳� �� �̹���")]
    [SerializeField] private Image spBarImage;

    [Header("�÷��̾� �̵�")]
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
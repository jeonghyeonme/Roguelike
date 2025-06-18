using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("체력 바 이미지")]
    [SerializeField] private Image hpBarImage;

    [Header("플레이어 체력")]
    [SerializeField] private PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth == null || hpBarImage == null) return;

        float ratio = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        hpBarImage.fillAmount = Mathf.Clamp01(ratio);
    }
}

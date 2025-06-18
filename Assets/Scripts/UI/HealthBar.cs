using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("ü�� �� �̹���")]
    [SerializeField] private Image hpBarImage;

    [Header("�÷��̾� ü��")]
    [SerializeField] private PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth == null || hpBarImage == null) return;

        float ratio = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        hpBarImage.fillAmount = Mathf.Clamp01(ratio);
    }
}

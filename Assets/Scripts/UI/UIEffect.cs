using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image img;
    private Outline outline;
    private Color originalColor;
    private Color hoverColor;

    private Vector3 originalScale;

    [SerializeField] private float darkenAmount = 0.15f;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float scaleFactor = 1.05f;

    void Awake()
    {
        img = GetComponent<Image>();
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.enabled = false; // 초기엔 비활성화
            outline.effectColor = new Color(1f, 1f, 1f, 0.8f); // 반투명 흰색
            outline.effectDistance = new Vector2(2f, -2f);
        }

        originalColor = img.color;
        hoverColor = DarkenColor(originalColor, darkenAmount);
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = originalColor;
        transform.localScale = originalScale;
        outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FlashEffect());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        img.color = hoverColor;
    }

    private IEnumerator FlashEffect()
    {
        img.color = Color.white;
        transform.localScale = originalScale * scaleFactor;
        outline.enabled = true;

        yield return new WaitForSeconds(flashDuration);

        img.color = hoverColor;
        transform.localScale = originalScale;
        outline.enabled = false;
    }

    private Color DarkenColor(Color color, float amount)
    {
        return new Color(
            Mathf.Clamp01(color.r * (1f - amount)),
            Mathf.Clamp01(color.g * (1f - amount)),
            Mathf.Clamp01(color.b * (1f - amount)),
            color.a
        );
    }
}
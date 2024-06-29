using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgcolor : MonoBehaviour
{
    public float duration = 3f; // Renk geçişinin süresi
    public Color[] colors; // Belirlenen renklerin dizisi
    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomTargetColor();
    }

    void Update()
    {
        // Mevcut rengi hedef renge smooth bir şekilde değiştir
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime / duration);

        // Eğer renk geçişi tamamlandıysa yeni hedef renk belirle
        if (Vector4.Distance(spriteRenderer.color, targetColor) < 0.01f)
        {
            SetRandomTargetColor();
        }
    }

    void SetRandomTargetColor()
    {
        Color newColor;
        do
        {
            newColor = colors[Random.Range(0, colors.Length)];
        } while (newColor == spriteRenderer.color);

        targetColor = newColor;
    }
}

using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class FlickerEffect : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private Color flickerColor = Color.red;
    [SerializeField] private float flickerDuration = 0.1f;
    [SerializeField] private int flickerRepeatCount = 5;

    private List<Color> originalColors = new List<Color>();

    void Start()
    {
        // Capture the original colors of the sprite renderers
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                originalColors.Add(spriteRenderer.color);
            }
        }
        
    }

    public void ApplyFlickerEffect()
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            var spriteRenderer = spriteRenderers[i];

            if (spriteRenderer != null)
            {
                // Create a sequence for the flicker effect
                Sequence flickerSequence = DOTween.Sequence();

                // Add flicker effect by changing color back and forth
                flickerSequence.Append(spriteRenderer.DOColor(flickerColor, flickerDuration))
                    .Append(spriteRenderer.DOColor(originalColors[i], flickerDuration))
                    .SetLoops(flickerRepeatCount * 2, LoopType.Yoyo);
            }
        }
    }
}
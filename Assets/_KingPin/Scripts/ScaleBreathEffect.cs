using UnityEngine;
using DG.Tweening;

public class ScaleBreathEffect : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1.2f; // El factor de escala al que se aumentará el tamaño.
    [SerializeField] private float duration = 1.0f; // La duración de la animación en segundos.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Creamos la animación de escalamiento hacia el tamaño aumentado.
        transform.DOScale(scaleFactor, duration)
            .SetLoops(-1, LoopType.Yoyo) // Se configura para repetir indefinidamente en un bucle de vaivén.
            .SetEase(Ease.InOutSine); // Se establece un tipo de suavizado para un efecto de "respiración" más realista.
    }

    // Update is called once per frame
    void Update()
    {
    }
}
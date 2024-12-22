using System;
using UnityEngine;
using DG.Tweening;

public class ResultsPanelController : MonoBehaviour
{
    // Lista de GameObjects que recibirá el script
    [SerializeField] private GameObject[] gameObjectsToScale;

    // Tiempo del tween para cada GameObject
    public float tweenDuration = 1.0f;

    // Retraso entre cada tween
    public float delayBetweenTweens = 0.5f;

    // Array para almacenar las escalas originales
    private Vector3[] originalScales;

    // Método Start

    private void OnEnable()
    {
        CaptureOriginalScales();
        ScaleObjectsToZero();
        ScaleObjectsToOriginalWithDelay();
    }

    // Captura las escalas originales de los objetos
    void CaptureOriginalScales()
    {
        originalScales = new Vector3[gameObjectsToScale.Length];
        for (int i = 0; i < gameObjectsToScale.Length; i++)
        {
            if (gameObjectsToScale[i] != null)
            {
                originalScales[i] = gameObjectsToScale[i].transform.localScale;
            }
        }
    }

    // Escala todos los objetos a cero
    void ScaleObjectsToZero()
    {
        foreach (var obj in gameObjectsToScale)
        {
            if (obj != null)
            {
                obj.transform.localScale = Vector3.zero;
            }
        }
    }

    // Escala todos los objetos a su escala original con un delay entre cada uno
    void ScaleObjectsToOriginalWithDelay()
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < gameObjectsToScale.Length; i++)
        {
            var obj = gameObjectsToScale[i];
            if (obj != null)
            {
                sequence.Append(obj.transform.DOScale(originalScales[i], tweenDuration).SetEase(Ease.OutBack));
                sequence.AppendInterval(delayBetweenTweens);
            }
        }
    }
}
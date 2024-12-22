using System;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;

public class MoveOnClick : MonoBehaviour, MMEventListener<PuzzleSolved>
{
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private bool isRequiredPin;
    [SerializeField] private bool canReposition; // Nuevo campo para alternar posiciones

    private bool isMoving = false;
    private Vector3 initialPosition; // Almacena la posición inicial

    private void Awake()
    {
        // Guarda la posición inicial al iniciar
        initialPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        if (!isMoving)
        {
            isMoving = true;
            if (isRequiredPin)
                PinMovement.Trigger();
            MoveObject();
        }
    }

    private void MoveObject()
    {
        if (canReposition)
        {
            // Alterna entre la posición final e inicial
            Vector3 targetPosition = (transform.localPosition == initialPosition) ? finalPosition : initialPosition;
            transform.DOLocalMove(targetPosition, moveDuration).OnComplete(() => isMoving = false).SetEase(Ease.OutBack);
        }
        else
        {
            // Mueve siempre a la posición final si no puede cambiar de posición
            transform.DOLocalMove(finalPosition, moveDuration).OnComplete(() => isMoving = false);
        }
    }

    public void OnMMEvent(PuzzleSolved eventType)
    {
        isMoving = true;
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PuzzleSolved>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PuzzleSolved>();
        isMoving = false;
    }
}
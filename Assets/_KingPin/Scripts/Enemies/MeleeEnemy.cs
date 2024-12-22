using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private int turnsToReachTarget;
    [SerializeField] private float minimalDistanceToTarget;
    [SerializeField] private float movingDuration = 0.5f;
    [SerializeField] private GameObject damageSource;
    private Transform destinationTarget;
    private Transform attackTarget;
    private float distanceToTarget;
    private Vector3 moveDelta;
    private float totalDistanceToCover;
    private float distancePerTurn;
    private int curretTurn;
    private float delayAtTarget = 0.5f;
    [SerializeField] private Vector3 attackingTargetOffset;

    private void Awake()
    {
        destinationTarget = GameObject.FindGameObjectWithTag("EnemyPosition").transform;
        attackTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }


    public override void TakeTurn()
    {
        base.TakeTurn();
        TurnFinished = false;
        DetermineDistanceToTarget();
        if (curretTurn == turnsToReachTarget)
        {
            Attack();
        }
        else
        {
            MoveToTarget();
        }
    }

    public override void MoveToTarget()
    {
        base.MoveToTarget();
        if (curretTurn == turnsToReachTarget) return;
        if (moveDelta == Vector3.zero)
        {
            // Calcula la distancia total a cubrir
            totalDistanceToCover = distanceToTarget - minimalDistanceToTarget;
            distancePerTurn = totalDistanceToCover / turnsToReachTarget;

            // Calcula la dirección hacia el objetivo solo en X e Y
            Vector3 directionToTarget = (destinationTarget.position - transform.position).normalized;
            directionToTarget.z = 0; // No cambiar el eje Z

            // Ajustar el movimiento solo en el plano X,Y
            moveDelta = directionToTarget * distancePerTurn;
        }

        // Calcula la nueva posición para este turno
        Vector3 newPosition = transform.position + new Vector3(moveDelta.x, 0, 0);

        // Mueve el transform hacia la nueva posición
        transform.DOMove(newPosition, movingDuration).SetEase(Ease.Linear).OnComplete(() => TurnFinished = true);
        EnemyMoving.Trigger(this.transform);
        curretTurn++;
    }

    public override void DetermineDistanceToTarget()
    {
        base.DetermineDistanceToTarget();
        distanceToTarget = Vector3.Distance(transform.position, destinationTarget.position);
    }

    public override void Attack()
    {
        base.Attack();
        EnemyMoving.Trigger(this.transform);
        Vector3 originalPosition = transform.position;

        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(transform.DOMove(attackTarget.position + attackingTargetOffset, movingDuration).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
            
                EnemyAttacking.Trigger(this.transform);
            })
            .AppendInterval(delayAtTarget) // Wait at the target position
            .AppendCallback(() =>
            {
             
                EnemyMoving.Trigger(this.transform);
            })
            .Append(transform.DOMove(originalPosition, movingDuration).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                TurnFinished = true;
                // Reset any other states if needed
            });
    }
    
    public void EnableDamageSource()
    {
        Debug.Log($"Damage source enabled");
        StartCoroutine(HandleDamageSource());
    }

    IEnumerator HandleDamageSource()
    {
        damageSource.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        damageSource.SetActive(false);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        EnemyDeath.Trigger(this);
    }
}
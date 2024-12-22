using System;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerController : MonoBehaviour, MMEventListener<PlayerTurnStarted>, MMEventListener<EnemyDeath>
{
    private PlayerShootingManager shootingManager => GetComponent<PlayerShootingManager>();
    private bool isInTurn;
    private bool isShooting;
    private int numberOfTurns = 1;
    private float numberOfTurnsIncreasePercentage;
    private bool extraDamageThisTurn;
    private bool attackIfKilledEnemy;

    public bool AttackIfKilledEnemy
    {
        get => attackIfKilledEnemy;
        set => attackIfKilledEnemy = value;
    }

    [SerializeField] private DamageUpgradeONWave damageUpgradeONWave;

    public float NumberOfTurnsIncreasePercentage
    {
        get => numberOfTurnsIncreasePercentage;
        set => numberOfTurnsIncreasePercentage = value;
    }

    public bool ExtraDamageThisTurn
    {
        get => extraDamageThisTurn;
        set => extraDamageThisTurn = value;
    }

    private void EndTurn()
    {
        GameManager.Instance.OnPlayerTurnPhaseComplete();
        isInTurn = false;
    }


    private void TakeTurn()
    {
        isInTurn = true;

        float randomValue = UnityEngine.Random.Range(0f, 100f);
        if (randomValue < numberOfTurnsIncreasePercentage)
            numberOfTurns = 2;
        StartCoroutine(TakeMultipleTurns());
    }


    private IEnumerator TakeMultipleTurns()
    {
        for (int i = 0; i < numberOfTurns; i++)
        {
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(0.2f); // Asegurarse de espaciar bien las acciones
        }

        // Llamar a la corrutina después de la última iteración
        StartCoroutine(EndTurnAfterDelay());
    }

    IEnumerator Shoot(float delay = 0.2f)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitUntil(() => !isShooting);
        isShooting = true;
        shootingManager.Shoot();
        yield return new WaitForSeconds(0.3f);
        isShooting = false;
    }

    IEnumerator EndTurnAfterDelay()
    {
        while (isInTurn) // Solo continuar mientras el jugador esté en turno
        {
            // Esperar 1 segundo antes de cada chequeo
            yield return new WaitForSeconds(0.3f);

            // Si no está disparando, terminar turno
            if (!isShooting)
            {
                numberOfTurns = 1;
                EndTurn();
                yield break; // Salir de la corrutina 
            }
        }
    }

    public void OnDeath()
    {
        TriggerDeathEvent();
    }

    public void TriggerDeathEvent()
    {
        PlayerDeath.Trigger();
    }

    public void OnMMEvent(PlayerTurnStarted eventType)
    {
        TakeTurn();
    }

    public void OnMMEvent(EnemyDeath eventType)
    {
        if (isInTurn) // Verificar si el jugador está en turno
        {
            if(attackIfKilledEnemy)
                StartCoroutine(Shoot());
        }
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PlayerTurnStarted>();
        this.MMEventStartListening<EnemyDeath>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PlayerTurnStarted>();
        this.MMEventStopListening<EnemyDeath>();
    }
}
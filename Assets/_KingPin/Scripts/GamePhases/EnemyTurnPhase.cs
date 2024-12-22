// EnemyTurnPhase.cs

using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;


public class EnemyTurnPhase : GamePhase
{
    private List<Enemy> _enemies;
    private int _currentEnemyIndex;

    public EnemyTurnPhase(List<Enemy> enemies)
    {
        _enemies = enemies;
        _currentEnemyIndex = 0;
    }

    public override void EnterPhase()
    {
        Debug.ClearDeveloperConsole();
        base.EnterPhase();
        
        _currentEnemyIndex = 0;
        _enemies[_currentEnemyIndex].TakeTurn();
    }

    public override void UpdatePhase()
    {
        if (IsEnemyTurnFinished() && GameManager.Instance.GetPlayerStatus())
        {
            _currentEnemyIndex++;
            if (_currentEnemyIndex < _enemies.Count)
            {

                Debug.Log("EnemyTurn");
                _enemies[_currentEnemyIndex].TakeTurn();
            }
            else
            {
                CompletePhase();
            }
        }
    }

    private bool IsEnemyTurnFinished()
    {
        // Lógica para determinar si el turno del enemigo actual ha terminado
        if (_currentEnemyIndex < _enemies.Count)
        {
            return _enemies[_currentEnemyIndex].TurnFinished; // Aquí implementas la lógica de cuándo termina un turno
        }
        else
        {
            return false;
        }

    }

    private void CompletePhase()
    {
       //if (_enemies.Count == 0)
        {
            GameManager.Instance.OnEnemyTurnPhaseComplete();
        }
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Enemy Turn Phase");
    }
}
// PlayerTurnPhase.cs

using MoreMountains.TopDownEngine;
using UnityEngine;


public class PlayerTurnPhase : GamePhase
{
    public override void EnterPhase()
    {
        base.EnterPhase();
        Debug.Log("Entering Player Turn Phase");
        PlayerTurnStarted.Trigger();
    }

    private bool SimulatePlayerDeath()
    {
        // Simulación para verificar si el jugador ha muerto
        return false; // Personaliza esta condición
    }

    private void CompletePhase()
    {
       GameManager.Instance.OnPlayerTurnPhaseComplete();
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Player Turn Phase");
        PlayerTurnEndend.Trigger();
    }
}
// TravelingPhase.cs

using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class TravelingPhase : GamePhase
{
    public override void EnterPhase()
    {
        base.EnterPhase();
        Debug.Log("Entering Traveling Phase");
        TravelingPhaseStarted.Trigger();
    }
    
    private void CompletePhase()
    {
        GameManager.Instance.OnTravelPhaseComplete();
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Traveling Phase");
        TravelingPhaseEnded.Trigger();
    }
}
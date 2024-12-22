using MoreMountains.TopDownEngine;
using UnityEngine;

public class UpgradePhase : GamePhase
{
    public override void EnterPhase()
    {
        base.EnterPhase();
        Debug.Log("Entering Upgrade Phase");
        UpgradePhaseStarted.Trigger();

    }

    private void CompletePhase()
    {
        GameManager.Instance.OnUpgradePhaseComplete();
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Upgrade Phase");
        UpgradePhaseEnded.Trigger();
  
    }
}

using UnityEngine;

public class EndGamePhase : GamePhase
{
    public override void EnterPhase()
    {
        base.EnterPhase();
        Debug.Log("Entering Endgame Phase");
   
    }

    private void CompletePhase()
    {
        //GameManager.Instance.OnPuzzlePhaseComplete();
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Endgame Phase");
      
    }
}

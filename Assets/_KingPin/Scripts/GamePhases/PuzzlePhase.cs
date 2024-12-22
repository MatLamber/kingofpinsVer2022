// PuzzlePhase.cs

using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;


public class PuzzlePhase : GamePhase
{
    public override void EnterPhase()
    {
        base.EnterPhase();
        Debug.Log("Entering Puzzle Phase");
        PuzzlePhaseStarted.Trigger();
    }

    private void CompletePhase()
    {
        GameManager.Instance.OnPuzzlePhaseComplete();
    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        Debug.Log("Exiting Puzzle Phase");
        PuzzlePhaseEnded.Trigger();
    }
}
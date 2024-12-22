using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Serialization;

public class Puzzle : MonoBehaviour, MMEventListener<LootPoolEmptied>
{
    [SerializeField] private int lootPools;
    private Coroutine countdownCoroutine;
    private const float countdownTime = 10f;
    private bool puzzleSolved = false; // Track if the puzzle is already solved
    private void EndPuzzle()
    {
        if (puzzleSolved)
        {
            return; // Exit if already resolved
        }

        puzzleSolved = true; // Mark as solved
        Debug.Log("Puzzle Solved");
        PuzzleSolved.Trigger();
        if(gameObject.activeSelf)
            StartCoroutine(EndPhase());
    }
    

    IEnumerator EndPhase()
    {
        if (!gameObject.activeSelf) yield return null;
        yield return new WaitForSeconds(1f);
        if(gameObject.activeSelf)
            GameManager.Instance.OnPuzzlePhaseComplete();
    }
    
    public void OnLootPoolEmptied()
    {
        if(!gameObject.activeSelf) return;
        lootPools--;
        if(lootPools == 0)
            EndPuzzle();
    }

    
    public void OnMMEvent(LootPoolEmptied eventType)
    {
        OnLootPoolEmptied();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<LootPoolEmptied>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<LootPoolEmptied>();
    }


}
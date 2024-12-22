using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

public class PuzzlesManager : MonoBehaviour, MMEventListener<PuzzlePhaseStarted>, MMEventListener<PuzzlePhaseEnded>
{
    [SerializeField] private List<GameObject> puzzles;
    private int currentPuzzleIndex = 0;
    private Vector3 originalPosition;

    private void Start()
    {
        if (puzzles == null || puzzles.Count == 0) return;
        // Set the original position from the first puzzle in the list
        originalPosition = puzzles[currentPuzzleIndex].transform.localPosition;
        ShufflePuzzleList();
        
    }
    
    private void ShufflePuzzleList()
    {
        for (int i = puzzles.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (puzzles[i], puzzles[randomIndex]) = (puzzles[randomIndex], puzzles[i]);
        }
    }

    public void ShowCurrentPuzzle()
    {
        if (currentPuzzleIndex >= puzzles.Count) return;
        GameObject currentPuzzle = puzzles[currentPuzzleIndex];
        currentPuzzle.SetActive(true);
        currentPuzzle.transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutBack);
    }

    public void HideCurrentPuzzle()
    {
        if (currentPuzzleIndex >= puzzles.Count) return;

        GameObject currentPuzzle = puzzles[currentPuzzleIndex];
        currentPuzzle.transform.DOLocalMove(originalPosition, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            currentPuzzle.SetActive(false);
            currentPuzzleIndex++;
        });
    }

    public void OnMMEvent(PuzzlePhaseStarted eventType)
    {
        ShowCurrentPuzzle();
    }

    public void OnMMEvent(PuzzlePhaseEnded eventType)
    {
        HideCurrentPuzzle();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<PuzzlePhaseStarted>();
        this.MMEventStartListening<PuzzlePhaseEnded>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<PuzzlePhaseStarted>();
        this.MMEventStopListening<PuzzlePhaseEnded>();
    }
}
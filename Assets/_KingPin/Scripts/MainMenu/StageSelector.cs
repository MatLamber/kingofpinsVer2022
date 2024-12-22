using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    [SerializeField] private Transform[] stagePreviews;
    [SerializeField] private List<GameObject> navigationsArrows;
    [SerializeField] private GameObject playerButton;
    [SerializeField] private List<string> levelNames;
    private List<StagePreview> stagePreviewList = new List<StagePreview>();
    private float offsetDistance;
    private int currentStageIndex = 0;
    private bool isRepositioning = false;
    private int lastLevelUnlocked;

    private void Start()
    {
        offsetDistance = Vector3.Distance(stagePreviews[0].position, stagePreviews[1].position);
        lastLevelUnlocked = KingOfPinsData.Instance.LastLevelUnlocked;
        SetStagePreviewElements();
        SelectStage(lastLevelUnlocked);
    }

    public void SetStagePreviewElements()
    {
        foreach (var t in stagePreviews)
            stagePreviewList.Add(t.GetComponent<StagePreview>());

        for (int i = 0; i < stagePreviewList.Count; i++)
            stagePreviewList[i].SetStagePreview(i, i <= lastLevelUnlocked);
    }

    public void NextStage()
    {
        if (isRepositioning) return;
        isRepositioning = true;
        if (stagePreviews.Length == 0) return;

        if (currentStageIndex >= stagePreviews.Length - 1)
        {
            isRepositioning = false;
        }
        else
        {
            Debug.Log("Next Stage");
            int nextIndex = (currentStageIndex + 1) % stagePreviews.Length;
            UpdateStageView(nextIndex);
        }
    }

    public void SelectStage(int stageIndex)
    {
        StartCoroutine(SelectStageCo(stageIndex));
    }


    IEnumerator SelectStageCo(int index)
    {
        int i = 0;
        while (i < index)
        {
            yield return new WaitUntil(() => !isRepositioning);
            NextStage();
            i++;
        }
    }


    public void PreviousStage()
    {
        if (isRepositioning) return;
        isRepositioning = true;
        if (stagePreviews.Length == 0) return;

        if (currentStageIndex <= 0)
        {
            isRepositioning = false;
        }
        else
        {
            Debug.Log("Previous Stage");
            int previousIndex = (currentStageIndex - 1 + stagePreviews.Length) % stagePreviews.Length;
            UpdateStageView(previousIndex);
        }
    }

    private void Update()
    {
        UpdateArrows();
        playerButton.SetActive(currentStageIndex <= lastLevelUnlocked);
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextStage();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousStage();
    }

    private void UpdateStageView(int newStageIndex)
    {
        int direction = newStageIndex > currentStageIndex ? -1 : 1;

        for (int i = 0; i < stagePreviews.Length; i++)
        {
            Vector3 newPosition = stagePreviews[i].position + Vector3.right * direction * offsetDistance;
            stagePreviews[i].DOMove(newPosition, 0.05f).OnComplete(() => { isRepositioning = false; });
        }

        currentStageIndex = newStageIndex;
    }

    private void UpdateArrows()
    {
        if (currentStageIndex >= stagePreviews.Length - 1)
        {
            navigationsArrows[0].SetActive(true);
            navigationsArrows[1].SetActive(false);
        }
        else if (currentStageIndex <= 0)
        {
            navigationsArrows[0].SetActive(false);
            navigationsArrows[1].SetActive(true);
        }
        else
        {
            navigationsArrows[0].SetActive(true);
            navigationsArrows[1].SetActive(true);
        }
    }

    public void LoadLevel()
    {
        if (currentStageIndex < levelNames.Count)
        {
            SceneManager.LoadScene(levelNames[currentStageIndex]);
        }
    }
}
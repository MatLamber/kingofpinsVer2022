using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using Tabtale.TTPlugins;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
/// Handles all GUI effects and changes
/// </summary>
[AddComponentMenu("TopDown Engine/Managers/GUI Manager")]
public class UIManager : MMSingleton<UIManager>, MMEventListener<UpgradePhaseStarted>, MMEventListener<PlayerDeath>, MMEventListener<TravelingPhaseEnded>
{
    /// the main canvas
    [Tooltip("the main canvas")] public Canvas MainCanvas;
    

    [SerializeField] private GameObject upgradesPanel;
    private Vector3 upgradesPanelOriginalPosition;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int levelNumber;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private MMF_Player selectFeedbacks;
    private Dictionary<string, object> parameters;

    /// <summary>
    /// Statics initialization to support enter play modes
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    protected static void InitializeStatics()
    {
        _instance = null;
    }

    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Inicialization();
        parameters = new Dictionary<string, object>();
        parameters.Add("clearStage", $"stage number {levelNumber}");
        TTPGameProgression.FirebaseEvents.MissionStarted(levelNumber,parameters);
    }
    

    private void Inicialization()
    {
        upgradesPanelOriginalPosition = upgradesPanel.transform.localPosition;
    }

    public void ShowUpgradesPanel()
    {
        upgradesPanel.SetActive(true);
        upgradesPanel.transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutBack);
    }
    
    public void HideUpgradesPanel()
    {
        upgradesPanel.transform.DOLocalMoveY(upgradesPanelOriginalPosition.y, 0.3f).SetEase(Ease.InBack);
    }

    public void SelectUpgradeOption()
    {
        selectFeedbacks?.PlayFeedbacks();
        HideUpgradesPanel();
        UpgradeOptionSelected.Trigger();
        GameManager.Instance.OnUpgradePhaseComplete();
    }


    public void OnUpgradePhaseStarted()
    {
        ShowUpgradesPanel();
    }
    public void OnMMEvent(UpgradePhaseStarted eventType)
    {
        OnUpgradePhaseStarted();
    }
    
    
    public void ShowLosePanel()
    {
        TTPGameProgression.FirebaseEvents.MissionFailed(parameters);
        StartCoroutine(ShowLosePanelCo());
    }
    
    public void ShowWinPanel()
    {
        TTPGameProgression.FirebaseEvents.MissionComplete(parameters);
        StartCoroutine(ShowWinPanelCo());
    }
    
    IEnumerator ShowWinPanelCo()
    {
        yield return new WaitForSeconds(1f);
        winPanel.SetActive(true);
    }
    IEnumerator ShowLosePanelCo()
    {
        yield return new WaitForSeconds(1f);
        losePanel.SetActive(true);
    }

    public void UpdateMoneyText(string moneyAmount)
    {
        moneyText.text = moneyAmount;
    }

    public void OnPlayerDeath()
    {
        ShowLosePanel();
    }

    public void GoToMainMenuAfterWin()
    {
        if(KingOfPinsData.Instance.LastLevelUnlocked < levelNumber) KingOfPinsData.Instance.LastLevelUnlocked = levelNumber;
        KingOfPinsProgressManager.Instance.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToMainMenuAfterLose()
    {
        KingOfPinsProgressManager.Instance.Save();
        SceneManager.LoadScene("MainMenu");
    }
    public void OnMMEvent(PlayerDeath eventType)
    {
        OnPlayerDeath();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<UpgradePhaseStarted>();
        this.MMEventStartListening<PlayerDeath>();
        this.MMEventStartListening<TravelingPhaseEnded>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<UpgradePhaseStarted>();
        this.MMEventStopListening<PlayerDeath>();
        this.MMEventStopListening<TravelingPhaseEnded>();
    }


    public void OnMMEvent(TravelingPhaseEnded eventType)
    {
        UpdateWaveText();
    }

    public void UpdateWaveText()
    {
        waveText.gameObject.SetActive(true);
        waveText.text =  $"WAVE {GameManager.Instance.CurrentEnemyFormation+1}/{GameManager.Instance.EnemiesInLevel.Count}";
    }
}
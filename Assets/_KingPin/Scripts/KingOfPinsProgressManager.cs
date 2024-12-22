using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class KingofPinsProgress
{
    public int money;
    public int lastLevelUnlocked;
}

public class KingOfPinsProgressManager : MMPersistentSingleton<KingOfPinsProgressManager>, MMEventListener<TopDownEngineEvent>
{

    [SerializeField] private PlayerStats defaultStats;
    [MMInspectorButton("CreateSaveGame")]
    /// A test button to test creating the save file
    public bool CreateSaveGameBtn;


    protected const string _saveFolderName = "KingOfPinsProgressData";
    protected const string _saveFileName = "Progress.data";

    /// <summary>
    /// Statics initialization to support enter play modes
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    protected static void InitializeStatics()
    {
        _instance = null;
    }

    /// <summary>
    /// On awake, we load our progress and initialize our stars counter
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        LoadSavedProgress();
    }

    /// <summary>
    /// When a level is completed, we update our progress
    /// </summary>
    protected virtual void LevelComplete()
    {
    }


    /// <summary>
    /// Saves the progress to a file
    /// </summary>
    protected virtual void SaveProgress()
    {
        KingofPinsProgress progress = new KingofPinsProgress();
        progress.money = KingOfPinsData.Instance.Money;
        progress.lastLevelUnlocked = KingOfPinsData.Instance.LastLevelUnlocked;

        MMSaveLoadManager.Save(progress, _saveFileName, _saveFolderName);
    }

    /// <summary>
    /// A test method to create a test save file at any time from the inspector
    /// </summary>
    protected virtual void CreateSaveGame()
    {
        SaveProgress();
    }

    /// <summary>
    /// Loads the saved progress into memory
    /// </summary>
    protected virtual void LoadSavedProgress()
    {
        KingofPinsProgress progress =
            (KingofPinsProgress)MMSaveLoadManager.Load(typeof(KingofPinsProgress), _saveFileName, _saveFolderName);
        if (progress != null)
        {
            KingOfPinsData.Instance.Money = progress.money;
            KingOfPinsData.Instance.LastLevelUnlocked = progress.lastLevelUnlocked;
        }
        else
        {
            KingOfPinsData.Instance.Money = defaultStats.money;
            KingOfPinsData.Instance.LastLevelUnlocked = defaultStats.lastLevelUnlocked;
        }
    }


    /// <summary>
    /// When we grab a level complete event, we update our status, and save our progress to file
    /// </summary>
    /// <param name="gameEvent">Game event.</param>
    public virtual void OnMMEvent(TopDownEngineEvent gameEvent)
    {
        switch (gameEvent.EventType)
        {
            case TopDownEngineEventTypes.LevelComplete:
                LevelComplete();
                SaveProgress();
                break;
            case TopDownEngineEventTypes.GameOver:
                break;
        }
    }


    /// <summary>
    /// OnEnable, we start listening to events.
    /// </summary>
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<TopDownEngineEvent>();
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
        this.MMEventStopListening<TopDownEngineEvent>();
    }

    public void Save()
    {
        SaveProgress();
    }
}
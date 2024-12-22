using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Managers/Game Manager")]
    public class GameManager : MMSingleton<GameManager>,
        MMEventListener<MMGameEvent>,
        MMEventListener<TopDownEngineEvent>,
        MMEventListener<TopDownEnginePointEvent>, MMEventListener<EnemyDeath>, MMEventListener<PlayerDeath>
    {
        [Tooltip("the target frame rate for the game")]
        public int TargetFrameRate = 300;

        [Header("Lives")] [Tooltip("the maximum amount of lives the character can currently have")]
        public int MaximumLives = 0;

        [Tooltip("the current number of lives ")]
        public int CurrentLives = 0;

        [Header("Bindings")] [Tooltip("the name of the scene to redirect to when all lives are lost")]
        public string GameOverScene;

        [Header("Points")] [MMReadOnly] [Tooltip("the current number of game points")]
        public int Points;

        [Header("Pause")] [Tooltip("if this is true, the game will automatically pause when opening an inventory")]
        public bool PauseGameWhenInventoryOpens = true;

        public virtual bool Paused { get; set; }
        public virtual bool StoredLevelMapPosition { get; set; }
        public virtual Vector2 LevelMapPosition { get; set; }
        public virtual Character PersistentCharacter { get; set; }
        public List<PointsOfEntryStorage> PointsOfEntry;
        public virtual Character StoredCharacter { get; set; }

        protected bool _inventoryOpen = false;
        protected bool _pauseMenuOpen = false;
        protected InventoryInputManager _inventoryInputManager;
        protected int _initialMaximumLives;
        protected int _initialCurrentLives;

        private GamePhase currentPhase;
        [MMReadOnly]
        private List<Enemy> activeEnemies = new List<Enemy>();
        [MMReadOnly]
        private List<GameObject> enemiesInLevel = new List<GameObject>();
        private int currentEnemyFormation;
        private bool playerIsAlive;

        private int playerMoney;
        public int PlayerMoney
        {
            get { return playerMoney; }
            set { playerMoney = value; }
        }

        public List<GameObject> EnemiesInLevel => enemiesInLevel;
        public int CurrentEnemyFormation => currentEnemyFormation;


        protected override void Awake()
        {
            base.Awake();
            PointsOfEntry = new List<PointsOfEntryStorage>();
            playerIsAlive = true;
        }

        protected virtual void Start()
        {
            Application.targetFrameRate = TargetFrameRate;
            _initialCurrentLives = CurrentLives;
            _initialMaximumLives = MaximumLives;
            StartTravelPhase();
        }

        private void StartTravelPhase()
        {
            TransitionToPhase(new TravelingPhase());
            Invoke(nameof(OnTravelPhaseComplete),2f);
        }

        public void InitializeEnemies(List<GameObject> enemiesFormations)
        {
            enemiesInLevel = enemiesFormations;
            activeEnemies = enemiesInLevel[currentEnemyFormation].GetComponent<EnemyFormation>().Enemies;
        }

        public void CheckIfShouldContinue()
        {
            if (currentEnemyFormation < enemiesInLevel.Count)
                activeEnemies = enemiesInLevel[currentEnemyFormation].GetComponent<EnemyFormation>().Enemies;

        }

        private void Update()
        {
            currentPhase?.UpdatePhase();
        }

        public void TransitionToPhase(GamePhase newPhase)
        {
            currentPhase?.ExitPhase();
            currentPhase = newPhase;
            currentPhase?.EnterPhase();
        }

        public void OnTravelPhaseComplete()
        {
            TransitionToPhase(new PuzzlePhase());
        }

        public void OnPuzzlePhaseComplete()
        {
            if (activeEnemies.Count > 0)
            {
                TransitionToPhase(new UpgradePhase());
            }
            else
            {
                if (currentEnemyFormation < enemiesInLevel.Count)
                {
                    TransitionToPhase(new UpgradePhase());
                }
                else
                {
                    UIManager.Instance.ShowWinPanel();
                }
            }

        }

        public void OnEnemyTurnPhaseComplete()
        {
            if (playerIsAlive)
            {
                TransitionToPhase(new PlayerTurnPhase());
            }
        }

        public void OnPlayerTurnPhaseComplete()
        {
            if (playerIsAlive && activeEnemies.Count > 0)
            {
                TransitionToPhase(new EnemyTurnPhase(activeEnemies));
            }
            else
            {
                if (activeEnemies.Count == 0)
                {
                    CheckIfShouldContinue();
                    if (currentEnemyFormation < enemiesInLevel.Count)
                    {
                        StartTravelPhase();
                    }
                    else
                    {
                        UIManager.Instance.ShowWinPanel();
                    }
    ;
                    EnemyFormationDefetead.Trigger();
                }
            }
        }

        public void OnUpgradePhaseComplete()
        {
            if (playerIsAlive && activeEnemies.Count > 0)
            {
                 TransitionToPhase(new EnemyTurnPhase(activeEnemies));
            }
            else
            {
                if (activeEnemies.Count == 0)
                {
                    CheckIfShouldContinue();
                    if (currentEnemyFormation < enemiesInLevel.Count)
                    {
                        StartTravelPhase();
                    }
                    else
                    {
                        UIManager.Instance.ShowWinPanel();
                    }
                    EnemyFormationDefetead.Trigger();
                }
            }
        }

        public void KillPlayer()
        {
            playerIsAlive = false;
        }

        public void EnemyKilled(Enemy enemy)
        {
            activeEnemies.Remove(enemy);
        }

        public virtual void Reset()
        {
            Points = 0;
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, 1f, 0f, false, 0f, true);
            Paused = false;
        }

        public virtual void LoseLife()
        {
            CurrentLives--;
        }

        public virtual void GainLives(int lives)
        {
            CurrentLives += lives;
            if (CurrentLives > MaximumLives)
            {
                CurrentLives = MaximumLives;
            }
        }

        public virtual void AddLives(int lives, bool increaseCurrent)
        {
            MaximumLives += lives;
            if (increaseCurrent)
            {
                CurrentLives += lives;
            }
        }

        public virtual void ResetLives()
        {
            CurrentLives = _initialCurrentLives;
            MaximumLives = _initialMaximumLives;
        }

        public virtual void AddPoints(int pointsToAdd)
        {
            Points += pointsToAdd;
            GUIManager.Instance.RefreshPoints();
        }

        public virtual void SetPoints(int points)
        {
            Points = points;
            GUIManager.Instance.RefreshPoints();
        }

        protected virtual void SetActiveInventoryInputManager(bool status)
        {
            _inventoryInputManager = GameObject.FindAnyObjectByType<InventoryInputManager>();
            if (_inventoryInputManager != null)
            {
                _inventoryInputManager.enabled = status;
            }
        }

        public virtual void Pause(PauseMethods pauseMethod = PauseMethods.PauseMenu, bool unpauseIfPaused = true)
        {
            if ((pauseMethod == PauseMethods.PauseMenu) && _inventoryOpen)
            {
                return;
            }

            if (Time.timeScale > 0.0f)
            {
                MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
                Instance.Paused = true;
                if ((GUIManager.HasInstance) && (pauseMethod == PauseMethods.PauseMenu))
                {
                    GUIManager.Instance.SetPauseScreen(true);
                    _pauseMenuOpen = true;
                    SetActiveInventoryInputManager(false);
                }

                if (pauseMethod == PauseMethods.NoPauseMenu)
                {
                    _inventoryOpen = true;
                }
            }
            else
            {
                if (unpauseIfPaused)
                {
                    UnPause(pauseMethod);
                }
            }

            LevelManager.Instance.ToggleCharacterPause();
        }

        public virtual void UnPause(PauseMethods pauseMethod = PauseMethods.PauseMenu)
        {
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Unfreeze, 1f, 0f, false, 0f, false);
            Instance.Paused = false;
            if ((GUIManager.HasInstance) && (pauseMethod == PauseMethods.PauseMenu))
            {
                GUIManager.Instance.SetPauseScreen(false);
                _pauseMenuOpen = false;
                SetActiveInventoryInputManager(true);
            }

            if (_inventoryOpen)
            {
                _inventoryOpen = false;
            }

            LevelManager.Instance.ToggleCharacterPause();
        }

        public virtual void StorePointsOfEntry(string levelName, int entryIndex,
            Character.FacingDirections facingDirection)
        {
            if (PointsOfEntry.Count > 0)
            {
                foreach (PointsOfEntryStorage point in PointsOfEntry)
                {
                    if (point.LevelName == levelName)
                    {
                        point.PointOfEntryIndex = entryIndex;
                        return;
                    }
                }
            }

            PointsOfEntry.Add(new PointsOfEntryStorage(levelName, entryIndex, facingDirection));
        }

        public virtual PointsOfEntryStorage GetPointsOfEntry(string levelName)
        {
            if (PointsOfEntry.Count > 0)
            {
                foreach (PointsOfEntryStorage point in PointsOfEntry)
                {
                    if (point.LevelName == levelName)
                    {
                        return point;
                    }
                }
            }

            return null;
        }

        public virtual void ClearPointOfEntry(string levelName)
        {
            if (PointsOfEntry.Count > 0)
            {
                foreach (PointsOfEntryStorage point in PointsOfEntry)
                {
                    if (point.LevelName == levelName)
                    {
                        PointsOfEntry.Remove(point);
                    }
                }
            }
        }

        public virtual void ClearAllPointsOfEntry()
        {
            PointsOfEntry.Clear();
        }

        public virtual void ResetAllSaves()
        {
            MMSaveLoadManager.DeleteSaveFolder("InventoryEngine");
            MMSaveLoadManager.DeleteSaveFolder("TopDownEngine");
            MMSaveLoadManager.DeleteSaveFolder("MMAchievements");
        }

        public virtual void StoreSelectedCharacter(Character selectedCharacter)
        {
            StoredCharacter = selectedCharacter;
        }

        public virtual void ClearSelectedCharacter()
        {
            StoredCharacter = null;
        }

        public virtual void SetPersistentCharacter(Character newCharacter)
        {
            PersistentCharacter = newCharacter;
        }

        public virtual void DestroyPersistentCharacter()
        {
            if (PersistentCharacter != null)
            {
                Destroy(PersistentCharacter.gameObject);
                SetPersistentCharacter(null);
            }

            if (LevelManager.Instance.Players[0] != null)
            {
                if (LevelManager.Instance.Players[0].gameObject.MMGetComponentNoAlloc<CharacterPersistence>() != null)
                {
                    Destroy(LevelManager.Instance.Players[0].gameObject);
                }
            }
        }

        public void OnEnemyDeath(Enemy deadEnemy)
        {
            activeEnemies.Remove(deadEnemy);
            if (activeEnemies.Count == 0) currentEnemyFormation++;
        }
        
        public void OnPlayerDeath()
        {
            KillPlayer();
        }

        public bool GetPlayerStatus()
        {
            return  playerIsAlive;
        }
        



        public virtual void OnMMEvent(MMGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "inventoryOpens":
                    if (PauseGameWhenInventoryOpens)
                    {
                        Pause(PauseMethods.NoPauseMenu, false);
                    }

                    break;

                case "inventoryCloses":
                    if (PauseGameWhenInventoryOpens)
                    {
                        UnPause(PauseMethods.NoPauseMenu);
                    }

                    break;
            }
        }

        public virtual void OnMMEvent(TopDownEngineEvent engineEvent)
        {
            switch (engineEvent.EventType)
            {
                case TopDownEngineEventTypes.TogglePause:
                    if (Paused)
                    {
                        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
                    }
                    else
                    {
                        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.Pause, null);
                    }

                    break;
                case TopDownEngineEventTypes.Pause:
                    Pause();
                    break;
                case TopDownEngineEventTypes.UnPause:
                    UnPause();
                    break;
                case TopDownEngineEventTypes.PauseNoMenu:
                    Pause(PauseMethods.NoPauseMenu, false);
                    break;
            }
        }

        public virtual void OnMMEvent(TopDownEnginePointEvent pointEvent)
        {
            switch (pointEvent.PointsMethod)
            {
                case PointsMethods.Set:
                    SetPoints(pointEvent.Points);
                    break;

                case PointsMethods.Add:
                    AddPoints(pointEvent.Points);
                    break;
            }
        }
        
        public void OnMMEvent(EnemyDeath eventType)
        {
            if(activeEnemies.Contains(eventType.EnemyScript))
                OnEnemyDeath(eventType.EnemyScript);
        }
        
        
        public void OnMMEvent(PlayerDeath eventType)
        {
            KillPlayer();
        }

  

        protected virtual void OnEnable()
        {
            this.MMEventStartListening<MMGameEvent>();
            this.MMEventStartListening<TopDownEngineEvent>();
            this.MMEventStartListening<TopDownEnginePointEvent>();
            this.MMEventStartListening<EnemyDeath>();
            this.MMEventStartListening<PlayerDeath>();
        }

        protected virtual void OnDisable()
        {
            this.MMEventStopListening<MMGameEvent>();
            this.MMEventStopListening<TopDownEngineEvent>();
            this.MMEventStopListening<TopDownEnginePointEvent>();
            this.MMEventStopListening<PlayerDeath>();
        }


    }
}
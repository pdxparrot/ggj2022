using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game
{
    public interface IGameManager
    {
        #region Events

        event EventHandler<EventArgs> GameStartServerEvent;
        event EventHandler<EventArgs> GameStartClientEvent;

        event EventHandler<EventArgs> GameReadyEvent;
        event EventHandler<EventArgs> GameUnReadyEvent;
        event EventHandler<EventArgs> GameOverEvent;

        event EventHandler<EventArgs> RestartLevelEvent;

        event EventHandler<EventArgs> LevelTransitioningEvent;

        #endregion

        GameData GameData { get; }

        CreditsData CreditsData { get; }

        Settings Settings { get; }

        bool IsGameReady { get; }

        bool IsGameOver { get; }

        bool TransitionToHighScores { get; }

        [CanBeNull]
        LevelHelper LevelHelper { get; }

        void Initialize();

        void Shutdown();

        void RegisterLevelHelper(LevelHelper levelHelper);

        void UnRegisterLevelHelper(LevelHelper levelHelper);

        void StartGameServer();

        void StartGameClient();

        void GameReady();

        void GameUnReady();

        void LevelEntered();

        void LevelTransitioning();

        void GameOver();

        void RestartLevel();

        void TransitionScene(string nextScene, Action onComplete);
    }

    public abstract class GameManager<T> : SingletonBehavior<T>, IGameManager where T : GameManager<T>
    {
        #region Events

        public event EventHandler<EventArgs> GameStartServerEvent;
        public event EventHandler<EventArgs> GameStartClientEvent;

        public event EventHandler<EventArgs> GameReadyEvent;
        public event EventHandler<EventArgs> GameUnReadyEvent;
        public event EventHandler<EventArgs> GameOverEvent;

        public event EventHandler<EventArgs> RestartLevelEvent;

        public event EventHandler<EventArgs> LevelEnterEvent;
        public event EventHandler<EventArgs> LevelTransitioningEvent;

        #endregion

        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        [SerializeField]
        private CreditsData _creditsData;

        public CreditsData CreditsData => _creditsData;

        [SerializeField]
        private /*readonly*/ Settings _settings = new Settings();

        public Settings Settings => _settings;

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private LevelHelper _levelHelper;

        [CanBeNull]
        public LevelHelper LevelHelper => _levelHelper;

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _isGameReady;

        public virtual bool IsGameReady
        {
            get => _isGameReady;
            private set => _isGameReady = value;
        }

        [SerializeField]
        [ReadOnly]
        private bool _isGameOver;

        public virtual bool IsGameOver
        {
            get => _isGameOver;
            private set => _isGameOver = value;
        }

        [SerializeField]
        [ReadOnly]
        private bool _transitionToHighScores;

        public bool TransitionToHighScores
        {
            get => _transitionToHighScores;
            set => _transitionToHighScores = value;
        }

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterGameManager();
            }

            base.OnDestroy();
        }

        #endregion

        public virtual void Initialize()
        {
            // NOTE: don't call Reset() here
            IsGameOver = false;
            IsGameReady = false;
            TransitionToHighScores = false;

            InitializeObjectPools();
        }

        public virtual void Shutdown()
        {
            IsGameOver = false;
            IsGameReady = false;

            DestroyObjectPools();

            if(Core.Network.NetworkManager.Instance.IsServerActive() && null != GameStateManager.Instance.PlayerManager) {
                GameStateManager.Instance.PlayerManager.DestroyPlayers();
            }
        }

        public virtual void Reset()
        {
            Debug.Log("[Game] Resetting...");

            IsGameOver = false;
            IsGameReady = false;
            TransitionToHighScores = false;

            Settings.Reset();
        }

        #region Level Helper

        public void RegisterLevelHelper(LevelHelper levelHelper)
        {
            Assert.IsNull(_levelHelper);
            _levelHelper = levelHelper;
        }

        public void UnRegisterLevelHelper(LevelHelper levelHelper)
        {
            Assert.IsTrue(levelHelper == _levelHelper);
            _levelHelper = null;
        }

        #endregion

        protected virtual void InitializeObjectPools()
        {
            if(null != GameData.FloatingTextPrefab && null != GameStateManager.Instance.GameUIManager) {
                PooledObject pooledObject = GameData.FloatingTextPrefab.GetComponent<PooledObject>();
                ObjectPoolManager.Instance.InitializePoolAsync(GameStateManager.Instance.GameUIManager.DefaultFloatingTextPoolName, pooledObject, GameData.FloatingTextPoolSize);
            }
        }

        protected virtual void DestroyObjectPools()
        {
            if(ObjectPoolManager.HasInstance && null != GameStateManager.Instance.GameUIManager) {
                ObjectPoolManager.Instance.DestroyPool(GameStateManager.Instance.GameUIManager.DefaultFloatingTextPoolName);
            }
        }

        public virtual void StartGameServer()
        {
            Debug.Log("[Game] Server start...");

            GameStartServerEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void StartGameClient()
        {
            Debug.Log("[Game] Client start...");

            GameStartClientEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void GameReady()
        {
            Assert.IsFalse(IsGameOver);

            Debug.Log("[Game] Ready!");

            IsGameReady = true;

            GameReadyEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void GameUnReady()
        {
            Debug.Log("[Game] UnReady...");

            IsGameReady = false;

            GameUnReadyEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void LevelEntered()
        {
            Debug.Log("[Game] Level entered...");

            LevelEnterEvent?.Invoke(this, null);
        }

        public virtual void LevelTransitioning()
        {
            Assert.IsFalse(IsGameReady);
            Assert.IsFalse(IsGameOver);

            Debug.Log("[Game] Level transitioning...");

            LevelTransitioningEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void GameOver()
        {
            Debug.Log("[Game] Over!");

            IsGameOver = true;

            GameOverEvent?.Invoke(this, EventArgs.Empty);
        }

        public virtual void RestartLevel()
        {
            Debug.Log("[Game] Restart level...");

            GameUnReady();

            RestartLevelEvent?.Invoke(this, EventArgs.Empty);
        }

        // TODO: this isn't handled by networking *at all*
        public virtual void TransitionScene(string nextScene, Action onComplete)
        {
            Debug.Log("[Game] Transition scene...");

            PartyParrotManager.Instance.LoadingManager.ShowTransitionScreen(true);

            // TODO: do we need an event here to tell the level to cleanup?

            GameStateManager.Instance.CurrentState.ChangeSceneAsync(nextScene, () => {
                onComplete?.Invoke();

                // TODO: this might be wrong, not sure
                // basically this is trying to tell the new
                // LevelHelper that we're ready to start
                StartGameServer();
                StartGameClient();

                PartyParrotManager.Instance.LoadingManager.ShowTransitionScreen(false);
            });
        }
    }
}

#pragma warning disable 0618    // disable obsolete warning for now

using System;
using System.Collections;
using System.Collections.Generic;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Loading;
#if ENABLE_SERVER_SPECTATOR
using pdxpartyparrot.Game.Network;
#endif

using UnityEngine;
using UnityEngine.Assertions;

#if USE_NETWORKING
using Unity.Netcode;
#endif

namespace pdxpartyparrot.Game.State
{
    public abstract class MainGameState : GameState
    {
        [SerializeField]
        private GameOverState _gameOverState;

#if ENABLE_SERVER_SPECTATOR
        private ServerSpectator _serverSpectator;
#endif

        // this is only used when not using "gamepads are players"
        private readonly List<short> _playerControllers = new List<short>();

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            InitDebugMenu();
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();
        }

        #endregion

        protected override IEnumerator InitSceneRoutine()
        {
            if(null == GameStateManager.Instance.GameManager.LevelHelper) {
                Debug.LogWarning("LevelHelper is missing! Players will not spawn without one!");
            }

#if USE_NAVMESH
            if(null != GameStateManager.Instance.GameManager && null != GameStateManager.Instance.GameManager.LevelHelper) {
                IEnumerator runner = GameStateManager.Instance.GameManager.LevelHelper.BuildNavMesh();
                while(runner.MoveNext()) {
                    yield return null;
                }
            }
#endif

            yield break;
        }

        public override IEnumerator<LoadStatus> OnEnterRoutine()
        {
            yield return new LoadStatus(0.0f, "Entering main game state...");

            IEnumerator<LoadStatus> runner = base.OnEnterRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(0.5f, "Entering main game state...");

            runner = InitializeRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

#if USE_NETWORKING
            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
            Core.Network.NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;
#endif

            yield return new LoadStatus(1.0f, "Main game state initialized!");
        }

        public override void OnUpdate(float dt)
        {
            if(GameStateManager.Instance.GameManager.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState, state => {
                    state.Initialize();
                });
            }
        }

        public override IEnumerator<LoadStatus> OnExitRoutine()
        {
            yield return new LoadStatus(0.0f, "Shutting down main game state...");

#if USE_NETWORKING
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
            }
#endif

            IEnumerator<LoadStatus> runner = ShutdownRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(0.5f, "Shutting down main game state...");

            runner = base.OnExitRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(1.0f, "Main game state shutdown!");
        }

        public override void OnExit()
        {
            if(null != GameStateManager.Instance.GameManager) {
                GameStateManager.Instance.GameManager.GameUnReady();
            }

            base.OnExit();
        }

        #region Initialize

        private IEnumerator<LoadStatus> InitializeRoutine()
        {
            yield return new LoadStatus(0.0f, "Initializing main game state...");

            PartyParrotManager.Instance.IsPaused = false;

            DebugMenuManager.Instance.ResetFrameStats();

            yield return new LoadStatus(0.25f, "Initializing main game state...");

            Assert.IsNotNull(GameStateManager.Instance.GameManager);
            GameStateManager.Instance.GameManager.Initialize();

            yield return new LoadStatus(0.5f, "Initializing main game state (server)...");
            InitializeServer();

            yield return new LoadStatus(0.75f, "Initializing main game state (client)...");
            InitializeClient();

            yield return new LoadStatus(1.0f, "Initializing main game state...");
        }

        protected virtual bool InitializeServer()
        {
            if(!Core.Network.NetworkManager.Instance.IsServerActive()) {
                return false;
            }

            Core.Network.NetworkManager.Instance.ServerChangedScene();

#if ENABLE_SERVER_SPECTATOR
            if(!NetworkClient.active && !PartyParrotManager.Instance.IsHeadless) {
                ViewerManager.Instance.AllocateViewers(1, GameStateManager.Instance.ServerSpectatorViewerPrefab);

                _serverSpectator = Instantiate(GameStateManager.Instance.ServerSpectatorPrefab);
            }
#endif

            return true;
        }

        protected virtual bool InitializeClient()
        {
            if(!Core.Network.NetworkManager.Instance.IsClientActive()) {
                return false;
            }

            if(null != GameStateManager.Instance.GameUIManager) {
                GameStateManager.Instance.GameUIManager.Initialize();
            } else {
                Debug.LogWarning("GameUIManager missing!");
            }

            Core.Network.NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection);

            if(GameStateManager.Instance.GameManager.GameData.SpawnPlayersOnLoad) {
                SpawnPlayers();
            }

            return true;
        }

        #endregion

        #region Shutdown

        private IEnumerator<LoadStatus> ShutdownRoutine()
        {
            yield return new LoadStatus(0.0f, "Shutting down main game state...");

            ShutdownClient();
            ShutdownServer();

            yield return new LoadStatus(0.25f, "Shutting down main game state...");

            if(GameStateManager.HasInstance) {
                if(null != GameStateManager.Instance.GameManager) {
                    GameStateManager.Instance.GameManager.Shutdown();
                }

                GameStateManager.Instance.ShutdownNetwork();
            }

            yield return new LoadStatus(0.75f, "Shutting down main game state...");

            PartyParrotManager.Instance.IsPaused = false;

            yield return new LoadStatus(1.0f, "Shutting down main game state...");
        }

        protected virtual void ShutdownServer()
        {
#if ENABLE_SERVER_SPECTATOR
            if(null != _serverSpectator) {
                Destroy(_serverSpectator.gameObject);
            }
            _serverSpectator = null;
#endif
        }

        protected virtual void ShutdownClient()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.FreeAllViewers();
            }

            if(GameStateManager.HasInstance && null != GameStateManager.Instance.GameUIManager) {
                GameStateManager.Instance.GameUIManager.Shutdown();
            }

            if(AudioManager.HasInstance) {
                AudioManager.Instance.StopAllMusic();
            }

            _playerControllers.Clear();
        }

        #endregion

        // this is only used when not "gamepads are players"
        public bool AddPlayerController(short playerControllerId)
        {
            if(!Core.Network.NetworkManager.Instance.IsClientActive()) {
                return false;
            }

            if(_playerControllers.Count >= GameStateManager.Instance.GameManager.GameData.MaxLocalPlayers) {
                return false;
            }

            _playerControllers.Add(playerControllerId);

            return true;
        }

        public void SpawnPlayers()
        {
            // TODO: this probably isn't the right place to handle "gamepads are players"
            // instead it probably should be done in whatever initializes the main game state
            if(GameStateManager.Instance.GameManager.GameData.GamepadsArePlayers) {
                ulong count = (ulong)Mathf.Min(Mathf.Max(InputManager.Instance.GetGamepadCount(), 1), GameStateManager.Instance.GameManager.GameData.MaxLocalPlayers);
                if(count < 1) {
                    Debug.LogWarning("No player controllers available!");
                } else {
                    Debug.Log($"Will spawn a player for each controller ({count})...");
                }

                for(ulong i = 0; i < count; ++i) {
                    Core.Network.NetworkManager.Instance.AddLocalPlayer(i);
                }
            } else {
                if(_playerControllers.Count < 1) {
                    Debug.LogWarning("No player controllers available!");
                }

                foreach(ulong clientId in _playerControllers) {
                    Debug.Log($"Spawning local player with controller {clientId}...");
                    Core.Network.NetworkManager.Instance.AddLocalPlayer(clientId);
                }
            }
        }

        #region Event Handlers

        private void ServerDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: server disconnect");
        }

        private void ClientDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: client disconnect");
        }

        #endregion

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Game.MainGameState");
            _debugMenuNode.RenderContentsAction = () => {
                if(GUILayout.Button("Game Over")) {
                    GameStateManager.Instance.GameManager.GameOver();
                }
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }

        #endregion
    }
}

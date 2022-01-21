﻿using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.State;

using UnityEngine;

#if USE_NAVMESH
using UnityEngine.AI;
#endif

namespace pdxpartyparrot.Game.Level
{
#if USE_NAVMESH
    [RequireComponent(typeof(NavMeshSurface))]
#endif
    public abstract class LevelHelper : MonoBehaviour
    {
        [SerializeField]
        private string _nextLevel;

        public bool HasNextLevel => !string.IsNullOrWhiteSpace(_nextLevel);

        #region Effects

        [Space(10)]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _levelEnterEffect;

        [CanBeNull]
        protected EffectTrigger LevelEnterEffect => _levelEnterEffect;

        [SerializeField]
        private bool _levelEnterIsBlocking = true;

        protected bool LevelEnterIsBlocking => _levelEnterIsBlocking;

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _levelExitEffect;

        [CanBeNull]
        protected EffectTrigger LevelExitEffect => _levelExitEffect;

        [SerializeField]
        private bool _levelExitIsBlocking = true;

        protected bool LevelExitIsBlocking => _levelExitIsBlocking;

        #endregion

#if USE_NAVMESH
        private NavMeshSurface _navMeshSurface;
#endif

        #region Unity Lifecycle

        protected virtual void Awake()
        {
#if USE_NAVMESH
            _navMeshSurface = GetComponent<NavMeshSurface>();
#endif

            GameStateManager.Instance.GameManager.RegisterLevelHelper(this);

            GameStateManager.Instance.GameManager.GameStartServerEvent += GameStartServerEventHandler;
            GameStateManager.Instance.GameManager.GameStartClientEvent += GameStartClientEventHandler;
            GameStateManager.Instance.GameManager.GameReadyEvent += GameReadyEventHandler;
            GameStateManager.Instance.GameManager.GameUnReadyEvent += GameUnReadyEventHandler;
            GameStateManager.Instance.GameManager.LevelTransitioningEvent += LevelTransitioningEventHandler;
            GameStateManager.Instance.GameManager.GameOverEvent += GameOverEventHandler;
            GameStateManager.Instance.GameManager.RestartLevelEvent += RestartLevelEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(GameStateManager.HasInstance && null != GameStateManager.Instance.GameManager) {
                GameStateManager.Instance.GameManager.RestartLevelEvent -= RestartLevelEventHandler;
                GameStateManager.Instance.GameManager.GameOverEvent -= GameOverEventHandler;
                GameStateManager.Instance.GameManager.LevelTransitioningEvent -= LevelTransitioningEventHandler;
                GameStateManager.Instance.GameManager.GameUnReadyEvent -= GameUnReadyEventHandler;
                GameStateManager.Instance.GameManager.GameReadyEvent -= GameReadyEventHandler;
                GameStateManager.Instance.GameManager.GameStartClientEvent -= GameStartClientEventHandler;
                GameStateManager.Instance.GameManager.GameStartServerEvent -= GameStartServerEventHandler;

                GameStateManager.Instance.GameManager.UnRegisterLevelHelper(this);
            }
        }

        protected virtual void Update()
        {
            // TODO: if we're in a blocking enter / exit,
            // based on a flag (can skip or something),
            // allow exiting out of the blocking trigger early
        }

        #endregion

        private void TriggerLevelEffect(EffectTrigger effectTrigger, bool isBlocking, Action action)
        {
            if(null != effectTrigger) {
                if(isBlocking) {
                    effectTrigger.Trigger(() => {
                        action?.Invoke();
                    });
                } else {
                    effectTrigger.Trigger();
                    action?.Invoke();
                }
            } else {
                action?.Invoke();
            }
        }

        private void TriggerEnterLevelEffect(Action action)
        {
            TriggerLevelEffect(_levelEnterEffect, _levelEnterIsBlocking, action);
        }

        private void TriggerExitLevelEffect(Action action)
        {
            TriggerLevelEffect(_levelExitEffect, _levelExitIsBlocking, action);
        }

        protected void TransitionLevel()
        {
            GameStateManager.Instance.GameManager.GameUnReady();

            // load the next level if we have one
            if(!string.IsNullOrWhiteSpace(_nextLevel)) {
                TriggerExitLevelEffect(DoLevelTransition);
            } else {
                GameStateManager.Instance.GameManager.GameOver();
            }
        }

        private void DoLevelTransition()
        {
            GameStateManager.Instance.GameManager.LevelTransitioning();

            GameStateManager.Instance.GameManager.TransitionScene(_nextLevel, null);
        }

#if USE_NAVMESH
        public IEnumerator BuildNavMesh()
        {
            Debug.Log("[Level] Building nav mesh...");

            // https://github.com/Unity-Technologies/NavMeshComponents/issues/97
            _navMeshSurface.RemoveData();
            _navMeshSurface.navMeshData = new NavMeshData(_navMeshSurface.agentTypeID) {
                name = _navMeshSurface.gameObject.name,
                position = _navMeshSurface.transform.position,
                rotation = _navMeshSurface.transform.rotation
            };
            _navMeshSurface.AddData();

            AsyncOperation asyncOp = _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
            while(!asyncOp.isDone) {
                yield return null;
            }
        }
#endif

        protected virtual void Reset()
        {
            GameStateManager.Instance.PlayerManager.DespawnPlayers();
        }

        #region Event Handlers

        protected virtual void GameStartServerEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Server start...");

            SpawnManager.Instance.Initialize();
        }

        protected virtual void GameStartClientEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Client start...");

            // TODO: we really should communicate our ready state to the server
            // and then have it communicate back to us when everybody is ready
            TriggerEnterLevelEffect(GameStateManager.Instance.GameManager.GameReady);
        }

        protected virtual void GameReadyEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Game ready...");

            GameStateManager.Instance.PlayerManager.RespawnPlayers();

            GameStateManager.Instance.GameManager.LevelEntered();
        }

        protected virtual void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Game unready...");
        }

        protected virtual void LevelTransitioningEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Level transitioning...");

            GameStateManager.Instance.PlayerManager.DespawnPlayers();
        }

        protected virtual void GameOverEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Game over...");
        }

        protected virtual void RestartLevelEventHandler(object sender, EventArgs args)
        {
            Debug.Log("[Level] Restart...");

            TriggerExitLevelEffect(() => {
                Reset();

                // TODO: we really should communicate our ready state to the server
                // and then have it communicate back to us when everybody is ready
                TriggerEnterLevelEffect(GameStateManager.Instance.GameManager.GameReady);
            });
        }

        #endregion
    }
}

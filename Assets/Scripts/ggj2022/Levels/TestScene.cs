using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2022.NPCs;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2022.Level
{
    public sealed class TestScene : TestSceneHelper, ILevel
    {
        [SerializeField]
        private Key _spawnEnemiesKey = Key.L;

        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _enemyContainer;

        [SerializeField]
        [ReadOnly]
        private ILevel.State _previousWorldState = ILevel.State.Alive;

        public ILevel.State WorldState => GameManager.Instance.WorldState;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.TransitionUpdateEvent += TransitionUpdateEventHandler;
        }

        protected override void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.TransitionUpdateEvent -= TransitionUpdateEventHandler;
            }

            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            if(Keyboard.current[_spawnEnemiesKey].wasPressedThisFrame) {
                NPCManager.Instance.SpawnEnemies(_enemyContainer.transform);
            }
        }

        #endregion

        protected override void Reset()
        {
            base.Reset();

            if(null != _enemyContainer) {
                Destroy(_enemyContainer);
                _enemyContainer = null;
            }

            _previousWorldState = ILevel.State.Alive;
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_enemyContainer);
            _enemyContainer = new GameObject("Enemies");

            GameManager.Instance.LevelEntered();
        }

        private void TransitionUpdateEventHandler(object sender, TransitionUpdateEventArgs args)
        {
            if(!args.IsGlobal || GameManager.Instance.WorldState == _previousWorldState) {
                return;
            }

            TriggerScriptEvent("TransitionUpdate");

            _previousWorldState = GameManager.Instance.WorldState;
        }

        #endregion
    }
}

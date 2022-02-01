using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2022.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.Level
{
    public sealed class Level : LevelHelper, ILevel
    {
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

            NPCManager.Instance.SpawnEnemies(_enemyContainer.transform);

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

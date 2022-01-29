using System;

using JetBrains.Annotations;

using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2022.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.Level
{
    public sealed class Level : LevelHelper
    {
        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _enemyContainer;

        protected override void Reset()
        {
            base.Reset();

            if(null != _enemyContainer) {
                Destroy(_enemyContainer);
                _enemyContainer = null;
            }
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

        #endregion
    }
}

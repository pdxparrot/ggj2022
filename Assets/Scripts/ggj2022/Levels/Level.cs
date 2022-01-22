using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;

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

        private void SpawnEnemies()
        {
            int spawnCount = SpawnManager.Instance.SpawnPointCount(GameManager.Instance.GameGameData.SlimeSpawnTag);

            /*Debug.Log($"Spawning {slimeCount} slimes");

            for(int i = 0; i < slimeCount; ++i) {
                SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SlimeSpawnTag);
                spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SlimePrefab, GameManager.Instance.GameGameData.SlimeBehaviorData, _enemyContainer.transform);
            }*/
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_enemyContainer);
            _enemyContainer = new GameObject("Enemies");

            SpawnEnemies();

            GameManager.Instance.LevelEntered();
        }

        #endregion
    }
}

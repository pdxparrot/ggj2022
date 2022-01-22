using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2022.Level
{
    public sealed class TestScene : TestSceneHelper
    {
        [SerializeField]
        private Key _spawnEnemiesKey = Key.L;

        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _enemyContainer;

        #region Unity Lifecycle

        private void FixedUpdate()
        {
            if(Keyboard.current[_spawnEnemiesKey].wasPressedThisFrame) {
                SpawnEnemies();
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
        }

        private void SpawnEnemies()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SlimeSpawnTag);
            spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SlimePrefab, GameManager.Instance.GameGameData.SlimeBehaviorData, _enemyContainer.transform);
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_enemyContainer);
            _enemyContainer = new GameObject("Enemies");

            GameManager.Instance.LevelEntered();
        }

        #endregion
    }
}

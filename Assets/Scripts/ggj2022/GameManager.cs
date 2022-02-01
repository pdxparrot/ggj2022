using System;
using System.Collections.Generic;

using UnityEngine;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2022.Camera;
using pdxpartyparrot.ggj2022.Data;
using pdxpartyparrot.ggj2022.UI;

namespace pdxpartyparrot.ggj2022
{
    public sealed class GameManager : GameManager<GameManager>
    {
        #region Events

        public event EventHandler<TransitionUpdateEventArgs> TransitionUpdateEvent;

        #endregion

        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private int _totalEnemyCount;

        private readonly Dictionary<string, int> _areaEnemyCount = new Dictionary<string, int>();

        [SerializeField]
        [ReadOnly]
        private int _stompedEnemyCount;

        private readonly Dictionary<string, int> _areaStompedEnemyCount = new Dictionary<string, int>();

        [SerializeField]
        [ReadOnly]
        private int _planterCount;

        private readonly Dictionary<string, int> _areaPlantersCount = new Dictionary<string, int>();

        [SerializeField]
        [ReadOnly]
        private int _totalSeedCount;

        [SerializeField]
        [ReadOnly]
        private int _collectedSeedCount;

        [SerializeField]
        [ReadOnly]
        private int _plantedSeedCount;

        private readonly Dictionary<string, int> _areaPlantedSeedCount = new Dictionary<string, int>();

        public bool ExitAvailable => _totalSeedCount > 0 && _collectedSeedCount >= _totalSeedCount;

        public bool PlantingAllowed => _totalEnemyCount > 0 && _stompedEnemyCount >= _totalEnemyCount;

        public bool AllSeedsPlanted => _totalSeedCount > 0 && _plantedSeedCount >= _totalSeedCount;

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }

        public void Reset(int maxHealth, int health)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.Reset(maxHealth, health);

            _totalEnemyCount = 0;
            _areaEnemyCount.Clear();

            _stompedEnemyCount = 0;
            _areaStompedEnemyCount.Clear();

            // don't reset the number of planters, those are static

            _totalSeedCount = 0;
            _collectedSeedCount = 0;

            _plantedSeedCount = 0;
            _areaPlantedSeedCount.Clear();

            UpdateAreaTransitions(string.Empty);
        }

        public void Exit()
        {
            GameOver();
        }

        private void UpdateAreaTransitions(string areaId)
        {
            // update for the area
            TransitionUpdateEvent?.Invoke(this, new TransitionUpdateEventArgs(
                areaId,
                _areaStompedEnemyCount.GetValueOrDefault(areaId),
                _areaEnemyCount.GetValueOrDefault(areaId),
                _areaPlantedSeedCount.GetValueOrDefault(areaId),
                _areaPlantersCount.GetValueOrDefault(areaId)
            ));

            // update for global
            TransitionUpdateEvent?.Invoke(this, new TransitionUpdateEventArgs(
                _stompedEnemyCount,
                _totalEnemyCount,
                _plantedSeedCount,
                _planterCount
            ));
        }

        #region Player

        public void PlayerDamaged(int health)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateHealth(health);
        }

        public void PlayerDied()
        {
            Debug.Log("Player died! Restarting level...");

            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateHealth(0);

            RestartLevel();
        }

        #endregion

        #region Enemies

        public void EnemySpawned(string areaId)
        {
            Debug.Log($"Enemy spawned in area {areaId}");

            _totalEnemyCount++;
            _areaEnemyCount[areaId] = _areaEnemyCount.GetValueOrDefault(areaId) + 1;

            UpdateAreaTransitions(areaId);
        }

        public void EnemyStomped(string areaId)
        {
            Debug.Log($"Enemy stomped in area {areaId}");

            _stompedEnemyCount++;
            _areaStompedEnemyCount[areaId] = _areaStompedEnemyCount.GetValueOrDefault(areaId) + 1;

            UpdateAreaTransitions(areaId);
        }

        #endregion

        #region Seeds

        public void RegisterPlanter(string areaId)
        {
            _planterCount++;
            _areaPlantersCount[areaId] = _areaPlantersCount.GetValueOrDefault(areaId) + 1;
        }

        public void UnRegisterPlanter(string areaId)
        {
            _planterCount--;
            _areaPlantersCount[areaId] = _areaPlantersCount.GetValueOrDefault(areaId) - 1;
        }

        public void SeedSpawned()
        {
            _totalSeedCount++;
        }

        public void SeedCollected()
        {
            _collectedSeedCount++;

            GameUIManager.Instance.GameGameUI.PlayerHUD.SeedCollected();
        }

        public void SeedPlanted(string areaId)
        {
            Debug.Log($"Seed planted in area {areaId}");

            _plantedSeedCount++;
            _areaPlantedSeedCount[areaId] = _areaPlantedSeedCount.GetValueOrDefault(areaId) + 1;

            UpdateAreaTransitions(areaId);

            GameUIManager.Instance.GameGameUI.PlayerHUD.SeedPlanted();
        }

        #endregion
    }
}

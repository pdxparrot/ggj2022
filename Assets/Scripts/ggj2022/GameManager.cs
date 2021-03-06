using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2022.Camera;
using pdxpartyparrot.ggj2022.Data;
using pdxpartyparrot.ggj2022.Level;
using pdxpartyparrot.ggj2022.UI;

namespace pdxpartyparrot.ggj2022
{
    public sealed class GameManager : GameManager<GameManager>
    {
        #region Events

        public event EventHandler<TransitionUpdateEventArgs> TransitionUpdateEvent;
        public event EventHandler<EventArgs> ExitAvailableChangedEvent;
        public event EventHandler<EventArgs> PlantersAvailableChangedEvent;

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

        [SerializeField]
        [ReadOnly]
        private ILevel.State _worldState = ILevel.State.Alive;

        public ILevel.State WorldState => _worldState;

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

        public void ResetHUD(int maxHealth, int health)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.Reset(maxHealth, health);
        }

        public override void GameUnReady()
        {
            base.GameUnReady();

            _totalEnemyCount = 0;
            _areaEnemyCount.Clear();

            _stompedEnemyCount = 0;
            _areaStompedEnemyCount.Clear();
            PlantersAvailableChangedEvent?.Invoke(this, EventArgs.Empty);

            // don't reset the number of planters, those are static

            _totalSeedCount = 0;
            _collectedSeedCount = 0;
            ExitAvailableChangedEvent?.Invoke(this, EventArgs.Empty);

            _plantedSeedCount = 0;
            _areaPlantedSeedCount.Clear();

            UpdateAreaTransitions(string.Empty);
        }

        public void Exit()
        {
            GameOver();
        }

        private void UpdateWorldState()
        {
            _worldState = _planterCount == 0 || _plantedSeedCount >= _planterCount ? ILevel.State.Alive : ILevel.State.Ash;
            _worldState = _totalEnemyCount == 0 || _stompedEnemyCount >= _totalEnemyCount ? ILevel.State.Ash : ILevel.State.OnFire;

            TransitionUpdateEvent?.Invoke(this, new TransitionUpdateEventArgs(
                _stompedEnemyCount,
                _totalEnemyCount,
                _plantedSeedCount,
                _planterCount

            ));
        }

        private void UpdateAreaTransitions(string areaId)
        {
            if(areaId.Any()) {
                TransitionUpdateEvent?.Invoke(this, new TransitionUpdateEventArgs(
                    areaId,
                    _areaStompedEnemyCount.GetValueOrDefault(areaId),
                    _areaEnemyCount.GetValueOrDefault(areaId),
                    _areaPlantedSeedCount.GetValueOrDefault(areaId),
                    _areaPlantersCount.GetValueOrDefault(areaId)
                ));
            }

            UpdateWorldState();
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
            PlantersAvailableChangedEvent?.Invoke(this, EventArgs.Empty);

            _areaEnemyCount[areaId] = _areaEnemyCount.GetValueOrDefault(areaId) + 1;

            UpdateAreaTransitions(areaId);
        }

        public void EnemyStomped(string areaId)
        {
            Debug.Log($"Enemy stomped in area {areaId}");

            _stompedEnemyCount++;
            PlantersAvailableChangedEvent?.Invoke(this, EventArgs.Empty);

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
            ExitAvailableChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void SeedCollected()
        {
            _collectedSeedCount++;
            ExitAvailableChangedEvent?.Invoke(this, EventArgs.Empty);

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

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

            _totalSeedCount = 0;
            _collectedSeedCount = 0;

            _plantedSeedCount = 0;
            _areaPlantedSeedCount.Clear();
        }

        public void Exit()
        {
            GameOver();
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
            _totalEnemyCount++;
            _areaEnemyCount[areaId] = _areaEnemyCount.GetValueOrDefault(areaId) + 1;
        }

        public void EnemyStomped(string areaId)
        {
            _stompedEnemyCount++;
            _areaStompedEnemyCount[areaId] = _areaStompedEnemyCount.GetValueOrDefault(areaId) + 1;
        }

        #endregion

        #region Seeds

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
            _plantedSeedCount++;
            _areaPlantedSeedCount[areaId] = _areaPlantedSeedCount.GetValueOrDefault(areaId) + 1;

            GameUIManager.Instance.GameGameUI.PlayerHUD.SeedPlanted();
        }

        #endregion
    }
}

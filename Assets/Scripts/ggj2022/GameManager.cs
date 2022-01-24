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

        [SerializeField]
        [ReadOnly]
        private int _totalEnemyCount;

        [SerializeField]
        [ReadOnly]
        private int _stompedEnemyCount;

        [SerializeField]
        [ReadOnly]
        private int _totalSeedCount;

        [SerializeField]
        [ReadOnly]
        private int _collectedSeedCount;

        public bool ExitAvailable => _totalSeedCount > 0 && _collectedSeedCount >= _totalSeedCount;

        public bool PlantingAllowed => _totalEnemyCount > 0 && _stompedEnemyCount >= _totalEnemyCount;

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
            _stompedEnemyCount = 0;

            _totalSeedCount = 0;
            _collectedSeedCount = 0;
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

        public void EnemySpawned()
        {
            _totalEnemyCount++;
        }

        public void EnemyStomped()
        {
            _stompedEnemyCount++;
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

        public void SeedPlanted()
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.SeedPlanted();
        }

        #endregion
    }
}

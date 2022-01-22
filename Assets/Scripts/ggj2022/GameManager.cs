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
        private int _seedCount = 0;

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }

        public void Reset(int health, int seedCount)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.Reset(health, seedCount);

            _seedCount = 0;
        }

        public void SeedSpawned()
        {
            _seedCount++;
        }

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

        public void SeedCollected(int seedCount)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateSeedCount(seedCount);

            _seedCount--;
            if(seedCount <= 0) {
                GameOver();
            }
        }
    }
}

using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private TextMeshProUGUI _health;

        [SerializeField]
        private TextMeshProUGUI _seedCount;

        public void Reset(int health, int seedCount)
        {
            UpdateHealth(health);
            UpdateSeedCount(seedCount);
        }

        public void UpdateHealth(int health)
        {
            _health.text = health.ToString();
        }

        public void UpdateSeedCount(int seedCount)
        {
            _seedCount.text = seedCount.ToString();
        }
    }
}

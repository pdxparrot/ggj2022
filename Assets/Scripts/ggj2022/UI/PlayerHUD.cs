using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private HealthBar _healthBar;

        [SerializeField]
        private TextMeshProUGUI _seedCount;

        public void Reset(int health, int seedCount)
        {
            _healthBar.ResetHealth(health);
            UpdateSeedCount(seedCount);
        }

        public void UpdateHealth(int health)
        {
            _healthBar.UpdateHealth(health);
        }

        public void UpdateSeedCount(int seedCount)
        {
            _seedCount.text = seedCount.ToString();
        }
    }
}
